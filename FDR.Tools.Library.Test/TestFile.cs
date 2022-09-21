using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace FDR.Tools.Library.Test
{
    internal class TestFile
    {
        public TestFile() { Created = DateTime.Now; }

        public bool Keep = true;
        public bool Create = true;
        public DateTime Created;
        public string SourceFolder;
        public string SourceName;
        public string DestFolder;
        public string DestName;

        public string Name { get { return DestName; } }

        public string GetSourcePath() => System.IO.Path.Combine(SourceFolder, SourceName);

        public string GetDestPath() => System.IO.Path.Combine(DestFolder, DestName);

        public string GetPath() => GetDestPath();
    }

    internal class TestFiles : List<TestFile>
    {
        public void Add(string sourceFolder, string sourceName, string destFolder, string destName)
        {
            base.Add(new TestFile() { Keep = true, Create = true, SourceFolder = sourceFolder, SourceName = sourceName, DestFolder = destFolder, DestName = destName });
        }

        public void Add(DateTime created, string sourceFolder, string sourceName, string destFolder, string destName)
        {
            base.Add(new TestFile() { Keep = true, Create = true, Created = created, SourceFolder = sourceFolder, SourceName = sourceName, DestFolder = destFolder, DestName = destName });
        }

        public void Add(string folder, string name, bool keep = true)
        {
            base.Add(new TestFile() { Keep = keep, Create = true, SourceFolder = folder, DestFolder = folder, SourceName = name, DestName = name });
        }

        public void CreateFiles()
        {
            this.Where(f => f.Create).ToList().ForEach(f => { File.WriteAllText(f.GetSourcePath(), ""); File.SetCreationTimeUtc(f.GetSourcePath(), f.Created); File.SetLastWriteTimeUtc(f.GetSourcePath(), f.Created); });
        }
    }
}
