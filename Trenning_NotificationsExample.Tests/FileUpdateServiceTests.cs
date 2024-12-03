namespace Trenning_NotificationsExample.Tests
{
    using NUnit.Framework;
    using System.IO.Compression;
    using Trenning_NotificationsExample.Services;

    [TestFixture]
    public class FileUpdateServiceTests
    {        
        [Test]
        public async Task UpdateFileAsync_ShouldHandleNonExistentFile()
        {
            // Arrange
            var fakeHttpHandler = new FakeHttpMessageHandler(new Dictionary<string, byte[]>());
            var httpClient = new HttpClient(fakeHttpHandler);
            var service = new FileUpdateService(httpClient);

            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await service.UpdateFileAsync("https://example.com/missing.zip", "Destination/");
            }, "Method threw an exception for a non-existent file.");
        }
    }
}
