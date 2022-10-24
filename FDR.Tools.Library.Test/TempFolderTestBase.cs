using System;
using System.IO;
using System.Linq;

namespace FDR.Tools.Library.Test
{
    public class TempFolderTestBase : TestBase
    {
        protected string tempFolderPath;
        protected DirectoryInfo tempFolder;
        protected readonly TestFiles files = new();

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolderPath);
            tempFolder = new DirectoryInfo(tempFolderPath);
        }

        public override void OneTimeTearDown()
        {
            if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath, true);
            base.OneTimeTearDown();
        }

        public override void SetUp()
        {
            base.SetUp();
            files.Clear();
        }

        public override void TearDown()
        {
            if (Directory.Exists(tempFolderPath)) Directory.GetFiles(tempFolderPath, "*", SearchOption.AllDirectories).ToList().ForEach(fn => File.Delete(fn));
            base.TearDown();
        }
    }
}
