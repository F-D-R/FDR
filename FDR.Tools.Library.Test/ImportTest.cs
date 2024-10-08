﻿using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;

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
        private string source101;
        private string source2;
        private string destinationRoot;
        private string dest1;
        private string dest2;
        private string raw1;
        private string raw2;

        private CancellationToken token = new();

        private const int importConfigCount = 4;
        private const int renameConfigCount = 1;
        private const string appConfigJson = """
{
  "RenameConfigs": {
    "yymmdd_ccc": { "FileFilter": "*.*", "FileNamePattern": "{cdate:yyMMdd}_{counter:3}", "Recursive": false, "StopOnError": true } },
  "MoveConfigs": {
    "raw": { "FileFilter": "*.CR3|*.CR2|*.CRW|*.DNG", "RelativeFolder": "RAW" } },
  "ImportConfigs": {
    "import1": {
      "Name": "import1",
      "DestStructure": "date",
      "DateFormat": "yyyyMMdd",
      "Rules": [ { "Type": "contains_folder", "Param": "???CANON" }, { "Type": "contains_folder", "Param": "CANONMSC" } ],
      "Actions": [ { "Type": "rename", "Config": "yymmdd_ccc" }, { "Type": "move", "Config": "raw" } ]
    },
    "import2": {
      "Name": "import2",
      "DestStructure": "date",
      "DateFormat": "yyyyMMdd",
      "Rules": [ { "Type": "contains_folder", "Param": "pictures" } ],
      "Actions": [ { "Type": "rename", "Config": "yymmdd_ccc" }, { "Type": "move", "Config": "raw" } ]
    },
    "import3": {
      "Name": "import3",
      "Rules": [ { "Type": "contains_file", "Param": "dummy" } ]
    },
    "import4": {
      "Name": "import4",
      "Rules": [ { "Type": "volume_label", "Param": "dummy" } ]
    }
  }
}
""";

        AppConfig appConfig;
        RenameConfig renameConfig;
        MoveConfig moveConfig;
        ImportConfig importConfig;


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
            source101 = Path.Combine(dcim1, "101CANON");
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

            appConfig = new AppConfig();
            renameConfig = new RenameConfig() { FilenamePattern = "{cdate:yyMMdd}_{counter:auto}" };
            moveConfig = new MoveConfig() { FileFilter = "*.CR?|*.DNG", RelativeFolder = "RAW" };
            importConfig = new ImportConfig() { DestRoot = destinationRoot, DestStructure = FolderStructure.year_date };
            appConfig.RenameConfigs.Add("rc", renameConfig);
            appConfig.MoveConfigs.Add("mc", moveConfig);
            var renameAction = new Action(appConfig) { Type = ActionType.rename, Config = "rc" };
            var moveAction = new Action(appConfig) { Type = ActionType.move, Config = "mc" };
            importConfig.Actions.Add(renameAction);
            importConfig.Actions.Add(moveAction);
            appConfig.ImportConfigs.Add("ic", importConfig);
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
        public void ImportFileTests()
        {
            files.Add(source1, "01.crw", dest1, "100CANON_01.crw", new DateTime(2022, 01, 01));
            files.Add(source1, "02.cr2", dest1, "100CANON_02.cr2", new DateTime(2022, 01, 01));
            files.Add(source1, "03.cr3", dest1, "100CANON_03.cr3", new DateTime(2022, 01, 01));
            files.Add(source1, "04.dng", dest1, "100CANON_04.dng", new DateTime(2022, 01, 01));
            files.Add(source1, "05.jpg", dest1, "100CANON_05.jpg", new DateTime(2022, 01, 01));
            files.CreateFiles();

            var count = files.Where(f => f.SourceFolder != f.DestFolder).Count();
            int i = 0;
            foreach (var f in files)
            {
                i++;
                Import.ImportFile(destinationRoot, new ExifFile(new FileInfo(f.GetSourcePath())), FolderStructure.date, "yyyyMMdd", 100 * i / count);
            }

            files.Validate();
        }

        [Test]
        public void MoveFilesInFolderTests()
        {
            files.Add(dest1, "01.crw", raw1, "01.crw");
            files.Add(dest1, "02.cr2", raw1, "02.cr2");
            files.Add(dest1, "03.cr3", raw1, "03.cr3");
            files.Add(dest1, "04.dng", raw1, "04.dng");
            files.Add(dest1, "05.jpg", dest1, "05.jpg");
            files.CreateFiles();

            var config = new MoveConfig();
            config.FileFilter = "*.CR3|*.CR2|*.CRW|*.DNG";
            config.RelativeFolder = "RAW";

            Rename.MoveFilesInFolder(new DirectoryInfo(dest1), config);

            files.Validate();
        }

        [Test]
        public void ImportTest1()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);
            appConfig.ImportConfigs.ToList().ForEach(ic => ic.Value.DestRoot = destinationRoot);
            appConfig.Validate();

            files.Add(source1, "aaa.cr3", raw1, "220101_004.cr3", new DateTime(2022, 1, 1, 13, 59, 4));
            files.Add(source1, "aaa.jpg", dest1, "220101_004.jpg", new DateTime(2022, 1, 1, 13, 59, 4));
            files.Add(source1, "bbb.cr2", raw1, "220101_003.cr2", new DateTime(2022, 1, 1, 13, 59, 3));
            files.Add(source1, "bbb.jpg", dest1, "220101_003.jpg", new DateTime(2022, 1, 1, 13, 59, 3));
            files.Add(source1, "ccc.crw", raw1, "220101_002.crw", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.jpg", dest1, "220101_002.jpg", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ddd.dng", raw1, "220101_001.dng", new DateTime(2022, 1, 1, 13, 59, 1));
            files.Add(source1, "ddd.jpg", dest1, "220101_001.jpg", new DateTime(2022, 1, 1, 13, 59, 1));
            files.CreateFiles();

            Directory.Delete(dest1, true);
            var import1 = appConfig.ImportConfigs["import1"];
            Import.ImportFiles(new DirectoryInfo(source1), import1, false, token);

            files.Validate();
        }

        [Test]
        public void ImportTest1WithoutActions()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);
            appConfig.ImportConfigs.ToList().ForEach(ic => ic.Value.DestRoot = destinationRoot);
            appConfig.Validate();

            files.Add(source1, "aaa.cr3", dest1, "100CANON_aaa.cr3", new DateTime(2022, 1, 1, 13, 59, 4));
            files.Add(source1, "aaa.jpg", dest1, "100CANON_aaa.jpg", new DateTime(2022, 1, 1, 13, 59, 4));
            files.Add(source1, "bbb.cr2", dest1, "100CANON_bbb.cr2", new DateTime(2022, 1, 1, 13, 59, 3));
            files.Add(source1, "bbb.jpg", dest1, "100CANON_bbb.jpg", new DateTime(2022, 1, 1, 13, 59, 3));
            files.Add(source1, "ccc.crw", dest1, "100CANON_ccc.crw", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.jpg", dest1, "100CANON_ccc.jpg", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ddd.dng", dest1, "100CANON_ddd.dng", new DateTime(2022, 1, 1, 13, 59, 1));
            files.Add(source1, "ddd.jpg", dest1, "100CANON_ddd.jpg", new DateTime(2022, 1, 1, 13, 59, 1));
            files.CreateFiles();

            Directory.Delete(dest1, true);
            var import1 = appConfig.ImportConfigs["import1"];
            import1.Actions?.Clear();
            Import.ImportFiles(new DirectoryInfo(source1), import1, false, token);

            files.Validate();
        }

        [Test]
        public void ImportTest2()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);
            appConfig.ImportConfigs.ToList().ForEach(ic => ic.Value.DestRoot = destinationRoot);
            appConfig.Validate();

            files.Add(source2, "aaa.cr3", raw2, "220202_004.cr3", new DateTime(2022, 2, 2, 13, 59, 4));
            files.Add(source2, "aaa.jpg", dest2, "220202_004.jpg", new DateTime(2022, 2, 2, 13, 59, 4));
            files.Add(source2, "bbb.cr2", raw2, "220202_003.cr2", new DateTime(2022, 2, 2, 13, 59, 3));
            files.Add(source2, "bbb.jpg", dest2, "220202_003.jpg", new DateTime(2022, 2, 2, 13, 59, 3));
            files.Add(source2, "ccc.crw", raw2, "220202_002.crw", new DateTime(2022, 2, 2, 13, 59, 2));
            files.Add(source2, "ccc.jpg", dest2, "220202_002.jpg", new DateTime(2022, 2, 2, 13, 59, 2));
            files.Add(source2, "ddd.dng", raw2, "220202_001.dng", new DateTime(2022, 2, 2, 13, 59, 1));
            files.Add(source2, "ddd.jpg", dest2, "220202_001.jpg", new DateTime(2022, 2, 2, 13, 59, 1));
            files.CreateFiles();

            Directory.Delete(dest2, true);
            var import2 = appConfig.ImportConfigs["import2"];
            Import.ImportFiles(new DirectoryInfo(source2), import2, false, token);

            files.Validate();
        }

        [Test]
        public void ImportTest2WithoutActions()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);
            appConfig.ImportConfigs.ToList().ForEach(ic => ic.Value.DestRoot = destinationRoot);
            appConfig.Validate();

            files.Add(source2, "aaa.cr3", dest2, "pictures_aaa.cr3", new DateTime(2022, 2, 2, 13, 59, 4));
            files.Add(source2, "aaa.jpg", dest2, "pictures_aaa.jpg", new DateTime(2022, 2, 2, 13, 59, 4));
            files.Add(source2, "bbb.cr2", dest2, "pictures_bbb.cr2", new DateTime(2022, 2, 2, 13, 59, 3));
            files.Add(source2, "bbb.jpg", dest2, "pictures_bbb.jpg", new DateTime(2022, 2, 2, 13, 59, 3));
            files.Add(source2, "ccc.crw", dest2, "pictures_ccc.crw", new DateTime(2022, 2, 2, 13, 59, 2));
            files.Add(source2, "ccc.jpg", dest2, "pictures_ccc.jpg", new DateTime(2022, 2, 2, 13, 59, 2));
            files.Add(source2, "ddd.dng", dest2, "pictures_ddd.dng", new DateTime(2022, 2, 2, 13, 59, 1));
            files.Add(source2, "ddd.jpg", dest2, "pictures_ddd.jpg", new DateTime(2022, 2, 2, 13, 59, 1));
            files.CreateFiles();

            Directory.Delete(dest2, true);
            var import2 = appConfig.ImportConfigs["import2"];
            import2.Actions?.Clear();
            Import.ImportFiles(new DirectoryInfo(source2), import2, false, token);

            files.Validate();
        }

        [Test]
        public void ImportFromSeveralDirectoriesIntoOneWithSameNamedFiles()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);
            appConfig.ImportConfigs.ToList().ForEach(ic => ic.Value.DestRoot = destinationRoot);
            appConfig.Validate();

            files.Add(source1, "aaa.cr3", raw1, "220101_004.cr3", new DateTime(2022, 1, 1, 13, 59, 4));
            files.Add(source1, "aaa.jpg", dest1, "220101_004.jpg", new DateTime(2022, 1, 1, 13, 59, 4));
            files.Add(source1, "bbb.cr2", raw1, "220101_003.cr2", new DateTime(2022, 1, 1, 13, 59, 3));
            files.Add(source1, "bbb.jpg", dest1, "220101_003.jpg", new DateTime(2022, 1, 1, 13, 59, 3));
            files.Add(source1, "ccc.crw", raw1, "220101_002.crw", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.jpg", dest1, "220101_002.jpg", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ddd.dng", raw1, "220101_001.dng", new DateTime(2022, 1, 1, 13, 59, 1));
            files.Add(source1, "ddd.jpg", dest1, "220101_001.jpg", new DateTime(2022, 1, 1, 13, 59, 1));

            files.Add(source101, "aaa.cr3", raw1, "220101_008.cr3", new DateTime(2022, 1, 1, 13, 59, 8));
            files.Add(source101, "aaa.jpg", dest1, "220101_008.jpg", new DateTime(2022, 1, 1, 13, 59, 8));
            files.Add(source101, "bbb.cr2", raw1, "220101_007.cr2", new DateTime(2022, 1, 1, 13, 59, 7));
            files.Add(source101, "bbb.jpg", dest1, "220101_007.jpg", new DateTime(2022, 1, 1, 13, 59, 7));
            files.Add(source101, "ccc.crw", raw1, "220101_006.crw", new DateTime(2022, 1, 1, 13, 59, 6));
            files.Add(source101, "ccc.jpg", dest1, "220101_006.jpg", new DateTime(2022, 1, 1, 13, 59, 6));
            files.Add(source101, "ddd.dng", raw1, "220101_005.dng", new DateTime(2022, 1, 1, 13, 59, 5));
            files.Add(source101, "ddd.jpg", dest1, "220101_005.jpg", new DateTime(2022, 1, 1, 13, 59, 5));

            files.CreateFiles();

            Directory.Delete(dest1, true);
            var import1 = appConfig.ImportConfigs["import1"];
            Import.ImportFiles(new DirectoryInfo(dcim1), import1, false, token);

            files.Validate();
        }

        [Test]
        public void ImportFromSeveralDirectoriesIntoOneWithSameNamedFilesWithoutActions()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);
            appConfig.ImportConfigs.ToList().ForEach(ic => ic.Value.DestRoot = destinationRoot);
            appConfig.Validate();

            files.Add(source1, "aaa.cr3", dest1, "100CANON_aaa.cr3", new DateTime(2022, 1, 1, 13, 59, 4));
            files.Add(source1, "aaa.jpg", dest1, "100CANON_aaa.jpg", new DateTime(2022, 1, 1, 13, 59, 4));
            files.Add(source1, "bbb.cr2", dest1, "100CANON_bbb.cr2", new DateTime(2022, 1, 1, 13, 59, 3));
            files.Add(source1, "bbb.jpg", dest1, "100CANON_bbb.jpg", new DateTime(2022, 1, 1, 13, 59, 3));
            files.Add(source1, "ccc.crw", dest1, "100CANON_ccc.crw", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.jpg", dest1, "100CANON_ccc.jpg", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ddd.dng", dest1, "100CANON_ddd.dng", new DateTime(2022, 1, 1, 13, 59, 1));
            files.Add(source1, "ddd.jpg", dest1, "100CANON_ddd.jpg", new DateTime(2022, 1, 1, 13, 59, 1));

            files.Add(source101, "aaa.cr3", dest1, "101CANON_aaa.cr3", new DateTime(2022, 1, 1, 13, 59, 8));
            files.Add(source101, "aaa.jpg", dest1, "101CANON_aaa.jpg", new DateTime(2022, 1, 1, 13, 59, 8));
            files.Add(source101, "bbb.cr2", dest1, "101CANON_bbb.cr2", new DateTime(2022, 1, 1, 13, 59, 7));
            files.Add(source101, "bbb.jpg", dest1, "101CANON_bbb.jpg", new DateTime(2022, 1, 1, 13, 59, 7));
            files.Add(source101, "ccc.crw", dest1, "101CANON_ccc.crw", new DateTime(2022, 1, 1, 13, 59, 6));
            files.Add(source101, "ccc.jpg", dest1, "101CANON_ccc.jpg", new DateTime(2022, 1, 1, 13, 59, 6));
            files.Add(source101, "ddd.dng", dest1, "101CANON_ddd.dng", new DateTime(2022, 1, 1, 13, 59, 5));
            files.Add(source101, "ddd.jpg", dest1, "101CANON_ddd.jpg", new DateTime(2022, 1, 1, 13, 59, 5));

            files.CreateFiles();

            Directory.Delete(dest1, true);
            var import1 = appConfig.ImportConfigs["import1"];
            import1.Actions?.Clear();
            Import.ImportFiles(new DirectoryInfo(dcim1), import1, false, token);

            files.Validate();
        }

        [Test]
        public void ForceImportWithExistingFiles()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);
            appConfig.ImportConfigs.ToList().ForEach(ic => ic.Value.DestRoot = destinationRoot);
            appConfig.Validate();

            files.Add(source1, "aaa.cr3", raw1, "220101_004.cr3", new DateTime(2022, 1, 1, 13, 59, 4));
            files.Add(source1, "aaa.jpg", dest1, "220101_004.jpg", new DateTime(2022, 1, 1, 13, 59, 4));
            files.Add(source1, "bbb.cr2", raw1, "220101_003.cr2", new DateTime(2022, 1, 1, 13, 59, 3));
            files.Add(source1, "bbb.jpg", dest1, "220101_003.jpg", new DateTime(2022, 1, 1, 13, 59, 3));
            files.Add(source1, "ccc.crw", raw1, "220101_002.crw", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.jpg", dest1, "220101_002.jpg", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ddd.dng", raw1, "220101_001.dng", new DateTime(2022, 1, 1, 13, 59, 1));
            files.Add(source1, "ddd.jpg", dest1, "220101_001.jpg", new DateTime(2022, 1, 1, 13, 59, 1));

            files.Add(dest1, "aaa.cr3", raw1, "220101_008.cr3", new DateTime(2022, 1, 1, 13, 59, 8));
            files.Add(dest1, "aaa.jpg", dest1, "220101_008.jpg", new DateTime(2022, 1, 1, 13, 59, 8));
            files.Add(dest1, "bbb.cr2", raw1, "220101_007.cr2", new DateTime(2022, 1, 1, 13, 59, 7));
            files.Add(dest1, "bbb.jpg", dest1, "220101_007.jpg", new DateTime(2022, 1, 1, 13, 59, 7));
            files.Add(dest1, "ccc.crw", raw1, "220101_006.crw", new DateTime(2022, 1, 1, 13, 59, 6));
            files.Add(dest1, "ccc.jpg", dest1, "220101_006.jpg", new DateTime(2022, 1, 1, 13, 59, 6));
            files.Add(dest1, "ddd.dng", raw1, "220101_005.dng", new DateTime(2022, 1, 1, 13, 59, 5));
            files.Add(dest1, "ddd.jpg", dest1, "220101_005.jpg", new DateTime(2022, 1, 1, 13, 59, 5));

            Directory.Delete(dest1, true);
            files.CreateFiles();

            var import1 = appConfig.ImportConfigs["import1"];
            Import.ImportFiles(new DirectoryInfo(source1), import1, true, token);

            files.Validate();
        }

        [Test]
        public void ImportMultipleDaysInReverseOrder()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.RenameConfigs.Should().HaveCount(renameConfigCount);
            appConfig.RenameConfigs["yymmdd_ccc"].Should().NotBeNull();
            appConfig.RenameConfigs["yymmdd_ccc"].FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);
            appConfig.ImportConfigs.ToList().ForEach(ic => ic.Value.DestRoot = destinationRoot);
            appConfig.Validate();

            files.Add(source1, "aaa.cr3", raw2, "220202_12.cr3", new DateTime(2022, 2, 2, 13, 59, 14));
            files.Add(source1, "aaa.jpg", dest2, "220202_12.jpg", new DateTime(2022, 2, 2, 13, 59, 14));
            files.Add(source1, "bbb.cr3", raw2, "220202_11.cr3", new DateTime(2022, 2, 2, 13, 59, 13));
            files.Add(source1, "bbb.jpg", dest2, "220202_11.jpg", new DateTime(2022, 2, 2, 13, 59, 13));
            files.Add(source1, "ccc.cr3", raw2, "220202_10.cr3", new DateTime(2022, 2, 2, 13, 59, 12));
            files.Add(source1, "ccc.jpg", dest2, "220202_10.jpg", new DateTime(2022, 2, 2, 13, 59, 12));
            files.Add(source1, "ddd.cr3", raw2, "220202_09.cr3", new DateTime(2022, 2, 2, 13, 59, 11));
            files.Add(source1, "ddd.jpg", dest2, "220202_09.jpg", new DateTime(2022, 2, 2, 13, 59, 11));
            files.Add(source1, "eee.cr3", raw2, "220202_08.cr3", new DateTime(2022, 2, 2, 13, 59, 10));
            files.Add(source1, "eee.jpg", dest2, "220202_08.jpg", new DateTime(2022, 2, 2, 13, 59, 10));
            files.Add(source1, "fff.cr3", raw2, "220202_07.cr3", new DateTime(2022, 2, 2, 13, 59, 9));
            files.Add(source1, "fff.jpg", dest2, "220202_07.jpg", new DateTime(2022, 2, 2, 13, 59, 9));
            files.Add(source1, "ggg.cr3", raw2, "220202_06.cr3", new DateTime(2022, 2, 2, 13, 59, 8));
            files.Add(source1, "ggg.jpg", dest2, "220202_06.jpg", new DateTime(2022, 2, 2, 13, 59, 8));
            files.Add(source1, "hhh.cr3", raw2, "220202_05.cr3", new DateTime(2022, 2, 2, 13, 59, 7));
            files.Add(source1, "hhh.jpg", dest2, "220202_05.jpg", new DateTime(2022, 2, 2, 13, 59, 7));
            files.Add(source1, "iii.cr3", raw2, "220202_04.cr3", new DateTime(2022, 2, 2, 13, 59, 6));
            files.Add(source1, "iii.jpg", dest2, "220202_04.jpg", new DateTime(2022, 2, 2, 13, 59, 6));
            files.Add(source1, "jjj.cr3", raw2, "220202_03.cr3", new DateTime(2022, 2, 2, 13, 59, 5));
            files.Add(source1, "jjj.jpg", dest2, "220202_03.jpg", new DateTime(2022, 2, 2, 13, 59, 5));
            files.Add(source1, "kkk.cr3", raw2, "220202_02.cr3", new DateTime(2022, 2, 2, 13, 59, 4));
            files.Add(source1, "kkk.jpg", dest2, "220202_02.jpg", new DateTime(2022, 2, 2, 13, 59, 4));
            files.Add(source1, "lll.cr2", raw2, "220202_01.cr2", new DateTime(2022, 2, 2, 13, 59, 3));
            files.Add(source1, "lll.jpg", dest2, "220202_01.jpg", new DateTime(2022, 2, 2, 13, 59, 3));
            files.Add(source1, "mmm.crw", raw1, "220101_2.crw", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "mmm.jpg", dest1, "220101_2.jpg", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "nnn.dng", raw1, "220101_1.dng", new DateTime(2022, 1, 1, 13, 59, 1));
            files.Add(source1, "nnn.jpg", dest1, "220101_1.jpg", new DateTime(2022, 1, 1, 13, 59, 1));
            files.CreateFiles();

            Directory.Delete(dest1, true);
            Directory.Delete(dest2, true);
            var import1 = appConfig.ImportConfigs["import1"];
            Import.ImportFiles(new DirectoryInfo(source1), import1, false, token);

            files.Validate();
        }

        [Test]
        public void ImportMultipleDaysInDirectOrder()
        {
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigJson);
            appConfig.Should().NotBeNull();
            appConfig.RenameConfigs.Should().HaveCount(renameConfigCount);
            appConfig.RenameConfigs["yymmdd_ccc"].Should().NotBeNull();
            appConfig.RenameConfigs["yymmdd_ccc"].FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";
            appConfig.ImportConfigs.Should().HaveCount(importConfigCount);
            appConfig.ImportConfigs.ToList().ForEach(ic => ic.Value.DestRoot = destinationRoot);
            appConfig.Validate();

            files.Add(source1, "aaa.dng", raw1, "220101_1.dng", new DateTime(2022, 1, 1, 13, 59, 1));
            files.Add(source1, "aaa.jpg", dest1, "220101_1.jpg", new DateTime(2022, 1, 1, 13, 59, 1));
            files.Add(source1, "bbb.crw", raw1, "220101_2.crw", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "bbb.jpg", dest1, "220101_2.jpg", new DateTime(2022, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.cr2", raw2, "220202_01.cr2", new DateTime(2022, 2, 2, 13, 59, 3));
            files.Add(source1, "ccc.jpg", dest2, "220202_01.jpg", new DateTime(2022, 2, 2, 13, 59, 3));
            files.Add(source1, "ddd.cr3", raw2, "220202_02.cr3", new DateTime(2022, 2, 2, 13, 59, 4));
            files.Add(source1, "ddd.jpg", dest2, "220202_02.jpg", new DateTime(2022, 2, 2, 13, 59, 4));
            files.Add(source1, "eee.cr3", raw2, "220202_03.cr3", new DateTime(2022, 2, 2, 13, 59, 5));
            files.Add(source1, "eee.jpg", dest2, "220202_03.jpg", new DateTime(2022, 2, 2, 13, 59, 5));
            files.Add(source1, "fff.cr3", raw2, "220202_04.cr3", new DateTime(2022, 2, 2, 13, 59, 6));
            files.Add(source1, "fff.jpg", dest2, "220202_04.jpg", new DateTime(2022, 2, 2, 13, 59, 6));
            files.Add(source1, "ggg.cr3", raw2, "220202_05.cr3", new DateTime(2022, 2, 2, 13, 59, 7));
            files.Add(source1, "ggg.jpg", dest2, "220202_05.jpg", new DateTime(2022, 2, 2, 13, 59, 7));
            files.Add(source1, "hhh.cr3", raw2, "220202_06.cr3", new DateTime(2022, 2, 2, 13, 59, 8));
            files.Add(source1, "hhh.jpg", dest2, "220202_06.jpg", new DateTime(2022, 2, 2, 13, 59, 8));
            files.Add(source1, "iii.cr3", raw2, "220202_07.cr3", new DateTime(2022, 2, 2, 13, 59, 9));
            files.Add(source1, "iii.jpg", dest2, "220202_07.jpg", new DateTime(2022, 2, 2, 13, 59, 9));
            files.Add(source1, "jjj.cr3", raw2, "220202_08.cr3", new DateTime(2022, 2, 2, 13, 59, 10));
            files.Add(source1, "jjj.jpg", dest2, "220202_08.jpg", new DateTime(2022, 2, 2, 13, 59, 10));
            files.Add(source1, "kkk.cr3", raw2, "220202_09.cr3", new DateTime(2022, 2, 2, 13, 59, 11));
            files.Add(source1, "kkk.jpg", dest2, "220202_09.jpg", new DateTime(2022, 2, 2, 13, 59, 11));
            files.Add(source1, "lll.cr3", raw2, "220202_10.cr3", new DateTime(2022, 2, 2, 13, 59, 12));
            files.Add(source1, "lll.jpg", dest2, "220202_10.jpg", new DateTime(2022, 2, 2, 13, 59, 12));
            files.Add(source1, "mmm.cr3", raw2, "220202_11.cr3", new DateTime(2022, 2, 2, 13, 59, 13));
            files.Add(source1, "mmm.jpg", dest2, "220202_11.jpg", new DateTime(2022, 2, 2, 13, 59, 13));
            files.Add(source1, "nnn.cr3", raw2, "220202_12.cr3", new DateTime(2022, 2, 2, 13, 59, 14));
            files.Add(source1, "nnn.jpg", dest2, "220202_12.jpg", new DateTime(2022, 2, 2, 13, 59, 14));
            files.CreateFiles();

            Directory.Delete(dest1, true);
            Directory.Delete(dest2, true);
            var import1 = appConfig.ImportConfigs["import1"];
            Import.ImportFiles(new DirectoryInfo(source1), import1, false, token);

            files.Validate();
        }

        [Test]
        public void ImportDate()
        {
            renameConfig.FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";
            importConfig.DestStructure = FolderStructure.date;
            appConfig.Validate();

            files.Add(source1, "aaa.dng", destinationRoot, "230101/RAW/230101_1.dng", new DateTime(2023, 1, 1, 13, 59, 1));
            files.Add(source1, "aaa.jpg", destinationRoot, "230101/230101_1.jpg", new DateTime(2023, 1, 1, 13, 59, 1));
            files.Add(source1, "bbb.crw", destinationRoot, "230101/RAW/230101_2.crw", new DateTime(2023, 1, 1, 13, 59, 2));
            files.Add(source1, "bbb.jpg", destinationRoot, "230101/230101_2.jpg", new DateTime(2023, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.cr2", destinationRoot, "230102/RAW/230102_1.cr2", new DateTime(2023, 1, 2, 13, 59, 3));
            files.Add(source1, "ccc.jpg", destinationRoot, "230102/230102_1.jpg", new DateTime(2023, 1, 2, 13, 59, 3));
            files.Add(source1, "ddd.cr3", destinationRoot, "230102/RAW/230102_2.cr3", new DateTime(2023, 1, 2, 13, 59, 4));
            files.Add(source1, "ddd.jpg", destinationRoot, "230102/230102_2.jpg", new DateTime(2023, 1, 2, 13, 59, 4));
            files.Add(source1, "eee.cr3", destinationRoot, "230201/RAW/230201_1.cr3", new DateTime(2023, 2, 1, 13, 59, 5));
            files.Add(source1, "eee.jpg", destinationRoot, "230201/230201_1.jpg", new DateTime(2023, 2, 1, 13, 59, 5));
            files.Add(source1, "fff.cr3", destinationRoot, "230201/RAW/230201_2.cr3", new DateTime(2023, 2, 1, 13, 59, 6));
            files.Add(source1, "fff.jpg", destinationRoot, "230201/230201_2.jpg", new DateTime(2023, 2, 1, 13, 59, 6));
            files.Add(source1, "ggg.cr3", destinationRoot, "240101/RAW/240101_1.cr3", new DateTime(2024, 1, 1, 13, 59, 7));
            files.Add(source1, "ggg.jpg", destinationRoot, "240101/240101_1.jpg", new DateTime(2024, 1, 1, 13, 59, 7));
            files.Add(source1, "hhh.cr3", destinationRoot, "240101/RAW/240101_2.cr3", new DateTime(2024, 1, 1, 13, 59, 8));
            files.Add(source1, "hhh.jpg", destinationRoot, "240101/240101_2.jpg", new DateTime(2024, 1, 1, 13, 59, 8));
            files.Add(source1, "iii.cr3", destinationRoot, "240202/RAW/240202_1.cr3", new DateTime(2024, 2, 2, 13, 59, 9));
            files.Add(source1, "iii.jpg", destinationRoot, "240202/240202_1.jpg", new DateTime(2024, 2, 2, 13, 59, 9));
            files.CreateFiles();

            Import.ImportFiles(new DirectoryInfo(source1), importConfig, false, token);

            files.Validate();
        }

        [Test]
        public void ImportYearDate()
        {
            renameConfig.FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";
            importConfig.DestStructure = FolderStructure.year_date;
            appConfig.Validate();

            files.Add(source1, "aaa.dng", destinationRoot, "2023/230101/RAW/230101_1.dng", new DateTime(2023, 1, 1, 13, 59, 1));
            files.Add(source1, "aaa.jpg", destinationRoot, "2023/230101/230101_1.jpg", new DateTime(2023, 1, 1, 13, 59, 1));
            files.Add(source1, "bbb.crw", destinationRoot, "2023/230101/RAW/230101_2.crw", new DateTime(2023, 1, 1, 13, 59, 2));
            files.Add(source1, "bbb.jpg", destinationRoot, "2023/230101/230101_2.jpg", new DateTime(2023, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.cr2", destinationRoot, "2023/230102/RAW/230102_1.cr2", new DateTime(2023, 1, 2, 13, 59, 3));
            files.Add(source1, "ccc.jpg", destinationRoot, "2023/230102/230102_1.jpg", new DateTime(2023, 1, 2, 13, 59, 3));
            files.Add(source1, "ddd.cr3", destinationRoot, "2023/230102/RAW/230102_2.cr3", new DateTime(2023, 1, 2, 13, 59, 4));
            files.Add(source1, "ddd.jpg", destinationRoot, "2023/230102/230102_2.jpg", new DateTime(2023, 1, 2, 13, 59, 4));
            files.Add(source1, "eee.cr3", destinationRoot, "2023/230201/RAW/230201_1.cr3", new DateTime(2023, 2, 1, 13, 59, 5));
            files.Add(source1, "eee.jpg", destinationRoot, "2023/230201/230201_1.jpg", new DateTime(2023, 2, 1, 13, 59, 5));
            files.Add(source1, "fff.cr3", destinationRoot, "2023/230201/RAW/230201_2.cr3", new DateTime(2023, 2, 1, 13, 59, 6));
            files.Add(source1, "fff.jpg", destinationRoot, "2023/230201/230201_2.jpg", new DateTime(2023, 2, 1, 13, 59, 6));
            files.Add(source1, "ggg.cr3", destinationRoot, "2024/240101/RAW/240101_1.cr3", new DateTime(2024, 1, 1, 13, 59, 7));
            files.Add(source1, "ggg.jpg", destinationRoot, "2024/240101/240101_1.jpg", new DateTime(2024, 1, 1, 13, 59, 7));
            files.Add(source1, "hhh.cr3", destinationRoot, "2024/240101/RAW/240101_2.cr3", new DateTime(2024, 1, 1, 13, 59, 8));
            files.Add(source1, "hhh.jpg", destinationRoot, "2024/240101/240101_2.jpg", new DateTime(2024, 1, 1, 13, 59, 8));
            files.Add(source1, "iii.cr3", destinationRoot, "2024/240202/RAW/240202_1.cr3", new DateTime(2024, 2, 2, 13, 59, 9));
            files.Add(source1, "iii.jpg", destinationRoot, "2024/240202/240202_1.jpg", new DateTime(2024, 2, 2, 13, 59, 9));
            files.CreateFiles();

            Import.ImportFiles(new DirectoryInfo(source1), importConfig, false, token);

            files.Validate();
        }

        [Test]
        public void ImportYearMonthDate()
        {
            renameConfig.FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";
            importConfig.DestStructure = FolderStructure.year_month_date;
            appConfig.Validate();

            files.Add(source1, "aaa.dng", destinationRoot, "2023/01/230101/RAW/230101_1.dng", new DateTime(2023, 1, 1, 13, 59, 1));
            files.Add(source1, "aaa.jpg", destinationRoot, "2023/01/230101/230101_1.jpg", new DateTime(2023, 1, 1, 13, 59, 1));
            files.Add(source1, "bbb.crw", destinationRoot, "2023/01/230101/RAW/230101_2.crw", new DateTime(2023, 1, 1, 13, 59, 2));
            files.Add(source1, "bbb.jpg", destinationRoot, "2023/01/230101/230101_2.jpg", new DateTime(2023, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.cr2", destinationRoot, "2023/01/230102/RAW/230102_1.cr2", new DateTime(2023, 1, 2, 13, 59, 3));
            files.Add(source1, "ccc.jpg", destinationRoot, "2023/01/230102/230102_1.jpg", new DateTime(2023, 1, 2, 13, 59, 3));
            files.Add(source1, "ddd.cr3", destinationRoot, "2023/01/230102/RAW/230102_2.cr3", new DateTime(2023, 1, 2, 13, 59, 4));
            files.Add(source1, "ddd.jpg", destinationRoot, "2023/01/230102/230102_2.jpg", new DateTime(2023, 1, 2, 13, 59, 4));
            files.Add(source1, "eee.cr3", destinationRoot, "2023/02/230201/RAW/230201_1.cr3", new DateTime(2023, 2, 1, 13, 59, 5));
            files.Add(source1, "eee.jpg", destinationRoot, "2023/02/230201/230201_1.jpg", new DateTime(2023, 2, 1, 13, 59, 5));
            files.Add(source1, "fff.cr3", destinationRoot, "2023/02/230201/RAW/230201_2.cr3", new DateTime(2023, 2, 1, 13, 59, 6));
            files.Add(source1, "fff.jpg", destinationRoot, "2023/02/230201/230201_2.jpg", new DateTime(2023, 2, 1, 13, 59, 6));
            files.Add(source1, "ggg.cr3", destinationRoot, "2024/01/240101/RAW/240101_1.cr3", new DateTime(2024, 1, 1, 13, 59, 7));
            files.Add(source1, "ggg.jpg", destinationRoot, "2024/01/240101/240101_1.jpg", new DateTime(2024, 1, 1, 13, 59, 7));
            files.Add(source1, "hhh.cr3", destinationRoot, "2024/01/240101/RAW/240101_2.cr3", new DateTime(2024, 1, 1, 13, 59, 8));
            files.Add(source1, "hhh.jpg", destinationRoot, "2024/01/240101/240101_2.jpg", new DateTime(2024, 1, 1, 13, 59, 8));
            files.Add(source1, "iii.cr3", destinationRoot, "2024/02/240202/RAW/240202_1.cr3", new DateTime(2024, 2, 2, 13, 59, 9));
            files.Add(source1, "iii.jpg", destinationRoot, "2024/02/240202/240202_1.jpg", new DateTime(2024, 2, 2, 13, 59, 9));
            files.CreateFiles();

            Import.ImportFiles(new DirectoryInfo(source1), importConfig, false, token);

            files.Validate();
        }

        [Test]
        public void ImportYearMonthDay()
        {
            renameConfig.FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";
            importConfig.DestStructure = FolderStructure.year_month_day;
            appConfig.Validate();

            files.Add(source1, "aaa.dng", destinationRoot, "2023/01/01/RAW/230101_1.dng", new DateTime(2023, 1, 1, 13, 59, 1));
            files.Add(source1, "aaa.jpg", destinationRoot, "2023/01/01/230101_1.jpg", new DateTime(2023, 1, 1, 13, 59, 1));
            files.Add(source1, "bbb.crw", destinationRoot, "2023/01/01/RAW/230101_2.crw", new DateTime(2023, 1, 1, 13, 59, 2));
            files.Add(source1, "bbb.jpg", destinationRoot, "2023/01/01/230101_2.jpg", new DateTime(2023, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.cr2", destinationRoot, "2023/01/02/RAW/230102_1.cr2", new DateTime(2023, 1, 2, 13, 59, 3));
            files.Add(source1, "ccc.jpg", destinationRoot, "2023/01/02/230102_1.jpg", new DateTime(2023, 1, 2, 13, 59, 3));
            files.Add(source1, "ddd.cr3", destinationRoot, "2023/01/02/RAW/230102_2.cr3", new DateTime(2023, 1, 2, 13, 59, 4));
            files.Add(source1, "ddd.jpg", destinationRoot, "2023/01/02/230102_2.jpg", new DateTime(2023, 1, 2, 13, 59, 4));
            files.Add(source1, "eee.cr3", destinationRoot, "2023/02/01/RAW/230201_1.cr3", new DateTime(2023, 2, 1, 13, 59, 5));
            files.Add(source1, "eee.jpg", destinationRoot, "2023/02/01/230201_1.jpg", new DateTime(2023, 2, 1, 13, 59, 5));
            files.Add(source1, "fff.cr3", destinationRoot, "2023/02/01/RAW/230201_2.cr3", new DateTime(2023, 2, 1, 13, 59, 6));
            files.Add(source1, "fff.jpg", destinationRoot, "2023/02/01/230201_2.jpg", new DateTime(2023, 2, 1, 13, 59, 6));
            files.Add(source1, "ggg.cr3", destinationRoot, "2024/01/01/RAW/240101_1.cr3", new DateTime(2024, 1, 1, 13, 59, 7));
            files.Add(source1, "ggg.jpg", destinationRoot, "2024/01/01/240101_1.jpg", new DateTime(2024, 1, 1, 13, 59, 7));
            files.Add(source1, "hhh.cr3", destinationRoot, "2024/01/01/RAW/240101_2.cr3", new DateTime(2024, 1, 1, 13, 59, 8));
            files.Add(source1, "hhh.jpg", destinationRoot, "2024/01/01/240101_2.jpg", new DateTime(2024, 1, 1, 13, 59, 8));
            files.Add(source1, "iii.cr3", destinationRoot, "2024/02/02/RAW/240202_1.cr3", new DateTime(2024, 2, 2, 13, 59, 9));
            files.Add(source1, "iii.jpg", destinationRoot, "2024/02/02/240202_1.jpg", new DateTime(2024, 2, 2, 13, 59, 9));
            files.CreateFiles();

            Import.ImportFiles(new DirectoryInfo(source1), importConfig, false, token);

            files.Validate();
        }

        [Ignore("Monthly folders don't work yet")]
        [Test]
        public void ImportYearMonth()
        {
            renameConfig.FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";
            importConfig.DestStructure = FolderStructure.year_month;
            appConfig.Validate();

            files.Add(source1, "aaa.dng", destinationRoot, "2023/01/RAW/230101_1.dng", new DateTime(2023, 1, 1, 13, 59, 1));
            files.Add(source1, "aaa.jpg", destinationRoot, "2023/01/230101_1.jpg", new DateTime(2023, 1, 1, 13, 59, 1));
            files.Add(source1, "bbb.crw", destinationRoot, "2023/01/RAW/230101_2.crw", new DateTime(2023, 1, 1, 13, 59, 2));
            files.Add(source1, "bbb.jpg", destinationRoot, "2023/01/230101_2.jpg", new DateTime(2023, 1, 1, 13, 59, 2));
            files.Add(source1, "ccc.cr2", destinationRoot, "2023/01/RAW/230102_1.cr2", new DateTime(2023, 1, 2, 13, 59, 3));
            files.Add(source1, "ccc.jpg", destinationRoot, "2023/01/230102_1.jpg", new DateTime(2023, 1, 2, 13, 59, 3));
            files.Add(source1, "ddd.cr3", destinationRoot, "2023/01/RAW/230102_2.cr3", new DateTime(2023, 1, 2, 13, 59, 4));
            files.Add(source1, "ddd.jpg", destinationRoot, "2023/01/230102_2.jpg", new DateTime(2023, 1, 2, 13, 59, 4));
            files.Add(source1, "eee.cr3", destinationRoot, "2023/02/RAW/230201_1.cr3", new DateTime(2023, 2, 1, 13, 59, 5));
            files.Add(source1, "eee.jpg", destinationRoot, "2023/02/230201_1.jpg", new DateTime(2023, 2, 1, 13, 59, 5));
            files.Add(source1, "fff.cr3", destinationRoot, "2023/02/RAW/230201_2.cr3", new DateTime(2023, 2, 1, 13, 59, 6));
            files.Add(source1, "fff.jpg", destinationRoot, "2023/02/230201_2.jpg", new DateTime(2023, 2, 1, 13, 59, 6));
            files.Add(source1, "ggg.cr3", destinationRoot, "2024/01/RAW/240101_1.cr3", new DateTime(2024, 1, 1, 13, 59, 7));
            files.Add(source1, "ggg.jpg", destinationRoot, "2024/01/240101_1.jpg", new DateTime(2024, 1, 1, 13, 59, 7));
            files.Add(source1, "hhh.cr3", destinationRoot, "2024/01/RAW/240101_2.cr3", new DateTime(2024, 1, 1, 13, 59, 8));
            files.Add(source1, "hhh.jpg", destinationRoot, "2024/01/240101_2.jpg", new DateTime(2024, 1, 1, 13, 59, 8));
            files.Add(source1, "iii.cr3", destinationRoot, "2024/02/RAW/240202_1.cr3", new DateTime(2024, 2, 2, 13, 59, 9));
            files.Add(source1, "iii.jpg", destinationRoot, "2024/02/240202_1.jpg", new DateTime(2024, 2, 2, 13, 59, 9));
            files.CreateFiles();

            Import.ImportFiles(new DirectoryInfo(source1), importConfig, false, token);

            files.Validate();
        }
    }
}
