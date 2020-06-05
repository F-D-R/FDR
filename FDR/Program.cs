using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using FDR.Tools.Library;
using System.Threading;
//using MetadataExtractor;
//using ExifLibrary;
//using ExifLib;

namespace FDR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            bool help = false;
            bool verbose = false;
            bool import = false;
            bool auto = false;

            //var services = ConfigureServices();
            //var serviceProvider = services.BuildServiceProvider();

            foreach (var arg in args)
            {
                switch (arg.ToLower())
                {
                    case "-h":
                    case "-help":
                    case "-?":
                    case "/?":
                        help = true;
                        break;

                    case "-v":
                    case "-verbose":
                        verbose = true;
                        break;

                    case "-i":
                    case "-import":
                        import = true;
                        break;

                    case "-a":
                    case "-auto":
                        auto = true;
                        break;
                }
            }

            if (help)
            {
                if (import)
                {
                    Msg($"FDR Tools {version} - Import Help", ConsoleColor.Yellow);
                }
                else
                {
                    Msg($"FDR Tools {version} - Help", ConsoleColor.Yellow);
                    Msg("Usage:");
                    Msg("    fdr.exe [options]");
                    Msg("");
                    Msg("Where options can be:");
                    Msg("    -h    -help       Help (this screen)");
                    Msg("    -v    -verbose    Detailed output");
                    Msg("    -i    -import     Import memory card content");
                    Msg("    -a    -auto       Automatic start");
                }
                return;
            }
            else
            {
                Msg($"FDR Tools {version}", ConsoleColor.Yellow);
            }

            using (ConsoleTraceListener consoleTracer = new ConsoleTraceListener())
            {
                if (verbose) Trace.Listeners.Add(consoleTracer);

                //var renameConfig = new RenameConfig();
                //renameConfig.FilenamePattern = "FDR_{cdate:yyyyMMdd}";
                //renameConfig.AdditionalFileTypes = new string[] { ".else" };

                var renameConfig = JsonConvert.DeserializeObject<RenameConfig>(@"{""Filter"":""*.txt"",""FilenamePattern"":""FDR_{cdate:yyyyMMdd}"",""FilenameCase"":""unchanged"",""ExtensionCase"":""upper"",""AdditionalFileTypes"":["".else""]}");
                //Rename.RenameFile(new FileInfo(@"C:\temp\0\test.txt"), 1, renameConfigv);



                //Trace.WriteLine("MetaData:");
                //Trace.Indent();
                //var mds = MetadataExtractor.ImageMetadataReader.ReadMetadata(@"C:\temp\0\Cameras\EOS 6D\EOS_DIGITAL\DCIM\100CANON\IMG_1880.CR2");
                //foreach (var md in mds)
                //{
                //    Trace.WriteLine($"{md.Name}: ");
                //    var tags = md.Tags;
                //    Trace.Indent();
                //    foreach (var tmptag in tags)
                //    {
                //        Trace.WriteLine($"{tmptag.Name}");
                //    }
                //    Trace.Unindent();
                //}
                //Trace.Unindent();

                //Trace.WriteLine($"Exif IFD0 - Date/Time: {mds.Where(md => md.Name == "Exif IFD0").FirstOrDefault().Tags.Where(t => t.Name == "Date/Time").FirstOrDefault().Description}");
                ////Trace.WriteLine($"{mds.Where(md => md.Name == "Exif IFD0").FirstOrDefault().Tags.Where(t => t.Name == "Date/Time").FirstOrDefault().Description}");
                //var tag = mds.Where(md => md.Name == "Exif IFD0").FirstOrDefault().Tags.Where(t => t.Name == "Date/Time").FirstOrDefault();
                //Trace.WriteLine($"tag.Type: {tag.Type} {MetadataExtractor.Formats.Exif.ExifIfd0Directory.TagDateTime} tag.Description: {tag.Description}");

                //MetadataExtractor. ExifSubIFDDirectory directory = metadata.getFirstDirectoryOfType(ExifSubIFDDirectory.class);

                //MetadataExtractor.Directory directory = mds.getFirstDirectoryOfType(MetadataExtractor.Formats.Exif.ExifIfd0Directory.TagDateTime ExifSubIFDDirectory.class);
                //var edate = directory.getDate(ExifSubIFDDirectory.TAG_DATETIME_ORIGINAL);

                //DateTime sdate;
                //DateTime.TryParse(tag.Description, out sdate);
                //DateTime.Parse(tag.Description, new date)
                //Trace.WriteLine($"sdate: {sdate}");


                //using (var er = new ExifLib.ExifReader(@"C:\temp\0\Cameras\EOS 6D\EOS_DIGITAL\DCIM\100CANON\IMG_1880.tif"))
                //{
                //    //er.GetTagValue<>

                //    DateTime datePictureTaken;
                //    if (er.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out datePictureTaken))
                //    {
                //        // Do whatever is required with the extracted information
                //        Trace.WriteLine($"sdate: {datePictureTaken}");
                //    }

                //}


                //var ticks = DateTime.Now.Ticks;
                //for (int i = 0; i < 100; i++)
                //{
                //    var file = ImageFile.FromFile(@"C:\temp\0\Cameras\EOS 6D\EOS_DIGITAL\DCIM\100CANON\IMG_1880.CR2");
                //}
                //Trace.WriteLine($"Elapsed: {(DateTime.Now.Ticks - ticks) / 10000} ms");


                // the type of the ISO speed rating tag value is unsigned short
                // see documentation for tag data types
                //var isoTag = file.Properties.Get<ExifUShort>(ExifTag.ISOSpeedRatings);
                //Trace.WriteLine($"isoTag: {isoTag}");
                //var dateTag = file.Properties.Get<ExifDateTime>(ExifTag.DateTime);
                //Trace.WriteLine($"dateTag: {dateTag}");

                // the flash tag's value is an enum
                //var flashTag = data.Properties.Get<ExifEnumProperty<Flash>>(ExifTag.Flash);

                // GPS latitude is a custom type with three rational values
                // representing degrees/minutes/seconds of the latitude 
                //var latTag = data.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLatitude);

                //using (var er = new ExifLib.ExifReader(@"C:\temp\0\Cameras\EOS 6D\EOS_DIGITAL\DCIM\100CANON\IMG_1880.CR2"))
                //{
                //    DateTime datePictureTaken;
                //    if (er.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out datePictureTaken))
                //    {
                //        Trace.WriteLine($"sdate: {datePictureTaken}");
                //    }
                //}




                if (import)
                {
                    // FDR
                    //var importConfig = JsonConvert.DeserializeObject<ImportConfig>(@"{""DestRoot"":""D:\\FDR\\"",""DestStructure"":""year_date"",""DateFormat"":""yyMMdd"",""RenameConfigs"":[{""Filter"":""*.CR2"",""FileNamePattern"":""{cdate:yyMMdd}_{counter:3}""}],""MoveConfigs"":[{""Filter"":""*.CR2"",""RelativeFolder"":""RAW""}]}");
                    // MSE EOS
                    //var importConfig = JsonConvert.DeserializeObject<ImportConfig>(@"{""DestRoot"":""D:\\MSE\\EOS_Képek\\"",""DestStructure"":""year_date"",""DateFormat"":""yyMMdd"",""RenameConfigs"":[{""Filter"":""*.CR2"",""FileNamePattern"":""MSE_{cdate:yyMMdd}_{counter:3}""}],""MoveConfigs"":[{""Filter"":""*.CR2"",""RelativeFolder"":""RAW""}]}");
                    // MSE Képek
                    //var importConfig = JsonConvert.DeserializeObject<ImportConfig>(@"{""DestRoot"":""D:\\MSE\\Képek\\"",""DestStructure"":""year_date"",""DateFormat"":""yyyy_MM_dd"",""MoveConfigs"":[{""Filter"":""*.CR2"",""RelativeFolder"":""RAW""}]}");
                    //Import.ImportWizard(importConfig, auto);

                    var appPath = Assembly.GetExecutingAssembly().Location;
                    var configPath = Path.Combine(Path.GetDirectoryName(appPath), "appsettings.json");
                    var appConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(configPath, Encoding.UTF8));

                    Import.ImportWizard(appConfig.ImportConfigs, auto);
                }

                Trace.Flush();
                Trace.Listeners.Remove(consoleTracer);
                consoleTracer.Close();
            }
        }

        private static void Msg(string msg, ConsoleColor color = ConsoleColor.White, bool newline = true)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = color;
            if (newline)
                Console.WriteLine(msg);
            else
                Console.Write(msg);
            Console.ResetColor();
        }

        //private static IServiceCollection ConfigureServices()
        //{
        //    IServiceCollection services = new ServiceCollection();

        //    //var config = LoadConfiguration();
        //    //services.AddSingleton(config);

        //    // required to run the application
        //    //services.AddTransient<App>();

        //    var myLogger = new App();
        //    services.AddSingleton(myLogger);

        //    return services;
        //}
    }


    //public interface IApp
    //{
    //    void Msg(string msg, ConsoleColor color = ConsoleColor.White);
    //}

    //public class App : IApp
    //{
    //    //private readonly ILogger<MyLogger> _logger;

    //    //public MyLogger(ILogger<MyLogger> logger)
    //    //{
    //    //    _logger = logger;
    //    //}

    //    public void Msg(string msg, ConsoleColor color = ConsoleColor.White)
    //    {
    //        Console.BackgroundColor = ConsoleColor.Black;
    //        Console.ForegroundColor = color;
    //        Console.WriteLine(msg);
    //        Console.ResetColor();
    //    }
    //}
}
