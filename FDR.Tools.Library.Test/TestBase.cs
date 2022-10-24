using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class TestBase
    {
        private ConsoleTraceListener consoleTracer;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            consoleTracer = new ConsoleTraceListener();
            Trace.Listeners.Add(consoleTracer);
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            Trace.Flush();
            Trace.Listeners.Remove(consoleTracer);
            consoleTracer?.Close();
            consoleTracer?.Dispose();
        }

        [SetUp]
        public virtual void SetUp()
        {
        }

        [TearDown]
        public virtual void TearDown()
        {
        }
    }

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
