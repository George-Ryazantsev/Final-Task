namespace Trenning_NotificationsExample.Services
{
    public class PassportUpdateHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        PassportChangesService _passportChangesService;
        FileUpdateService _fileUpdateService;
        public PassportUpdateHostedService(PassportChangesService passportChangesService, FileUpdateService fileUpdateService )
        {
            _passportChangesService = passportChangesService;
            _fileUpdateService = fileUpdateService;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdatePassports, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }        
        private async void UpdatePassports(object state)
        {
            string fileDownloadDir = "ArchivedFiles\\";
            string unZipFilePath = "UnZipFiles\\";
            Directory.CreateDirectory(fileDownloadDir);

              string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), fileDownloadDir);
              string fileUrl = " https://www.dropbox.com/scl/fi/el4itqnexz09jov2fjv06/DataZ.zip?rlkey=n6ivk5epo1e9ym8s7tzzgtatp&st=wzeiv953&dl=1";

              string fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);                      

          /*   destinationPath = await _fileUpdateService.UpdateFileAsync(fileUrl, destinationPath);
              var lastModified = File.GetLastAccessTime(destinationPath+ fileName);

            if (destinationPath.Contains(fileName))
            {
                Console.WriteLine("Архив успешно скачан и распакован");
            }
            else if (lastModified.Date != DateTime.Today)
            {
                StreamFileComparer comparer = new StreamFileComparer(_passportChangesService);
                string newUnzipedFileName = Path.Combine(destinationPath.Split('\\')[0], fileName.Split('.')[0] + ".csv");

                Console.WriteLine("Обрабатываем 2 новых файла...");
                await comparer.CompareFilesAsync(destinationPath, newUnzipedFileName);
            }
           else {
                
                Console.WriteLine("Тестовый запуск");
                StreamFileComparer comparer = new StreamFileComparer(_passportChangesService);
                string newUnzipedFileName = Path.Combine(destinationPath.Split('\\')[0], fileName.Split('.')[0] + ".csv");

                Console.WriteLine("Обрабатываем 2 новых файла...");
                await comparer.CompareFilesAsync(destinationPath, newUnzipedFileName); 

            }*/

           /* StreamFileComparer comparer = new StreamFileComparer(_passportChangesService);
            destinationPath = Path.Combine(Directory.GetCurrentDirectory(), unZipFilePath);

            string newUnzipedFileName = destinationPath + "DataZ.csv";
            destinationPath += "Outdated Data.csv";            

            Console.WriteLine("Обрабатываем 2 новых файла...");
            await comparer.CompareFilesAsync(destinationPath, newUnzipedFileName);*/
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
