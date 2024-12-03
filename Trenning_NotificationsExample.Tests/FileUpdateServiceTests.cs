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
      
    }
}







