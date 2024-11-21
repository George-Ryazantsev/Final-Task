using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Trenning_NotificationsExample.Services
{
    public class PassportUpdateService
    {        
        private readonly string _connectionString;
        private const int batchSize = 5000;

        public PassportUpdateService(IConfiguration configuration)
        {            
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }      

        public async Task WtiteFirstFileToDb(string filePath)
        {
            string line;
            int counter = 0;

            using var reader = new StreamReader(filePath);

            await reader.ReadLineAsync();

            DataTable passportDataTable = new DataTable();

            passportDataTable.Columns.Add("Series", typeof(string));
            passportDataTable.Columns.Add("Number", typeof(string));


            while ((line = await reader.ReadLineAsync()) != null)
            {

                var parts = line.Split(',');

                var series = parts[0].Trim();
                var number = parts[1].Trim();

                DataRow row = passportDataTable.NewRow();
                row["Series"] = parts[0].Trim();
                row["Number"] = parts[1].Trim();

                passportDataTable.Rows.Add(row);

                if (counter >= batchSize)
                {
                    BulkInsert(passportDataTable);

                    passportDataTable.Clear();
                    counter = 0;
                }

                counter++;
            }
        }
      
        public async Task BatchUpdate(DataTable passportRemoveDataTable, DataTable passportAddDataTable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("DeletePassports", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    SqlParameter tableParam = command.Parameters.AddWithValue("@PassportTable", passportRemoveDataTable);
                    tableParam.SqlDbType = SqlDbType.Structured;

                    await command.ExecuteNonQueryAsync();
                }

                await BulkInsert(passportAddDataTable);
                await UpdatePassportchanges(connection, passportRemoveDataTable, passportAddDataTable);
            }          
        }
        private async Task BulkInsert(DataTable dataTable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.InsertCommand = new SqlCommand(
                "INSERT INTO InactivePassports (Series, Number) VALUES (@Series, @Number);",
                 connection);
                adapter.InsertCommand.Parameters.Add("@Series", SqlDbType.NVarChar, 10, "Series");
                adapter.InsertCommand.Parameters.Add("@Number", SqlDbType.NVarChar, 10, "Number");
                adapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                adapter.Update(dataTable);
            }
        }

        private async Task UpdatePassportchanges(SqlConnection connection, DataTable passportRemoveDataTable, DataTable passportAddDataTable)
        {

            // "Removed"
            using (SqlCommand removeCommand = new SqlCommand(
                "INSERT INTO PassportChanges (Series, Number, ChangeType, ChangeDate) VALUES (@Series, @Number, 'Removed', @ChangeDate);",
                connection))
            {
                removeCommand.Parameters.Add("@Series", SqlDbType.NVarChar, 10);
                removeCommand.Parameters.Add("@Number", SqlDbType.NVarChar, 10);
                removeCommand.Parameters.Add("@ChangeDate", SqlDbType.DateTime);

                foreach (DataRow row in passportRemoveDataTable.Rows)
                {
                    removeCommand.Parameters["@Series"].Value = row["Series"];
                    removeCommand.Parameters["@Number"].Value = row["Number"];
                    removeCommand.Parameters["@ChangeDate"].Value = DateTime.Now;
                    await removeCommand.ExecuteNonQueryAsync();
                }
            }
            
            using (SqlCommand addCommand = new SqlCommand(
                "INSERT INTO PassportChanges (Series, Number, ChangeType, ChangeDate) VALUES (@Series, @Number, 'Added', @ChangeDate);",
                connection))
            {
                addCommand.Parameters.Add("@Series", SqlDbType.NVarChar, 10);
                addCommand.Parameters.Add("@Number", SqlDbType.NVarChar, 10);
                addCommand.Parameters.Add("@ChangeDate", SqlDbType.DateTime);

                foreach (DataRow row in passportAddDataTable.Rows)
                {
                    addCommand.Parameters["@Series"].Value = row["Series"];
                    addCommand.Parameters["@Number"].Value = row["Number"];
                    addCommand.Parameters["@ChangeDate"].Value = DateTime.Now;
                    await addCommand.ExecuteNonQueryAsync();
                }
            }
        }
    }
}