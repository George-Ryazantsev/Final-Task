using System.Data;
namespace Trenning_NotificationsExample.Services
{
    public class StreamFileComparer
    {           
        private readonly PassportUpdateService _passportUpdateService;
        private const int batchSize = 300;
        public StreamFileComparer(PassportUpdateService passportUpdateService)
        {
            _passportUpdateService = passportUpdateService;
        }

        public async Task CompareFilesAsync(string file1Path, string file2Path)
        {
            
            var file1Lines = File.ReadLinesAsync(file1Path).GetAsyncEnumerator();
            var file2Lines = File.ReadLinesAsync(file2Path).GetAsyncEnumerator();

            DataTable removedPassports = new DataTable();
            DataTable addedPassports = new DataTable();

            removedPassports.Columns.Add("Series", typeof(string));
            removedPassports.Columns.Add("Number", typeof(string));

            addedPassports.Columns.Add("Series", typeof(string));
            addedPassports.Columns.Add("Number", typeof(string));

            Console.WriteLine("Сравнение файлов начато...");                                 

            bool file1HasLines = await file1Lines.MoveNextAsync();
            bool file2HasLines = await file2Lines.MoveNextAsync();           

            while (file1HasLines || file2HasLines)
            {                
                HashSet<string> batch1 = new HashSet<string>();
                HashSet<string> batch2 = new HashSet<string>();
                
                for (int i = 0; i < batchSize && file1HasLines; i++)
                {
                    file1HasLines = await file1Lines.MoveNextAsync();
                    if (file1HasLines)
                        batch1.Add(file1Lines.Current);                    
                }
                
                for (int i = 0; i < batchSize && file2HasLines; i++)
                {
                    file2HasLines = await file2Lines.MoveNextAsync();
                    if (file2HasLines)
                        batch2.Add(file2Lines.Current);                    
                }

                // удалённые строки (есть в batch1, но нет в batch2)
                var removed = batch1.Except(batch2);
                foreach (var line in removed)
                {
                    AddToTable(removedPassports, line);
                }

                // добавленные строки (есть в batch2, но нет в batch1)
                var added = batch2.Except(batch1);
                foreach (var line in added)
                {
                    AddToTable(addedPassports, line);
                }
                
                batch1.Clear();
                batch2.Clear();
                
                 await _passportUpdateService.BatchUpdate(removedPassports, addedPassports);
                Console.WriteLine($" Обновлено {batchSize} строк");

                if (removedPassports.Rows.Count>= batchSize) removedPassports.Clear();
                if (addedPassports.Rows.Count >= batchSize) addedPassports.Clear();
            }          
        }

        private void AddToTable(DataTable table, string line)
        {            
            var parts = line.Split(',');

            DataRow row = table.NewRow();
            row["Series"] = parts[0].Trim();
            row["Number"] = parts[1].Trim();

            table.Rows.Add(row);          
        }
    }
}


