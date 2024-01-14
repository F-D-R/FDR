using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class RenameModel : PageModel
    {
        private readonly IConfiguration Configuration;
        private readonly List<ProcessInfo> Processes;
        private CancellationTokenSource cts = new();

        public RenameModel(IConfiguration configuration, List<ProcessInfo> processes)
        {
            Configuration = configuration;
            Processes = processes;
            //Processes.Add(new (Operation.Rename));
        }

        public AppConfig? AppConfig { get; set; }

        [BindProperty]
        public string? ConfigKey { get; set; }

        [BindProperty]
        public RenameConfig RenameConfig { get; set; } = new RenameConfig();

        [Display(Name = "Verbose output")]
        [BindProperty]
        public bool Verbose { get; set; } = false;

        [Required(ErrorMessage = "Folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "ValidateFolder", ErrorMessage = "Folder doesn't exist!")]
        [BindProperty]
        public string? Folder { get; set; }

        //public StreamReader Output { get; } = new StreamReader(Console.OpenStandardOutput());

        public void OnGet()
        {
            Console.WriteLine("RenameModel.OnGet...");
            try
            {
                AppConfig = AppConfig.Load();
            }
            catch { }



            ConfigKey = Request.Query["config"];
            if (AppConfig != null && !string.IsNullOrWhiteSpace(ConfigKey))
            {
                RenameConfig = AppConfig.RenameConfigs.Where(d => string.Equals(d.Key, ConfigKey, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Value.Clone();
            }
            if (RenameConfig == null) { RenameConfig = new RenameConfig(); }
        }

        public JsonResult OnPostValidateFolder()
        {
            var folder = new DirectoryInfo(Folder!);
            return new JsonResult(folder.Exists);
        }

        public void SelectFolder()
        {
            Console.WriteLine("SelectFolder...");
        }
        public void OnPostSelectFolder()
        {
            Console.WriteLine("OnPostSelectFolder...");
        }
        public void OnGetSelectFolder()
        {
            Console.WriteLine("OnGetSelectFolder...");
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("RenameModel.OnPost...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("RenameModel.OnPost ModelState is NOT valid!!!");
                return Page();
            }

            try { RenameConfig.Validate(); }
            catch (Exception ex)
            {
                ModelState.AddModelError("RenameConfig", ex.Message ?? "Invalid rename configuration!");
                return Page();
            }

            var folder = new DirectoryInfo(Folder!);
            if (!folder.Exists)
            {
                ModelState.AddModelError("Folder", "Folder doesn't exist!");
                return Page();
            }

            Console.WriteLine("RenameModel.OnPost ModelState is valid...");

            Console.WriteLine("");
            Console.WriteLine($"Selected config: {ConfigKey}");
            Console.WriteLine($"File filter: {RenameConfig.FileFilter}");
            Console.WriteLine($"Filename pattern: {RenameConfig.FilenamePattern}");
            Console.WriteLine($"Filename case: {RenameConfig.FilenameCase}");
            Console.WriteLine($"Extension case: {RenameConfig.ExtensionCase}");
            Console.WriteLine($"Additional files: {RenameConfig.AdditionalFiles}");
            Console.WriteLine($"Recursive: {RenameConfig.Recursive}");
            Console.WriteLine($"Stop on error: {RenameConfig.StopOnError}");
            Console.WriteLine($"Folder: {Folder}");
            Console.WriteLine($"Verbose output: {Verbose}");

            //var sourceFilePath = @"D:\GIT\FDR\FDR.Web\bin\Release\net7.0\appsettings.json";
            //var destFilePath = @"D:\GIT\FDR\FDR.Web\bin\Release\net7.0\appsettings_copy.json";
            //await Common.CopyFileAsync(sourceFilePath, destFilePath);

            cts.Token.ThrowIfCancellationRequested();
            _ = DummyProcess.Start(cts.Token);
            Processes.Add(new(Operation.Rename, cts));

            return RedirectToPage("./Output");
        }
    }
}
