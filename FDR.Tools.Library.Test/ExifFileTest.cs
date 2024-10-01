using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class ExifFileTest : TempFolderTestBase
    {

        [Test]
        public void ExifFileConstructor()
        {
            DateTime date = new DateTime(2001, 2, 3, 4, 5, 6);
            DateTime defaultDate = new DateTime(1601, 1, 1, 1, 0, 0);
            string fileName = "1.jpg";
            string fileFullName = Path.Combine(tempFolderPath, fileName);

            var fi = files.Add(tempFolderPath, fileName, tempFolderPath, fileName, date, date, date);
            fi.Should().NotBeNull();
            fi.CreationTime.Should().Be(defaultDate);
            fi.DirectoryName.Should().Be(tempFolderPath);
            fi.Exists.Should().BeFalse();
            fi.Extension.Should().Be(".jpg");
            fi.FullName.Should().Be(fileFullName);
            fi.LastWriteTime.Should().Be(defaultDate);
            fi.Name.Should().Be(fileName);

            var ef = new ExifFile(fi);
            ef.Should().NotBeNull();
            ef.CreationTime.Should().Be(defaultDate);
            ef.DirectoryName.Should().Be(tempFolderPath);
            ef.IsExifLoaded.Should().BeFalse();     //Has to be before .ExifTime
            ef.ExifTime.Should().Be(defaultDate);
            ef.IsExifLoaded.Should().BeTrue();      //Has to be after .ExifTime
            ef.Exists.Should().BeFalse();
            ef.Extension.Should().Be(".jpg");
            ef.FullName.Should().Be(fileFullName);
            ef.LastWriteTime.Should().Be(defaultDate);
            ef.Name.Should().Be(fileName);
            ef.OriginalFullName.Should().Be(fileFullName);
            ef.OriginalName.Should().Be(fileName);

            files.CreateFiles();

            fi.Refresh();
            fi.Should().NotBeNull();
            fi.CreationTime.Should().Be(date);
            fi.DirectoryName.Should().Be(tempFolderPath);
            fi.Exists.Should().BeTrue();
            fi.Extension.Should().Be(".jpg");
            fi.FullName.Should().Be(fileFullName);
            fi.LastWriteTime.Should().Be(date);
            fi.Name.Should().Be(fileName);

            ef = new ExifFile(fi);
            ef.Should().NotBeNull();
            ef.CreationTime.Should().Be(date);
            ef.DirectoryName.Should().Be(tempFolderPath);
            ef.IsExifLoaded.Should().BeFalse();     //Has to be before .ExifTime
            ef.ExifTime.Should().Be(date);
            ef.IsExifLoaded.Should().BeTrue();      //Has to be after .ExifTime
            ef.Exists.Should().BeTrue();
            ef.Extension.Should().Be(".jpg");
            ef.FullName.Should().Be(fileFullName);
            ef.LastWriteTime.Should().Be(date);
            ef.Name.Should().Be(fileName);
            ef.OriginalFullName.Should().Be(fileFullName);
            ef.OriginalName.Should().Be(fileName);
        }

        [Test]
        public void NewLocation()
        {
            string fileName = "1.jpg";
            string fileFullName = Path.Combine(tempFolderPath, fileName);
            string fileNewLocation = Path.Combine(tempFolderPath, "2.jpg");

            var fi = files.Add(tempFolderPath, fileName, tempFolderPath, fileName);
            files.CreateFiles();
            fi.Refresh();

            var ef = new ExifFile(fi);
            ef.Should().NotBeNull();
            ef.NewLocation.Should().BeNull();
            ef.NewLocationSpecified.Should().BeFalse();

            ef.NewLocation = fileFullName;
            ef.NewLocation.Should().BeNull();
            ef.NewLocationSpecified.Should().BeFalse();

            ef.NewLocation = fileNewLocation;
            ef.NewLocation.Should().Be(fileNewLocation);
            ef.NewLocationSpecified.Should().BeTrue();


            ef = new ExifFile(fi);
            ef.Should().NotBeNull();
            ef.NewLocation.Should().BeNull();
            ef.NewLocationSpecified.Should().BeFalse();

            var config = new RenameConfig();
            config.FilenamePattern = "2";

            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(fileNewLocation);
            ef.NewLocationSpecified.Should().BeTrue();
        }

        [Test]
        public void CopyTo()
        {
            string fileName = "1.jpg";
            string fileFullName = Path.Combine(tempFolderPath, fileName);
            string fileNewName = "2.jpg";
            string fileNewLocation = Path.Combine(tempFolderPath, fileNewName);

            var fi = files.Add(tempFolderPath, fileName, true);
            files.CreateFiles();
            fi.Refresh();
            files.Add(tempFolderPath, fileNewName, true);

            var ef = new ExifFile(fi);
            ef.Should().NotBeNull();

            var nfi = ef.CopyTo(fileNewLocation);
            nfi.FullName.Should().Be(fileNewLocation);
            nfi.Name.Should().Be(fileNewName);
            ef.FullName.Should().Be(fileFullName);
            ef.Name.Should().Be(fileName);
            ef.OriginalFullName.Should().Be(fileFullName);
            ef.OriginalName.Should().Be(fileName);

            files.Validate();
        }

        [Test]
        public void CopyToNewLocation()
        {
            string fileName = "1.jpg";
            string fileFullName = Path.Combine(tempFolderPath, fileName);
            string fileNewName = "2.jpg";
            string fileNewLocation = Path.Combine(tempFolderPath, fileNewName);

            var fi = files.Add(tempFolderPath, fileName, true);
            files.CreateFiles();
            fi.Refresh();
            files.Add(tempFolderPath, fileNewName, true);

            var ef = new ExifFile(fi);
            ef.Should().NotBeNull();
            ef.NewLocation.Should().BeNull();
            ef.NewLocationSpecified.Should().BeFalse();

            ef.NewLocation = fileNewLocation;
            ef.NewLocation.Should().Be(fileNewLocation);
            ef.NewLocationSpecified.Should().BeTrue();

            var nfi = ef.CopyToNewLocation();
            nfi.FullName.Should().Be(fileNewLocation);
            nfi.Name.Should().Be(fileNewName);
            ef.FullName.Should().Be(fileFullName);
            ef.Name.Should().Be(fileName);
            ef.OriginalFullName.Should().Be(fileFullName);
            ef.OriginalName.Should().Be(fileName);

            files.Validate();
        }

        [Test]
        public void CopyAndSwitchTo()
        {
            string fileName = "1.jpg";
            string fileFullName = Path.Combine(tempFolderPath, fileName);
            string fileNewName = "2.jpg";
            string fileNewLocation = Path.Combine(tempFolderPath, fileNewName);

            var fi = files.Add(tempFolderPath, fileName, true);
            files.CreateFiles();
            fi.Refresh();
            files.Add(tempFolderPath, fileNewName, true);

            var ef = new ExifFile(fi);
            ef.Should().NotBeNull();

            ef.CopyAndSwitchTo(fileNewLocation);
            ef.FullName.Should().Be(fileNewLocation);
            ef.Name.Should().Be(fileNewName);
            ef.OriginalFullName.Should().Be(fileFullName);
            ef.OriginalName.Should().Be(fileName);

            files.Validate();
        }

        [Test]
        public void CopyAndSwitchToNewLocation()
        {
            string fileName = "1.jpg";
            string fileFullName = Path.Combine(tempFolderPath, fileName);
            string fileNewName = "2.jpg";
            string fileNewLocation = Path.Combine(tempFolderPath, fileNewName);

            var fi = files.Add(tempFolderPath, fileName, true);
            files.CreateFiles();
            fi.Refresh();
            files.Add(tempFolderPath, fileNewName, true);

            var ef = new ExifFile(fi);
            ef.Should().NotBeNull();
            ef.NewLocation.Should().BeNull();
            ef.NewLocationSpecified.Should().BeFalse();

            ef.NewLocation = fileNewLocation;
            ef.NewLocation.Should().Be(fileNewLocation);
            ef.NewLocationSpecified.Should().BeTrue();

            ef.CopyAndSwitchToNewLocation();
            ef.FullName.Should().Be(fileNewLocation);
            ef.Name.Should().Be(fileNewName);
            ef.OriginalFullName.Should().Be(fileFullName);
            ef.OriginalName.Should().Be(fileName);

            files.Validate();
        }

        [Test]
        public void MoveTo()
        {
            string fileName = "1.jpg";
            string fileFullName = Path.Combine(tempFolderPath, fileName);
            string fileNewName = "2.jpg";
            string fileNewLocation = Path.Combine(tempFolderPath, fileNewName);

            var fi = files.Add(tempFolderPath, fileName, tempFolderPath, fileNewName);
            files.CreateFiles();
            fi.Refresh();

            var ef = new ExifFile(fi);
            ef.Should().NotBeNull();

            ef.MoveTo(fileNewLocation);
            ef.FullName.Should().Be(fileNewLocation);
            ef.Name.Should().Be(fileNewName);
            ef.OriginalFullName.Should().Be(fileFullName);
            ef.OriginalName.Should().Be(fileName);

            files.Validate();
        }

        [Test]
        public void MoveToNewLocation()
        {
            string fileName = "1.jpg";
            string fileFullName = Path.Combine(tempFolderPath, fileName);
            string fileNewName = "2.jpg";
            string fileNewLocation = Path.Combine(tempFolderPath, fileNewName);

            var fi = files.Add(tempFolderPath, fileName, tempFolderPath, fileNewName);
            files.CreateFiles();
            fi.Refresh();

            var ef = new ExifFile(fi);
            ef.Should().NotBeNull();
            ef.NewLocation.Should().BeNull();
            ef.NewLocationSpecified.Should().BeFalse();

            ef.NewLocation = fileNewLocation;
            ef.NewLocation.Should().Be(fileNewLocation);
            ef.NewLocationSpecified.Should().BeTrue();

            ef.MoveToNewLocation();
            ef.FullName.Should().Be(fileNewLocation);
            ef.Name.Should().Be(fileNewName);
            ef.NewLocation.Should().BeNull();
            ef.NewLocationSpecified.Should().BeFalse();
            ef.OriginalFullName.Should().Be(fileFullName);
            ef.OriginalName.Should().Be(fileName);

            files.Validate();
        }

        public void CalculateNewLocation()
        {
            var config = new RenameConfig();
            var date = DateTime.Now;
            string fileName = "abcdef.txt";
            string fileFullName = Path.Combine(tempFolderPath, fileName);

            var fi = files.Add(tempFolderPath, fileName, tempFolderPath, fileName, date, date, date);
            files.CreateFiles();
            fi.Refresh();

            var ef = new ExifFile(fi);
            ef.Should().NotBeNull();
            ef.NewLocation.Should().BeNull();
            ef.NewLocationSpecified.Should().BeFalse();


            config.FilenamePattern = "PRE_{name:0,2}_{name:2,2}_post";
            config.FilenameCase = CharacterCasing.unchanged;
            config.ExtensionCase = CharacterCasing.unchanged;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, "PRE_ab_cd_post.txt"));

            config.FilenamePattern = "PRE_{name:0,2}_{name:2,2}_post";
            config.FilenameCase = CharacterCasing.upper;
            config.ExtensionCase = CharacterCasing.lower;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, "PRE_AB_CD_POST.txt"));

            config.FilenamePattern = "PRE_{name:0,2}_{name:2,2}_post";
            config.FilenameCase = CharacterCasing.lower;
            config.ExtensionCase = CharacterCasing.upper;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, "pre_ab_cd_post.TXT"));


            config.FilenamePattern = "PRE_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post";
            config.FilenameCase = CharacterCasing.unchanged;
            config.ExtensionCase = CharacterCasing.unchanged;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, $"PRE_{date:yyyy}_{date:MM}_{date:dd}_post.txt"));

            config.FilenamePattern = "PRE_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post";
            config.FilenameCase = CharacterCasing.upper;
            config.ExtensionCase = CharacterCasing.lower;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, $"PRE_{date:yyyy}_{date:MM}_{date:dd}_POST.txt"));

            config.FilenamePattern = "PRE_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post";
            config.FilenameCase = CharacterCasing.lower;
            config.ExtensionCase = CharacterCasing.upper;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, $"pre_{date:yyyy}_{date:MM}_{date:dd}_post.TXT"));


            config.FilenamePattern = "PRE_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post";
            config.FilenameCase = CharacterCasing.unchanged;
            config.ExtensionCase = CharacterCasing.unchanged;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, $"PRE_{date:yyyy}_{date:MM}_{date:dd}_post.txt"));

            config.FilenamePattern = "PRE_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post";
            config.FilenameCase = CharacterCasing.upper;
            config.ExtensionCase = CharacterCasing.lower;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, $"PRE_{date:yyyy}_{date:MM}_{date:dd}_POST.txt"));

            config.FilenamePattern = "PRE_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post";
            config.FilenameCase = CharacterCasing.lower;
            config.ExtensionCase = CharacterCasing.upper;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, $"pre_{date:yyyy}_{date:MM}_{date:dd}_post.TXT"));


            config.FilenamePattern = "PRE_{now:yyyy}_{now:MM}_{now:dd}_post";
            config.FilenameCase = CharacterCasing.unchanged;
            config.ExtensionCase = CharacterCasing.unchanged;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, $"PRE_{date:yyyy}_{date:MM}_{date:dd}_post.txt"));

            config.FilenamePattern = "PRE_{now:yyyy}_{now:MM}_{now:dd}_post";
            config.FilenameCase = CharacterCasing.upper;
            config.ExtensionCase = CharacterCasing.lower;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, $"PRE_{date:yyyy}_{date:MM}_{date:dd}_POST.txt"));

            config.FilenamePattern = "PRE_{now:yyyy}_{now:MM}_{now:dd}_post";
            config.FilenameCase = CharacterCasing.lower;
            config.ExtensionCase = CharacterCasing.upper;
            ef.CalculateNewLocation(config);
            ef.NewLocation.Should().Be(Path.Combine(tempFolderPath, $"pre_{date:yyyy}_{date:MM}_{date:dd}_post.TXT"));
        }

        [TestCase("abcdef.txt", true)]
        [TestCase("abcdef.txt", null, typeof(ArgumentNullException))]
        [TestCase("xxxxxxxxxxxx", true, typeof(FileNotFoundException))]
        public void CalculateNewLocationValidationTests(string fileName, bool? validConfig, Type ex = null)
        {
            files.Add(tempFolderPath, "abcdef.txt");
            files.CreateFiles();

            ExifFile ef = new ExifFile(new FileInfo(Path.Combine(tempFolderPath, fileName)));

            var config = new RenameConfig();
            config.FilenamePattern = "1";
            if (validConfig == null)
                config = null;

            if (ex != null)
                Assert.Throws(ex, delegate { ef.CalculateNewLocation(config); });
            else
                Assert.That(ef.CalculateNewLocation(config), Is.Not.Null);
        }

    }
}