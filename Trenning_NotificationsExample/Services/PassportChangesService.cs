using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trenning_NotificationsExample.Models;
using Trenning_NotificationsExample.MongoDB;

namespace Trenning_NotificationsExample.Services
{
    public class PassportChangesService
    {
        private readonly IMongoCollection<PassportChanges> _passportChangesCollection;

        public PassportChangesService(
            IOptions<PassportsDatabaseSettings> passportsDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                passportsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                passportsDatabaseSettings.Value.DatabaseName);

            _passportChangesCollection = mongoDatabase.GetCollection<PassportChanges>(
                passportsDatabaseSettings.Value.PassportsCollectionName);
        }
        public async Task<IEnumerable<PassportChanges>> GetAllChangesAsync()
        {
            return await _passportChangesCollection.Find(_ => true).ToListAsync();
        }

        public async Task<PassportChanges> GetLastChangeAsync(string series, string number)
        {
         
            /*var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Passports");
            var collection = database.GetCollection<PassportChanges>("PassportChanges");
            var count = await collection.CountDocumentsAsync(FilterDefinition<PassportChanges>.Empty);
            Console.WriteLine($"Documents in collection: {count}");     

           var newPassportChange = new PassportChanges
            {
                Series = "1234",
                Number = "567890",
                ChangeType = "Added",
                ChangeDate = DateTime.UtcNow
            };
            try
            {
                await _passportChangesCollection.InsertOneAsync(newPassportChange);
                //await collection.InsertOneAsync(newPassportChange);
            }
            catch (Exception ex) { Console.WriteLine("ddddd errror"); }
            Console.WriteLine("Document added successfully");




            series = "6666";
            number = "666666";
            var result = await _passportChangesCollection
                .Find(c => c.Series == series && c.Number == number)
                .FirstOrDefaultAsync();

            return result;*/

            return await _passportChangesCollection
                 .Find(c => c.Series == series && c.Number == number)
                 .SortByDescending(c => c.ChangeDate)
                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PassportChanges>> GetChangesByDateAsync(DateTime date)
        {
            return await _passportChangesCollection
                .Find(c => c.ChangeDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<PassportChanges>> GetHistoryAsync(string series, string number)
        {
            return await _passportChangesCollection
                .Find(c => c.Series == series && c.Number == number)
                .SortBy(c => c.ChangeDate)
                .ToListAsync();
        }

        public async Task WriteFileToDb(PassportChanges passportChange)
        {            
            await _passportChangesCollection.InsertOneAsync(passportChange);

            //Console.WriteLine("Добавлен новый документ:");
            Console.WriteLine($"Series: {passportChange.Series}, Number: {passportChange.Number}");
        }
       
    }

    
}
