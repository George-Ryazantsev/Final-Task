namespace Trenning_NotificationsExample.Tests
{
    using NUnit.Framework;
    using NUnit.Framework.Internal;
    using Trenning_NotificationsExample.Services;

    [TestFixture]
    public class FileUpdateServiceTests
    {
        [Test]
        public async Task UpdateFileAsync_ShouldHandleNonExistentFile()
        {
            var fakeHttpHandler = new FakeHttpMessageHandler(new Dictionary<string, byte[]>());
            var httpClient = new HttpClient(fakeHttpHandler);
            var service = new FileUpdateService(httpClient);

            Assert.DoesNotThrowAsync(async () =>
            {
                await service.UpdateFileAsync("https://example.com/missing.zip", "Destination/");
            }, "Method threw an exception for a non-existent file.");
        }

       /* [Test]
        public async Task UpdateFileAsync_ShouldHandleDownloadFile()
        {
            string fileUrl = "https://www.dropbox.com/scl/fi/pvx75x4vqlv6qzjlcu9r5/File-For-test.zip?rlkey=yphtkchtda7jz7gw6nuh2yw6f&st=6c72ojex&dl=1";

            string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), "TestDestination\\");
            string unZipFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UnZip files\\");          
            string fakeDownloadedFileName = "File For test.zip";      

            Directory.CreateDirectory(destinationPath);
            Directory.CreateDirectory(unZipFolderPath);
         

            var httpClient = new HttpClient();
            var service = new FileUpdateService(httpClient);
                     
            string resultFileName = await service.UpdateFileAsync(fileUrl, destinationPath);

                        
            string expectedDownloadedFilePath = Path.Combine(destinationPath, fakeDownloadedFileName);
            Assert.That(File.Exists(expectedDownloadedFilePath), Is.True, "Downloaded ZIP file does not exist.");

            Directory.Delete(destinationPath, true);
            Directory.Delete(unZipFolderPath, true);
        }*/
      
    }
}







