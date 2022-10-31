using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using FDR.Tools.Library;

namespace FDR
{
    public enum Operation
    {
        Help,
        Import,
        Hash,
        Verify,
        Diff,
        Cleanup,
        Rename,
        Resize
    }

    public class Program
    {
        private const ConsoleColor titleColor = ConsoleColor.White;
        private static Operation operation = Operation.Help;

        public static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
            bool verbose = false;
            bool auto = false;
            bool force = false;
            string folder = "";
            string file = "";
            string reference = "";
            string config = "";
            string function = "";

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-import":
                        operation = Operation.Import;
                        GetParam(ref folder, nameof(folder), true);
                        break;

                    case "-hash":
                        operation = Operation.Hash;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case "-rehash":
                        operation = Operation.Hash;
                        force = true;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case "-verify":
                        operation = Operation.Verify;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case "-diff":
                        operation = Operation.Diff;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case "-cleanup":
                        operation = Operation.Cleanup;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case "-verbose":
                        verbose = true;
                        break;

                    case "-auto":
                        auto = true;
                        break;

                    case "-file":
                        GetParam(ref file, nameof(file));
                        break;

                    case "-reference":
                        GetParam(ref reference, nameof(reference));
                        break;

                    case "-rename":
                        operation = Operation.Rename;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case "-resize":
                        operation = Operation.Resize;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case "-config":
                        GetParam(ref config, nameof(config));
                        break;

                    case "-help":
                        operation = Operation.Help;
                        GetParam(ref function, nameof(function), true);
                        break;
                }

