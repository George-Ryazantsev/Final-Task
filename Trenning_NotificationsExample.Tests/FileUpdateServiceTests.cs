namespace Trenning_NotificationsExample.Tests
{
    using NUnit.Framework;
    using System.IO.Compression;
    using Trenning_NotificationsExample.Services;

    [TestFixture]
    public class FileUpdateServiceTests
    {
        [Test]
        public async Task UpdateFileAsync_ShouldDownloadAndUnzipFile()
        {
            // Arrange
            string testZipContentPath = "test.zip";
            string extractedFileName = "test.txt";
            string extractedContent = "Hello, NUnit!";
            string destinationPath = "Destination/";

            Directory.CreateDirectory(destinationPath);

            // Создаем тестовый ZIP-файл
            using (var zipStream = new FileStream(testZipContentPath, FileMode.Create))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {
                var entry = archive.CreateEntry(extractedFileName);
                using var entryStream = entry.Open();
                using var writer = new StreamWriter(entryStream);
                writer.Write(extractedContent);
            }

            var zipContent = await File.ReadAllBytesAsync(testZipContentPath);
            var fakeHttpHandler = new FakeHttpMessageHandler(new Dictionary<string, byte[]>
        {
            { "https://example.com/test.zip", zipContent }
        });

            var httpClient = new HttpClient(fakeHttpHandler);
            var service = new FileUpdateService(httpClient);

            // Act
            var fileName = await service.UpdateFileAsync("https://example.com/test.zip", destinationPath);

            // Assert
            string unzippedFilePath = Path.Combine("UnZip files", extractedFileName);
            Assert.That(File.Exists(Path.Combine(destinationPath, fileName)), Is.True, "Downloaded file does not exist.");
            Assert.That(File.Exists(unzippedFilePath), Is.True, "Unzipped file does not exist.");
            Assert.That(await File.ReadAllTextAsync(unzippedFilePath), Is.EqualTo(extractedContent), "Unzipped file content does not match.");

            // Cleanup
            Directory.Delete(destinationPath, true);
            Directory.Delete("UnZip files", true);
            File.Delete(testZipContentPath);
        }

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
