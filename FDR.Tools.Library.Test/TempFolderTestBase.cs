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
            tempFolder = new DirectoryInfo(tempFolderPath);
            tempFolder.Create();
        }

        public override void OneTimeTearDown()
        {
            if (tempFolder.Exists) tempFolder.Delete(true);
            base.OneTimeTearDown();
        }

        public override void SetUp()
        {
            base.SetUp();
            files.Clear();
        }

        public override void TearDown()
        {
            if (tempFolder.Exists) tempFolder.GetFiles("*", SearchOption.AllDirectories).ToList().ForEach(f => f.Delete());
            base.TearDown();
        }
    }
}
