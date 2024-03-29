﻿using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class CommonTest
    {
        [Test]
        public void IsFolderValidTests()
        {
            string tempFolderPath = Path.GetTempPath();
            tempFolderPath.Should().NotBeNullOrWhiteSpace();
            Common.IsFolderValid(tempFolderPath).Should().BeTrue();
            Common.IsFolderValid(Path.Combine(tempFolderPath, Guid.NewGuid().ToString())).Should().BeFalse();
            Common.IsFolderValid("").Should().BeFalse();
        }

        [TestCase(".CR3", false)]
        [TestCase(".CR2", false)]
        [TestCase(".CRW", false)]
        [TestCase(".DNG", false)]
        [TestCase(".JPG", true)]
        [TestCase(".JPEG", true)]
        [TestCase(".TIF", true)]
        [TestCase(".TIFF", true)]
        [TestCase(".EXE", false)]
        [TestCase(".MD5", false)]
        [TestCase(".AVI", false)]
        [TestCase(".MP4", false)]
        [TestCase(".MOV", false)]
        public void IsImageFileTests(string file, bool result)
        {
            Common.IsImageFile(file).Should().Be(result);
        }
    }
}