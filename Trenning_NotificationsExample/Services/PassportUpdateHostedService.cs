namespace Trenning_NotificationsExample.Services
{
    public class PassportUpdateHostedService : IHostedService, IDisposable
    {
        private Timer _timer;        
        PassportUpdateService _passportUpdateService;
        FileUpdateService _fileUpdateService;
        public PassportUpdateHostedService(PassportUpdateService passportUpdateService, FileUpdateService fileUpdateService )
        {            
            _passportUpdateService = passportUpdateService;
            _fileUpdateService = fileUpdateService;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdatePassports, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void UpdatePassports(object state)
        {
            string fileDownloadDir = "Archived files\\";
            Directory.CreateDirectory(fileDownloadDir);

            string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), fileDownloadDir);
            string fileUrl = " https://www.dropbox.com/scl/fi/el4itqnexz09jov2fjv06/DataZ.zip?rlkey=n6ivk5epo1e9ym8s7tzzgtatp&st=wzeiv953&dl=1";

            string fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);

            _passportUpdateService.FilePath = "Unzip files\\PassportChanges.json";            

            destinationPath = await _fileUpdateService.UpdateFileAsync(fileUrl, destinationPath);
            var lastModified = File.GetLastAccessTime(destinationPath);

            if (destinationPath.Contains(fileName))
            {
                Console.WriteLine("Архив успешно скачан и распакован");
            }
            else //if (lastModified.Date != DateTime.Today)
            {
                StreamFileComparer comparer = new StreamFileComparer(_passportUpdateService);
                string newUnzipedFileName = Path.Combine(destinationPath.Split('\\')[0], fileName.Split('.')[0] + ".csv");

                Console.WriteLine("Обрабатываем 2 новых файла...");
                await comparer.CompareFilesAsync(destinationPath, newUnzipedFileName);
            }
           // else Console.WriteLine("Повторный запуск");

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
