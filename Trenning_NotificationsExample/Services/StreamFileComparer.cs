using Trenning_NotificationsExample.Models;
namespace Trenning_NotificationsExample.Services
{
    public class StreamFileComparer
    {        
        private readonly PassportChangesService _passportChangesService;
        private const int batchSize = 7000;                
        public StreamFileComparer(PassportChangesService passportChangesService)
        {            
            _passportChangesService=passportChangesService;
        }

        public async Task CompareFilesAsync(string file1Path, string file2Path)
        {
            var file1Lines = File.ReadLinesAsync(file1Path).GetAsyncEnumerator();
            var file2Lines = File.ReadLinesAsync(file2Path).GetAsyncEnumerator();

            Console.WriteLine("Сравнение файлов начато...");
            
            bool file1HasLines = await file1Lines.MoveNextAsync();
            bool file2HasLines = await file2Lines.MoveNextAsync();

            while (file1HasLines || file2HasLines)
            {
                HashSet<string> batch1 = new HashSet<string>();
                HashSet<string> batch2 = new HashSet<string>();
              
                for (int i = 0; i < batchSize && file1HasLines; i++)
                {
                    batch1.Add(file1Lines.Current);
                    file1HasLines = await file1Lines.MoveNextAsync();
                }
               
                for (int i = 0; i < batchSize && file2HasLines; i++)
                {
                    batch2.Add(file2Lines.Current);
                    file2HasLines = await file2Lines.MoveNextAsync();
                }
                
                var removed = batch1.Except(batch2);
                foreach (var line in removed)
                {
                    var change = ParsePassportChange(line, "Removed");                    
                    await _passportChangesService.WriteFileToDb(change);
                }
               
                var added = batch2.Except(batch1);
                foreach (var line in added)
                {
                    var change = ParsePassportChange(line, "Added");                    
                    await _passportChangesService.WriteFileToDb(change);
                }

                if(batch1.Count>=batchSize) batch1.Clear(); 
                if(batch2.Count>=batchSize) batch2.Clear();                                 

            }

            Console.WriteLine("Сравнение файлов завершено.");
        }
        private PassportChanges ParsePassportChange(string line, string changeType)
        {
            var parts = line.Split(',');
            return new PassportChanges
            {
                Series = parts[0].Trim(),
                Number = parts[1].Trim(),
                ChangeType = changeType,
                ChangeDate = DateTime.Now
            };

        }
    }
}


