using System.IO;
using NUnit.Framework;
using FluentAssertions;
using System.Threading;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class CleanupFolderTest : TempFolderTestBase
    {
        private string rawFolderPath;
        private string panoramaFolderPath;
        private CancellationToken token = new();

        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            rawFolderPath = Path.Combine(tempFolderPath, "RAW");
            Directory.CreateDirectory(rawFolderPath);

            panoramaFolderPath = Path.Combine(tempFolderPath, "panorama");
            Directory.CreateDirectory(panoramaFolderPath);
        }

        [Test]
        public void CleanupFolderTests()
        {
            files.Add(tempFolderPath, "keep1m.jpg", true);
            files.Add(tempFolderPath, "keep2m.jpg", true);
            files.Add(tempFolderPath, "keep3m.jpg", true);
            files.Add(tempFolderPath, "keep4m.jpg", true);

            files.Add(tempFolderPath, ".keep1m.jpg.md5", true);
            files.Add(tempFolderPath, ".keep2m.jpg.md5", true);
            files.Add(tempFolderPath, ".keep3m.jpg.md5", true);
            files.Add(tempFolderPath, ".keep4m.jpg.md5", true);
            files.Add(tempFolderPath, ".missing1.jpg.md5", false);
            files.Add(tempFolderPath, ".missing2.jpg.md5", false);
            files.Add(tempFolderPath, ".missing3.jpg.md5", false);
            files.Add(tempFolderPath, ".missing4.jpg.md5", false);

            files.Add(tempFolderPath, "keep1m.jpg.error", true);
            files.Add(tempFolderPath, "keep2m.jpg.error", true);
            files.Add(tempFolderPath, "keep3m.jpg.error", true);
            files.Add(tempFolderPath, "keep4m.jpg.error", true);
            files.Add(tempFolderPath, "missing1.jpg.error", false);
            files.Add(tempFolderPath, "missing2.jpg.error", false);
            files.Add(tempFolderPath, "missing3.jpg.error", false);
            files.Add(tempFolderPath, "missing4.jpg.error", false);

            files.Add(rawFolderPath, "keep1.crw", true);
            files.Add(rawFolderPath, "keep2.cr2", true);
            files.Add(rawFolderPath, "keep3.cr3", true);
            files.Add(rawFolderPath, "keep4.dng", true);
            files.Add(rawFolderPath, "delete1.crw", false);
            files.Add(rawFolderPath, "delete2.cr2", false);
            files.Add(rawFolderPath, "delete3.cr3", false);
            files.Add(rawFolderPath, "delete4.dng", false);

            files.Add(rawFolderPath, ".keep1.crw.md5", true);
            files.Add(rawFolderPath, ".keep2.cr2.md5", true);
            files.Add(rawFolderPath, ".keep3.cr3.md5", true);
            files.Add(rawFolderPath, ".keep4.dng.md5", true);
            files.Add(rawFolderPath, ".delete1.crw.md5", false);
            files.Add(rawFolderPath, ".delete2.cr2.md5", false);
            files.Add(rawFolderPath, ".delete3.cr3.md5", false);
            files.Add(rawFolderPath, ".delete4.dng.md5", false);
            files.Add(rawFolderPath, ".missing1.crw.md5", false);
            files.Add(rawFolderPath, ".missing2.cr2.md5", false);
            files.Add(rawFolderPath, ".missing3.cr3.md5", false);
            files.Add(rawFolderPath, ".missing4.dng.md5", false);

            files.Add(rawFolderPath, "keep1.crw.error", true);
            files.Add(rawFolderPath, "keep2.cr2.error", true);
            files.Add(rawFolderPath, "keep3.cr3.error", true);
            files.Add(rawFolderPath, "keep4.dng.error", true);
            files.Add(rawFolderPath, "delete1.crw.error", false);
            files.Add(rawFolderPath, "delete2.cr2.error", false);
            files.Add(rawFolderPath, "delete3.cr3.error", false);
            files.Add(rawFolderPath, "delete4.dng.error", false);
            files.Add(rawFolderPath, "missing1.crw.error", false);
            files.Add(rawFolderPath, "missing2.cr2.error", false);
            files.Add(rawFolderPath, "missing3.cr3.error", false);
            files.Add(rawFolderPath, "missing4.dng.error", false);

            files.Add(panoramaFolderPath, "panorama1.crw", true);
            files.Add(panoramaFolderPath, "panorama2.cr2", true);
            files.Add(panoramaFolderPath, "panorama3.cr3", true);
            files.Add(panoramaFolderPath, "panorama4.dng", true);
            files.Add(panoramaFolderPath, "panorama1m.jpg", true);
            files.Add(panoramaFolderPath, "panorama2m.jpg", true);

            files.Add(panoramaFolderPath, ".panorama1.crw.md5", true);
            files.Add(panoramaFolderPath, ".panorama2.cr2.md5", true);
            files.Add(panoramaFolderPath, ".panorama3.cr3.md5", true);
            files.Add(panoramaFolderPath, ".panorama4.dng.md5", true);
            files.Add(panoramaFolderPath, ".panorama1m.jpg.md5", true);
            files.Add(panoramaFolderPath, ".panorama2m.jpg.md5", true);
            files.Add(panoramaFolderPath, ".missing1.crw.md5", false);
            files.Add(panoramaFolderPath, ".missing2.cr2.md5", false);
            files.Add(panoramaFolderPath, ".missing3.cr3.md5", false);
            files.Add(panoramaFolderPath, ".missing4.dng.md5", false);
            files.Add(panoramaFolderPath, ".missing1.jpg.md5", false);
            files.Add(panoramaFolderPath, ".missing2.jpg.md5", false);
            files.Add(panoramaFolderPath, ".missing3.jpg.md5", false);
            files.Add(panoramaFolderPath, ".missing4.jpg.md5", false);

            files.Add(panoramaFolderPath, "panorama1.crw.error", true);
            files.Add(panoramaFolderPath, "panorama2.cr2.error", true);
            files.Add(panoramaFolderPath, "panorama3.cr3.error", true);
            files.Add(panoramaFolderPath, "panorama4.dng.error", true);
            files.Add(panoramaFolderPath, "panorama1m.jpg.error", true);
            files.Add(panoramaFolderPath, "panorama2m.jpg.error", true);
            files.Add(panoramaFolderPath, "missing1.crw.error", false);
            files.Add(panoramaFolderPath, "missing2.cr2.error", false);
            files.Add(panoramaFolderPath, "missing3.cr3.error", false);
            files.Add(panoramaFolderPath, "missing4.dng.error", false);
            files.Add(panoramaFolderPath, "missing1.jpg.error", false);
            files.Add(panoramaFolderPath, "missing2.jpg.error", false);
            files.Add(panoramaFolderPath, "missing3.jpg.error", false);
            files.Add(panoramaFolderPath, "missing4.jpg.error", false);

            files.CreateFiles();

            Raw.CleanupFolder(tempFolder, token);

            files.ValidateExistance();
        }

        [Test]
        public void CleanupUnconvertedTests()
        {
            files.Add(rawFolderPath, "keep1.crw", true);
            files.Add(rawFolderPath, "keep2.cr2", true);
            files.Add(rawFolderPath, "keep3.cr3", true);
            files.Add(rawFolderPath, "keep4.dng", true);

            files.Add(rawFolderPath, ".keep1.crw.md5", true);
            files.Add(rawFolderPath, ".keep2.cr2.md5", true);
            files.Add(rawFolderPath, ".keep3.cr3.md5", true);
            files.Add(rawFolderPath, ".keep4.dng.md5", true);
            files.Add(rawFolderPath, ".missing1.crw.md5", false);
            files.Add(rawFolderPath, ".missing2.cr2.md5", false);
            files.Add(rawFolderPath, ".missing3.cr3.md5", false);
            files.Add(rawFolderPath, ".missing4.dng.md5", false);

            files.Add(rawFolderPath, "keep1.crw.error", true);
            files.Add(rawFolderPath, "keep2.cr2.error", true);
            files.Add(rawFolderPath, "keep3.cr3.error", true);
            files.Add(rawFolderPath, "keep4.dng.error", true);
            files.Add(rawFolderPath, "missing1.crw.error", false);
            files.Add(rawFolderPath, "missing2.cr2.error", false);
            files.Add(rawFolderPath, "missing3.cr3.error", false);
            files.Add(rawFolderPath, "missing4.dng.error", false);

            files.Add(panoramaFolderPath, "panorama1.crw", true);
            files.Add(panoramaFolderPath, "panorama2.cr2", true);
            files.Add(panoramaFolderPath, "panorama3.cr3", true);
            files.Add(panoramaFolderPath, "panorama4.dng", true);
            files.Add(panoramaFolderPath, "panorama1m.jpg", true);
            files.Add(panoramaFolderPath, "panorama2m.jpg", true);

            files.Add(panoramaFolderPath, ".panorama1.crw.md5", true);
            files.Add(panoramaFolderPath, ".panorama2.cr2.md5", true);
            files.Add(panoramaFolderPath, ".panorama3.cr3.md5", true);
            files.Add(panoramaFolderPath, ".panorama4.dng.md5", true);
            files.Add(panoramaFolderPath, ".panorama1m.jpg.md5", true);
            files.Add(panoramaFolderPath, ".panorama2m.jpg.md5", true);
            files.Add(panoramaFolderPath, ".missing1.crw.md5", false);
            files.Add(panoramaFolderPath, ".missing2.cr2.md5", false);
            files.Add(panoramaFolderPath, ".missing3.cr3.md5", false);
            files.Add(panoramaFolderPath, ".missing4.dng.md5", false);
            files.Add(panoramaFolderPath, ".missing1.jpg.md5", false);
            files.Add(panoramaFolderPath, ".missing2.jpg.md5", false);
            files.Add(panoramaFolderPath, ".missing3.jpg.md5", false);
            files.Add(panoramaFolderPath, ".missing4.jpg.md5", false);

            files.Add(panoramaFolderPath, "panorama1.crw.error", true);
            files.Add(panoramaFolderPath, "panorama2.cr2.error", true);
            files.Add(panoramaFolderPath, "panorama3.cr3.error", true);
            files.Add(panoramaFolderPath, "panorama4.dng.error", true);
            files.Add(panoramaFolderPath, "panorama1m.jpg.error", true);
            files.Add(panoramaFolderPath, "panorama2m.jpg.error", true);
            files.Add(panoramaFolderPath, "missing1.crw.error", false);
            files.Add(panoramaFolderPath, "missing2.cr2.error", false);
            files.Add(panoramaFolderPath, "missing3.cr3.error", false);
            files.Add(panoramaFolderPath, "missing4.cdngr3.error", false);
            files.Add(panoramaFolderPath, "missing1.jpg.error", false);
            files.Add(panoramaFolderPath, "missing2.jpg.error", false);
            files.Add(panoramaFolderPath, "missing3.jpg.error", false);
            files.Add(panoramaFolderPath, "missing4.jpg.error", false);

            files.CreateFiles();

            Raw.CleanupFolder(tempFolder, token);

            files.ValidateExistance();
        }
    }
}