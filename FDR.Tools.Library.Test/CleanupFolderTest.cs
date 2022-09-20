using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    public class CleanupFolderTest : TestFixtureBase
    {
        private string tempFolderPath;
        private DirectoryInfo tempFolder;
        private string rawFolderPath;
        private string panoramaFolderPath;
        private readonly TestFiles files = new();

        private class TestFile
        {
            public TestFile(string folder, string name, bool keep)
            {
                Folder = folder;
                Name = name;
                Keep = keep;
            }
            public string Folder;
            public string Name;
            public bool Keep;
            public string Path => System.IO.Path.Combine(Folder, Name);
        }

        private class TestFiles : List<TestFile>
        {
            public void Add(string folder, string name, bool keep)
            {
                base.Add(new TestFile(folder, name, keep));
            }
        }

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolderPath);
            tempFolder = new DirectoryInfo(tempFolderPath);

            rawFolderPath = Path.Combine(tempFolderPath, "RAW");
            Directory.CreateDirectory(rawFolderPath);

            panoramaFolderPath = Path.Combine(tempFolderPath, "panorama");
            Directory.CreateDirectory(panoramaFolderPath);
        }

        public override void OneTimeTearDown()
        {
            if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath, true);

            base.OneTimeTearDown();
        }

        public override void SetUp()
        {
            base.SetUp();

            Directory.GetFiles(tempFolderPath, "*", SearchOption.AllDirectories).ToList().ForEach(f => File.Delete(f));
            files.Clear();
        }

        public override void TearDown()
        {
            Directory.GetFiles(tempFolderPath, "*", SearchOption.AllDirectories).ToList().ForEach(f => File.Delete(f));
            files.Clear();

            base.TearDown();
        }

        [Test]
        public void CleanupFolderTests()
        {
            files.Add(tempFolderPath, "keep1m.jpg", true);
            files.Add(tempFolderPath, "keep2m.jpg", true);
            files.Add(tempFolderPath, "keep3m.jpg", true);

            files.Add(tempFolderPath, ".keep1m.jpg.md5", true);
            files.Add(tempFolderPath, ".keep2m.jpg.md5", true);
            files.Add(tempFolderPath, ".keep3m.jpg.md5", true);
            files.Add(tempFolderPath, ".missing1.jpg.md5", false);
            files.Add(tempFolderPath, ".missing2.jpg.md5", false);
            files.Add(tempFolderPath, ".missing3.jpg.md5", false);

            files.Add(tempFolderPath, "keep1m.jpg.error", true);
            files.Add(tempFolderPath, "keep2m.jpg.error", true);
            files.Add(tempFolderPath, "keep3m.jpg.error", true);
            files.Add(tempFolderPath, "missing1.jpg.error", false);
            files.Add(tempFolderPath, "missing2.jpg.error", false);
            files.Add(tempFolderPath, "missing3.jpg.error", false);

            files.Add(rawFolderPath, "keep1.crw", true);
            files.Add(rawFolderPath, "keep2.cr2", true);
            files.Add(rawFolderPath, "keep3.cr3", true);
            files.Add(rawFolderPath, "delete1.crw", false);
            files.Add(rawFolderPath, "delete2.cr2", false);
            files.Add(rawFolderPath, "delete3.cr3", false);

            files.Add(rawFolderPath, ".keep1.crw.md5", true);
            files.Add(rawFolderPath, ".keep2.cr2.md5", true);
            files.Add(rawFolderPath, ".keep3.cr3.md5", true);
            files.Add(rawFolderPath, ".delete1.crw.md5", false);
            files.Add(rawFolderPath, ".delete2.cr2.md5", false);
            files.Add(rawFolderPath, ".delete3.cr3.md5", false);
            files.Add(rawFolderPath, ".missing1.crw.md5", false);
            files.Add(rawFolderPath, ".missing2.cr2.md5", false);
            files.Add(rawFolderPath, ".missing3.cr3.md5", false);

            files.Add(rawFolderPath, "keep1.crw.error", true);
            files.Add(rawFolderPath, "keep2.cr2.error", true);
            files.Add(rawFolderPath, "keep3.cr3.error", true);
            files.Add(rawFolderPath, "delete1.crw.error", false);
            files.Add(rawFolderPath, "delete2.cr2.error", false);
            files.Add(rawFolderPath, "delete3.cr3.error", false);
            files.Add(rawFolderPath, "missing1.crw.error", false);
            files.Add(rawFolderPath, "missing2.cr2.error", false);
            files.Add(rawFolderPath, "missing3.cr3.error", false);

            files.Add(panoramaFolderPath, "panorama1.crw", true);
            files.Add(panoramaFolderPath, "panorama2.cr2", true);
            files.Add(panoramaFolderPath, "panorama3.cr3", true);
            files.Add(panoramaFolderPath, "panorama1m.jpg", true);
            files.Add(panoramaFolderPath, "panorama2m.jpg", true);

            files.Add(panoramaFolderPath, ".panorama1.crw.md5", true);
            files.Add(panoramaFolderPath, ".panorama2.cr2.md5", true);
            files.Add(panoramaFolderPath, ".panorama3.cr3.md5", true);
            files.Add(panoramaFolderPath, ".panorama1m.jpg.md5", true);
            files.Add(panoramaFolderPath, ".panorama2m.jpg.md5", true);
            files.Add(panoramaFolderPath, ".missing1.crw.md5", false);
            files.Add(panoramaFolderPath, ".missing2.cr2.md5", false);
            files.Add(panoramaFolderPath, ".missing3.cr3.md5", false);
            files.Add(panoramaFolderPath, ".missing1.jpg.md5", false);
            files.Add(panoramaFolderPath, ".missing2.jpg.md5", false);
            files.Add(panoramaFolderPath, ".missing3.jpg.md5", false);

            files.Add(panoramaFolderPath, "panorama1.crw.error", true);
            files.Add(panoramaFolderPath, "panorama2.cr2.error", true);
            files.Add(panoramaFolderPath, "panorama3.cr3.error", true);
            files.Add(panoramaFolderPath, "panorama1m.jpg.error", true);
            files.Add(panoramaFolderPath, "panorama2m.jpg.error", true);
            files.Add(panoramaFolderPath, "missing1.crw.error", false);
            files.Add(panoramaFolderPath, "missing2.cr2.error", false);
            files.Add(panoramaFolderPath, "missing3.cr3.error", false);
            files.Add(panoramaFolderPath, "missing1.jpg.error", false);
            files.Add(panoramaFolderPath, "missing2.jpg.error", false);
            files.Add(panoramaFolderPath, "missing3.jpg.error", false);

            files.ForEach(f => File.WriteAllText(f.Path, ""));

            Raw.CleanupFolder(tempFolder);

            files.ForEach(f => File.Exists(f.Path).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void CleanupUnconvertedTests()
        {
            files.Add(rawFolderPath, "keep1.crw", true);
            files.Add(rawFolderPath, "keep2.cr2", true);
            files.Add(rawFolderPath, "keep3.cr3", true);

            files.Add(rawFolderPath, ".keep1.crw.md5", true);
            files.Add(rawFolderPath, ".keep2.cr2.md5", true);
            files.Add(rawFolderPath, ".keep3.cr3.md5", true);
            files.Add(rawFolderPath, ".missing1.crw.md5", false);
            files.Add(rawFolderPath, ".missing2.cr2.md5", false);
            files.Add(rawFolderPath, ".missing3.cr3.md5", false);

            files.Add(rawFolderPath, "keep1.crw.error", true);
            files.Add(rawFolderPath, "keep2.cr2.error", true);
            files.Add(rawFolderPath, "keep3.cr3.error", true);
            files.Add(rawFolderPath, "missing1.crw.error", false);
            files.Add(rawFolderPath, "missing2.cr2.error", false);
            files.Add(rawFolderPath, "missing3.cr3.error", false);

            files.Add(panoramaFolderPath, "panorama1.crw", true);
            files.Add(panoramaFolderPath, "panorama2.cr2", true);
            files.Add(panoramaFolderPath, "panorama3.cr3", true);
            files.Add(panoramaFolderPath, "panorama1m.jpg", true);
            files.Add(panoramaFolderPath, "panorama2m.jpg", true);

            files.Add(panoramaFolderPath, ".panorama1.crw.md5", true);
            files.Add(panoramaFolderPath, ".panorama2.cr2.md5", true);
            files.Add(panoramaFolderPath, ".panorama3.cr3.md5", true);
            files.Add(panoramaFolderPath, ".panorama1m.jpg.md5", true);
            files.Add(panoramaFolderPath, ".panorama2m.jpg.md5", true);
            files.Add(panoramaFolderPath, ".missing1.crw.md5", false);
            files.Add(panoramaFolderPath, ".missing2.cr2.md5", false);
            files.Add(panoramaFolderPath, ".missing3.cr3.md5", false);
            files.Add(panoramaFolderPath, ".missing1.jpg.md5", false);
            files.Add(panoramaFolderPath, ".missing2.jpg.md5", false);
            files.Add(panoramaFolderPath, ".missing3.jpg.md5", false);

            files.Add(panoramaFolderPath, "panorama1.crw.error", true);
            files.Add(panoramaFolderPath, "panorama2.cr2.error", true);
            files.Add(panoramaFolderPath, "panorama3.cr3.error", true);
            files.Add(panoramaFolderPath, "panorama1m.jpg.error", true);
            files.Add(panoramaFolderPath, "panorama2m.jpg.error", true);
            files.Add(panoramaFolderPath, "missing1.crw.error", false);
            files.Add(panoramaFolderPath, "missing2.cr2.error", false);
            files.Add(panoramaFolderPath, "missing3.cr3.error", false);
            files.Add(panoramaFolderPath, "missing1.jpg.error", false);
            files.Add(panoramaFolderPath, "missing2.jpg.error", false);
            files.Add(panoramaFolderPath, "missing3.jpg.error", false);

            files.ForEach(f => File.WriteAllText(f.Path, ""));

            Raw.CleanupFolder(tempFolder);

            files.ForEach(f => File.Exists(f.Path).Should().Be(f.Keep, f.Name));
        }
    }
}