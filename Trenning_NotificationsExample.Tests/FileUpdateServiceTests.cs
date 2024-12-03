namespace Trenning_NotificationsExample.Tests
{
    using NUnit.Framework;
    using Trenning_NotificationsExample.Services;

    [TestFixture]
    public class FileUpdateServiceTests
    {
        [Test]
        public async Task UpdateFileAsync_ShouldDownloadAndUnzipFile()
        {
            string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), "Archived files\\");
            string unZipFilePath = Path.Combine(Directory.GetCurrentDirectory(), "UnZip files\\");

            Directory.CreateDirectory(destinationPath);                                   
            Directory.CreateDirectory(unZipFilePath);                                                       

            var httpClient = new HttpClient();
            var service = new FileUpdateService(httpClient);
            
            var fileName = await service.UpdateFileAsync("https://www.dropbox.com/scl/fi/pvx75x4vqlv6qzjlcu9r5/File-For-test.zip?rlkey=yphtkchtda7jz7gw6nuh2yw6f&st=ogyl5u7f&dl=1", destinationPath);                       

            Assert.That(File.Exists(Path.Combine(destinationPath, fileName)), Is.True, "Downloaded file does not exist.");         
            
            Directory.Delete(destinationPath, true);
            Directory.Delete(unZipFilePath, true);

        }

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


    }
}
