using System.IO.Compression;

namespace Trenning_NotificationsExample.Services
{
    public class FileUpdateService
    {
        private string unZipFilePath = "UnZip files\\";                
        private readonly HttpClient _httpClient;
        public FileUpdateService(HttpClient httpClient)
        {
            _httpClient = httpClient;            
        }
        public async Task<string> UpdateFileAsync(string fileUrl, string destinationPath)
        {
            try
            {
                string fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
                string fullDestinationPath = destinationPath + fileName;

                string newFileName = "Outdated Data.csv";
                string newFileNamePath = Path.Combine(unZipFilePath, newFileName);

                if (Directory.GetFiles(destinationPath).Length > 0)
                {
                    var lastModified = File.GetLastWriteTime(fullDestinationPath);

                    if (lastModified.Date != DateTime.Today)
                    {
                        File.Delete(fileName);
                        await DownloadFileAsync(fileUrl, destinationPath);

                        Console.WriteLine($"Скачан обновленный zip архив {fileName}");
                    }
                }
                else
                {
                    await DownloadFileAsync(fileUrl, destinationPath);
                    await UnZipFileAsync(fullDestinationPath);

                    return fileName;
                }
                if (Directory.GetFiles(unZipFilePath).Length > 1)
                {
                    File.Delete(newFileNamePath);
                    File.Move(GetFileName(), newFileNamePath);

                    return await UnZipFileAsync(fullDestinationPath);
                }
                else
                {
                    File.Move(GetFileName(), newFileNamePath);
                    Console.WriteLine(" переименован 1 файл ");

                    return await UnZipFileAsync(fullDestinationPath);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении файла: {ex.Message}");
                return string.Empty; 
            }
        }
      
        private async Task DownloadFileAsync(string fileUrl, string destinationPath)
        {
            string fullDestinationPath = destinationPath;

            try
            {
                Console.WriteLine("Downloading...");

                using var response = await _httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                string fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
                fullDestinationPath = destinationPath + fileName;

                await using var fileStream = new FileStream(fullDestinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await using var contentStream = await response.Content.ReadAsStreamAsync();

                await contentStream.CopyToAsync(fileStream);

                Console.WriteLine($"Файл успешно скачан в: {destinationPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при скачивании файла: {ex.Message}");
            }
        }

        private async Task<string> UnZipFileAsync(string fullDestinationPath)
        {           

            try
            {
                Directory.CreateDirectory(unZipFilePath);

                using (ZipArchive archive = ZipFile.OpenRead(fullDestinationPath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string destinationPath = Path.Combine(unZipFilePath, entry.FullName);

                        entry.ExtractToFile(destinationPath, overwrite: true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            return Directory.GetFiles(unZipFilePath)[1];
        }
        private string GetFileName()
        {           
            return Directory.GetFiles(unZipFilePath)[0];
        }
    }
}
