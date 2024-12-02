using NUnit.Framework;
using Trenning_NotificationsExample.Services;
using Trenning_NotificationsExample.Tests.Stub_classes;

namespace Trenning_NotificationsExample.Tests
{
    [TestFixture]
    internal class StreamFileComparerTests
    {
        [Test]
        public async Task CompareFilesAsync_ShouldDetectAddedRecords()
        {            
            var fakeService = new FakePassportUpdateService("nothing");
            var comparer = new StreamFileComparer(fakeService);

            string file1Path = "file1.txt";
            string file2Path = "file2.txt";

            await File.WriteAllLinesAsync(file1Path, new[] { "1234,5678" });
            await File.WriteAllLinesAsync(file2Path, new[] { "1234,5678", "9876,5432" });
           
            await comparer.CompareFilesAsync(file1Path, file2Path);
   
            Assert.That(fakeService.Changes.Count, Is.EqualTo(1));
             var addedChange = fakeService.Changes.First();

            Assert.That("9876", Is.EqualTo(addedChange.Series));
            Assert.That("Added",Is.EqualTo(addedChange.ChangeType));                     

            File.Delete(file1Path);
            File.Delete(file2Path);
        }
    }
}
