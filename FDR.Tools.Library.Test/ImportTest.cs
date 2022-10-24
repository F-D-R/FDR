using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;
using Newtonsoft.Json;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class ImportTest : TempFolderTestBase
    {
        private string dcim0;
        private string dcim1;
        private string dcim2;
        private string dcim3;
        private string dcim4;
        private string source1;
        private string source2;
        private string destinationRoot;
        private string dest1;
        private string dest2;
        private string raw1;
        private string raw2;

        private const int importConfigCount = 4;
        private const string appConfigJson = @"
{
  ""RenameConfigs"": {
    ""yymmdd_ccc"": { ""FileFilter"": ""*.CR3|*.CR2|*.CRW"", ""FileNamePattern"": ""{cdate:yyMMdd}_{counter:3}"", ""AdditionalFileTypes"": [ "".JPG"" ], ""Recursive"": false, ""StopOnError"": true } },
  ""MoveConfigs"": {
    ""raw"": { ""FileFilter"": ""*.CR3|*.CR2|*.CRW"", ""RelativeFolder"": ""RAW"" } },
  ""ImportConfigs"": {
    ""import1"": {
      ""Name"": ""import1"",
      ""DestStructure"": ""date"",
      ""DateFormat"": ""yyyyMMdd"",
      ""Rules"": [ { ""Type"": ""contains_folder"", ""Param"": ""???CANON"" }, { ""Type"": ""contains_folder"", ""Param"": ""CANONMSC"" } ],
      ""Actions"": [ { ""Type"": ""rename"", ""Config"": ""yymmdd_ccc"" }, { ""Type"": ""move"", ""Config"": ""raw"" } ]
    },
    ""import2"": {
      ""Name"": ""import2"",
      ""DestStructure"": ""date"",
      ""DateFormat"": ""yyyyMMdd"",
      ""Rules"": [ { ""Type"": ""contains_folder"", ""Param"": ""pictures"" } ],
      ""Actions"": [ { ""Type"": ""rename"", ""Config"": ""yymmdd_ccc"" }, { ""Type"": ""move"", ""Config"": ""raw"" } ]
    },
    ""import3"": {
      ""Name"": ""import3"",
      ""Rules"": [ { ""Type"": ""contains_file"", ""Param"": ""dummy"" } ]
    },
    ""import4"": {
      ""Name"": ""import4"",
      ""Rules"": [ { ""Type"": ""volume_label"", ""Param"": ""dummy"" } ]
    }
  }
}
";


        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            dcim0 = Path.Combine(tempFolderPath, "drive0", "DCIM");
            Directory.CreateDirectory(dcim0);
            dcim1 = Path.Combine(tempFolderPath, "drive1", "DCIM");
            Directory.CreateDirectory(dcim1);
            dcim2 = Path.Combine(tempFolderPath, "drive2", "DCIM");
            Directory.CreateDirectory(dcim2);
            dcim3 = Path.Combine(tempFolderPath, "drive3", "DCIM");
            Directory.CreateDirectory(dcim3);
            dcim4 = Path.Combine(tempFolderPath, "drive4", "DCIM");
            Directory.CreateDirectory(dcim4);

            source1 = Path.Combine(dcim1, "100CANON");
            Directory.CreateDirectory(source1);
            Directory.CreateDirectory(Path.Combine(dcim1, "CANONMSC"));
            source2 = Path.Combine(dcim2, "pictures");
            Directory.CreateDirectory(source2);

            destinationRoot = Path.Combine(tempFolderPath, "ROOT");
            Directory.CreateDirectory(destinationRoot);

            dest1 = Path.Combine(destinationRoot, "20220101");
            Directory.CreateDirectory(dest1);
            dest2 = Path.Combine(destinationRoot, "20220202");
            Directory.CreateDirectory(dest2);

            raw1 = Path.Combine(dest1, "RAW");
            Directory.CreateDirectory(raw1);
            raw2 = Path.Combine(dest2, "RAW");
            Directory.CreateDirectory(raw2);
        }

        [Test]
        public void FindConfigTests()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);

            var import0 = Import.FindConfig(new DirectoryInfo(dcim0), appConfig.ImportConfigs);
            import0.Should().BeNull("import0");

            // contains_folder (???CANON, CANONMSC)
            var import1 = Import.FindConfig(new DirectoryInfo(dcim1), appConfig.ImportConfigs);
            import1.Should().NotBeNull("import1");
            import1.Name.Should().Be("import1");

            // contains_folder (pictures)
            var import2 = Import.FindConfig(new DirectoryInfo(dcim2), appConfig.ImportConfigs);
            import2.Should().NotBeNull("import2");
            import2.Name.Should().Be("import2");

            // contains_file (dummy)
            File.WriteAllText(Path.Combine(dcim3, "dummy"), "");
            var import3 = Import.FindConfig(new DirectoryInfo(dcim3), appConfig.ImportConfigs);
            import3.Should().NotBeNull("import3");
            import3.Name.Should().Be("import3");

            // volume_label
            var volume = Import.GetVolumeLabel(dcim4);
            appConfig.ImportConfigs["import4"].Rules[0].Param = volume;
            var import4 = Import.FindConfig(new DirectoryInfo(dcim4), appConfig.ImportConfigs);
            import4.Should().NotBeNull("import4");
            import4.Name.Should().Be("import4");
        }

        [TestCase(FolderStructure.date, "20211231", false)]
        [TestCase(FolderStructure.year_date, "2021\\20211231", false)]
        [TestCase(FolderStructure.year_month_date, "2021\\12\\20211231", false)]
        [TestCase(FolderStructure.year_month_day, "2021\\12\\31", false)]
        [TestCase(FolderStructure.year_month, "2021\\12", false)]
        [TestCase((FolderStructure)999, "", true)]
        public void GetRelativeDestFolderTests(FolderStructure destStruct, string result, bool exception)
        {
            var date = new DateTime(2021, 12, 31);
            if (exception)
            {
                Func<string> func = () => Import.GetRelativeDestFolder(destStruct, date, "yyyyMMdd");
                func.Should().Throw<NotImplementedException>();
            }
            else
                Import.GetRelativeDestFolder(destStruct, date, "yyyyMMdd").Should().Be(result);
        }

        [TestCase(FolderStructure.date, "C:\\dummy\\20211231", false)]
        [TestCase(FolderStructure.year_date, "C:\\dummy\\2021\\20211231", false)]
        [TestCase(FolderStructure.year_month_date, "C:\\dummy\\2021\\12\\20211231", false)]
        [TestCase(FolderStructure.year_month_day, "C:\\dummy\\2021\\12\\31", false)]
        [TestCase(FolderStructure.year_month, "C:\\dummy\\2021\\12", false)]
        [TestCase((FolderStructure)999, "", true)]
        public void GetAbsoluteDestFolderTests(FolderStructure destStruct, string result, bool exception)
        {
            var date = new DateTime(2021, 12, 31);
            if (exception)
            {
                Func<string> func = () => Import.GetAbsoluteDestFolder("C:\\dummy\\", destStruct, date, "yyyyMMdd");
                func.Should().Throw<NotImplementedException>();
            }
            else
                Import.GetAbsoluteDestFolder("C:\\dummy\\", destStruct, date, "yyyyMMdd").Should().Be(result);
        }

        [Test]
        public void CopyFileTests()
        {
            files.Add(new DateTime(2022, 01, 01), source1, "01.crw", dest1, "01.crw");
            files.Add(new DateTime(2022, 01, 01), source1, "02.cr2", dest1, "02.cr2");
            files.Add(new DateTime(2022, 01, 01), source1, "03.cr3", dest1, "03.cr3");
            files.Add(new DateTime(2022, 01, 01), source1, "04.jpg", dest1, "04.jpg");
            files.CreateFiles();

            var count = files.Where(f => f.SourceFolder != f.DestFolder).Count();
            int i = 0;
            foreach (var f in files)
            {
                i++;
                Import.CopyFile(destinationRoot, new FileInfo(f.GetSourcePath()), FolderStructure.date, "yyyyMMdd", 100 * i / count);
            }

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void MoveFilesInFolderTests()
        {
            files.Add(dest1, "01.crw", raw1, "01.crw");
            files.Add(dest1, "02.cr2", raw1, "02.cr2");
            files.Add(dest1, "03.cr3", raw1, "03.cr3");
            files.Add(dest1, "04.jpg", dest1, "04.jpg");
            files.CreateFiles();

            var config = new MoveConfig();
            config.FileFilter = "*.CR3|*.CR2|*.CRW";
            config.RelativeFolder = "RAW";

            Import.MoveFilesInFolder(new DirectoryInfo(dest1), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void ImportTests()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);
            appConfig.ImportConfigs.ToList().ForEach(ic => ic.Value.DestRoot = destinationRoot);
            appConfig.Validate();

            files.Add(new DateTime(2022, 1, 1, 13, 59, 3), source1, "aaa.cr3", raw1, "220101_003.cr3");
            files.Add(new DateTime(2022, 1, 1, 13, 59, 3), source1, "aaa.jpg", dest1, "220101_003.jpg");
            files.Add(new DateTime(2022, 1, 1, 13, 59, 2), source1, "bbb.cr2", raw1, "220101_002.cr2");
            files.Add(new DateTime(2022, 1, 1, 13, 59, 2), source1, "bbb.jpg", dest1, "220101_002.jpg");
            files.Add(new DateTime(2022, 1, 1, 13, 59, 1), source1, "ccc.crw", raw1, "220101_001.crw");
            files.Add(new DateTime(2022, 1, 1, 13, 59, 1), source1, "ccc.jpg", dest1, "220101_001.jpg");
            files.CreateFiles();

            Directory.Delete(dest1, true);
            var import1 = appConfig.ImportConfigs["import1"];
            Import.ImportFiles(new DirectoryInfo(source1), import1);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));


            files.Add(new DateTime(2022, 2, 2, 13, 59, 3), source2, "aaa.cr3", raw2, "220202_003.cr3");
            files.Add(new DateTime(2022, 2, 2, 13, 59, 3), source2, "aaa.jpg", dest2, "220202_003.jpg");
            files.Add(new DateTime(2022, 2, 2, 13, 59, 2), source2, "bbb.cr2", raw2, "220202_002.cr2");
            files.Add(new DateTime(2022, 2, 2, 13, 59, 2), source2, "bbb.jpg", dest2, "220202_002.jpg");
            files.Add(new DateTime(2022, 2, 2, 13, 59, 1), source2, "ccc.crw", raw2, "220202_001.crw");
            files.Add(new DateTime(2022, 2, 2, 13, 59, 1), source2, "ccc.jpg", dest2, "220202_001.jpg");
            files.CreateFiles();

            Directory.Delete(dest2, true);
            var import2 = appConfig.ImportConfigs["import2"];
            Import.ImportFiles(new DirectoryInfo(source2), import2);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }
    }
}
