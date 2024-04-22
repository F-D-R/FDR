﻿using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.IO;

namespace FDR.Tools.Library
{
    public class ExifFile
    {
        public ExifFile(FileInfo file)
        {
            FileInfo = file;
            OriginalFullName = file.FullName;
            OriginalName = file.Name;
            CreationTimeUtc = file.CreationTimeUtc;
            CreationTime = file.CreationTime;
            LastWriteTimeUtc = file.LastWriteTimeUtc;
            LastWriteTime = file.LastWriteTime;
        }

        public FileInfo FileInfo;

        public string FullName => FileInfo.FullName;

        public readonly string OriginalFullName;

        public string Name => FileInfo.Name;

        public readonly string OriginalName;

        public string Extension => FileInfo.Extension;

        public string? DirectoryName => FileInfo.DirectoryName;

        public DirectoryInfo? Directory => FileInfo.Directory;

        string? _NewLocation;
        public string? NewLocation
        {
            get { return _NewLocation; }
            set
            {
                if (string.Compare(value, FullName, false) != 0)
                    _NewLocation = value;
            }
        }

        public bool NewLocationSpecified => !string.IsNullOrWhiteSpace(NewLocation);

        public readonly DateTime CreationTimeUtc;

        public readonly DateTime CreationTime;

        public readonly DateTime LastWriteTimeUtc;

        public readonly DateTime LastWriteTime;

        DateTime _ExifTime;
        public DateTime ExifTime
        {
            get
            {
                if (!IsExifLoaded)
                {
                    _ExifTime = GetExifDate();
                    _IsExifLoaded = true;
                }
                return _ExifTime;
            }
        }

        public DateOnly ExifDate => DateOnly.FromDateTime(ExifTime);

        private bool _IsExifLoaded = false;
        public bool IsExifLoaded => _IsExifLoaded;

        private DateTime? GetExifDateOnly()
        {
            try
            {
                IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(FullName);
                return Common.GetExifDate(directories);
            }
            catch { }

            return null;
        }

        private DateTime GetExifDate(DateTime defaultDate)
        {
            DateTime? date = GetExifDateOnly();
            return date ?? defaultDate;
        }

        private DateTime GetExifDate()
        {
            return GetExifDate(CreationTime < LastWriteTime ? CreationTime : LastWriteTime);
        }

        public bool IsImageFile()
        {
            return Common.IsImageFile(Name);
        }

        public bool Exists => FileInfo.Exists;

        public void MoveTo(string dest)
        {
            FileInfo.MoveTo(dest);
        }

        public void MoveToNewLocation()
        {
            //if (string.IsNullOrWhiteSpace(NewLocation)) throw new ArgumentNullException(nameof(NewLocation));
            if (NewLocationSpecified)
            {
                FileInfo.MoveTo(NewLocation!);
                NewLocation = null;
            }
        }
    }

}