                void GetParam(ref string param, string paramname, bool optional = false)
                {
                    if (args.Length <= i + 1 || !args[i + 1].StartsWith("-"))
                    {
                        if (!optional)
                        {
                            Common.Msg($"FDR Tools {version} - ERROR", titleColor);
                            Common.Msg("");
                            Common.Msg($"Missing parameter: {paramname}!", ConsoleColor.Red);
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                                Common.Msg("See help for details: fdr.exe -help");
                            else
                                Common.Msg("See help for details: dotnet FDR.dll -help");
                            Environment.Exit(-1);
                        }
                    }
                    else
                    {
                        param = args[i + 1];
                        i++;
                    }
                }
            }

            using (ConsoleTraceListener consoleTracer = new ConsoleTraceListener())
            {
                AppConfig appConfig;
                if (verbose) Trace.Listeners.Add(consoleTracer);
                try
                {
                    switch (operation)
                    {
                        case Operation.Import:
                            Common.Msg($"FDR Tools {version} - Import", titleColor);
                            appConfig = LoadAppConfig();
                            if (appConfig.ImportConfigs == null)
                            {
                                Common.Msg("There are no import configurations!", ConsoleColor.Red);
                                return;
                            }
                            Import.ImportWizard(appConfig.ImportConfigs, string.IsNullOrWhiteSpace(folder) ? null : new DirectoryInfo(Path.GetFullPath(folder)), auto);
                            break;

                        case Operation.Hash:
                            Common.Msg($"FDR Tools {version} - Hash", titleColor);
                            if (!Common.IsFolderValid(folder)) return;
                            Verify.HashFolder(new DirectoryInfo(Path.GetFullPath(folder)), force);
                            break;

                        case Operation.Verify:
                            Common.Msg($"FDR Tools {version} - Verify", titleColor);
                            if (!Common.IsFolderValid(folder)) return;
                            Verify.VerifyFolder(new DirectoryInfo(Path.GetFullPath(folder)));
                            break;

                        case Operation.Diff:
                            Common.Msg($"FDR Tools {version} - Diff", titleColor);
                            if (!Common.IsFolderValid(folder)) return;
                            if (!Common.IsFolderValid(reference)) return;
                            Verify.DiffFolder(new DirectoryInfo(Path.GetFullPath(folder)), new DirectoryInfo(Path.GetFullPath(reference)));
                            break;

                        case Operation.Cleanup:
                            Common.Msg($"FDR Tools {version} - Cleanup", titleColor);
                            if (!Common.IsFolderValid(folder)) return;
                            Raw.CleanupFolder(new DirectoryInfo(Path.GetFullPath(folder)));
                            break;

                        case Operation.Rename:
                            Common.Msg($"FDR Tools {version} - Rename", titleColor);
                            if (!Common.IsFolderValid(folder)) return;
                            if (string.IsNullOrWhiteSpace(config))
                            {
                                Common.Msg("Rename configuration is not defined!", ConsoleColor.Red);
                                return;
                            }
                            appConfig = LoadAppConfig();
                            RenameConfig? renameConfig;
                            if (!appConfig.RenameConfigs.TryGetValue(config, out renameConfig))
                            {
                                Common.Msg("Given rename configuration does not exist!", ConsoleColor.Red);
                                return;
                            }
                            Rename.RenameFilesInFolder(new DirectoryInfo(Path.GetFullPath(folder)), renameConfig);
                            break;

                        case Operation.Resize:
                            Common.Msg($"FDR Tools {version} - Resize", titleColor);
                            if (!Common.IsFolderValid(folder)) return;
                            if (string.IsNullOrWhiteSpace(config))
                            {
                                Common.Msg("Resize configuration is not defined!", ConsoleColor.Red);
                                return;
                            }
                            appConfig = LoadAppConfig();
                            ResizeConfig? resizeConfig;
                            if (!appConfig.ResizeConfigs.TryGetValue(config, out resizeConfig))
                            {
                                Common.Msg("Given resize configuration does not exist!", ConsoleColor.Red);
                                return;
                            }
                            Resize.ResizeFilesInFolder(new DirectoryInfo(Path.GetFullPath(folder)), resizeConfig);
                            break;

                        default:
                            DisplayHelp(version, function);
                            break;
                    }
                }
                finally
                {
                    Trace.Flush();
                    Trace.Listeners.Remove(consoleTracer);
                    consoleTracer.Close();
                }
            }
        }

        private static void DisplayHelp(string version, string? function = null)
        {
            var func = function?.Trim().ToLower();
            Common.Msg($"FDR Tools {version} - Help" + (string.IsNullOrWhiteSpace(func) ? "" : ": " + func), titleColor);

            if (string.IsNullOrWhiteSpace(func))
            {
                Common.Msg("Usage:");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Common.Msg("    fdr.exe [options]");
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    Common.Msg("    dotnet FDR.dll [options]");
                else
                {
                    Common.Msg("    dotnet FDR.dll [options]");
                    Common.Msg("    Unsupported OS version!", ConsoleColor.Red);
                }
                Common.Msg("");
                Common.Msg("Where options can be:");
                Common.Msg("    -help [function]     Generic help (this screen) or optionally help about a given function");
                Common.Msg("    -import [<folder>]   Import memory card content or optionally the content of a folder");
                Common.Msg("    -hash <folder>       Create hash of files in a folder");
                Common.Msg("    -rehash <folder>     Recreate hashes of all files in a folder");
                Common.Msg("    -verify <folder>     Verify the files in a folder against their saved hash");
                Common.Msg("    -diff <folder>       Compare the files of a folder to a reference one");
                Common.Msg("    -reference <folder>  Reference folder for the diff function");
                Common.Msg("    -cleanup <folder>    Delete unnecessary raw, hash and err files");
                Common.Msg("    -rename <folder>     Rename image files based on a given configuration");
                Common.Msg("    -resize <folder>     Resize image files based on a given configuration");
                Common.Msg("    -config <config>     Named configuration for some functions like renaming and resizing");
                Common.Msg("    -auto                Start the import automatically");
                Common.Msg("    -verbose             More detailed output");
            }
            else
            {
                if (func =="import")
                    Import.ShowImportHelp();
                else if (func == "hash")
                    Common.Msg("\nCreates hidden hash files of files in the folder given after the -hash option for which there were none yet.");
                else if (func == "rehash")
                    Common.Msg("\nRecreates the hidden hash files of all files in the folder given after the -rehash option whether they existed or not.");
                else if (func == "verify")
                    Common.Msg("\nVerifies the files in the folder given after the -verify option against their saved hidden hash and creates error files whenever there are differences.");
                else if (func == "diff")
                    Common.Msg("\nCompares the files of the folder given after the -diff option to a reference folder given after the -reference option.");
                else if (func == "cleanup")
                    Common.Msg("\nDeletes unnecessary raw, hash and err files.");
                else if (func == "rename")
                    Rename.ShowRenameHelp();
                else if (func == "move")
                    Import.ShowMoveHelp();
                else if (func == "resize")
                    Resize.ShowResizeHelp();
                else
                    Common.Msg("Invalid function: " + func, ConsoleColor.Red);
            }
        }

        private static AppConfig LoadAppConfig()
        {
            var appPath = Assembly.GetExecutingAssembly().Location;
            var configPath = Path.Combine(Path.GetDirectoryName(appPath)!, "appsettings.json");
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(configPath, Encoding.UTF8));
            if (appConfig == null) appConfig = new AppConfig();
            appConfig.Validate();
            return appConfig;
        }
    }
}
