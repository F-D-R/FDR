using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class ImportModel : PageModel
    {
        private readonly Processes Processes;

        public ImportModel(Processes processes)
        {
            Processes = processes;
        }

        public AppConfig? AppConfig { get; set; }

        [BindProperty]
        public string? ConfigKey { get; set; }

        [BindProperty]
        public ImportConfig ImportConfig { get; set; } = new ImportConfig();

        [DisplayName("Overwrite existing folders")]
        [BindProperty]
        public bool Force { get; set; } = false;

        [DisplayName("Run no actions")]
        [BindProperty]
        public bool NoActions { get; set; } = false;

        [DisplayName("Verbose output")]
        [BindProperty]
        public bool Verbose { get; set; } = false;

        [Required(ErrorMessage = "Folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "ValidateFolder", ErrorMessage = "Folder doesn't exist!")]
        [BindProperty]
        public string? Folder { get; set; }

        public void OnGet()
        {
            Console.WriteLine("ImportModel.OnGet...");
            try
            {
                AppConfig = AppConfig.Load();
            }
            catch { }

            ConfigKey = Request.Query["config"];
            if (AppConfig != null && !string.IsNullOrWhiteSpace(ConfigKey))
            {
                ImportConfig = AppConfig.ImportConfigs.Where(d => string.Equals(d.Key, ConfigKey, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Value.Clone();
            }
            ImportConfig ??= new ImportConfig();
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
            Console.WriteLine("ImportModel.OnPost...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ImportModel.OnPost ModelState is NOT valid!!!");
                return Page();
            }

            try { ImportConfig.Validate(); }
            catch (Exception ex)
            {
                ModelState.AddModelError("ImportConfig", ex.Message ?? "Invalid import configuration!");
                return Page();
            }

            var folder = new DirectoryInfo(Folder!);
            if (!folder.Exists)
            {
                ModelState.AddModelError("Folder", "Folder doesn't exist!");
                return Page();
            }

            Console.WriteLine("ImportModel.OnPost ModelState is valid...");

            Console.WriteLine("");
            Console.WriteLine($"Selected config: {ConfigKey}");
            Console.WriteLine($"Name: {ImportConfig.Name}");
            Console.WriteLine($"File filter: {ImportConfig.FileFilter}");
            Console.WriteLine($"Dest root: {ImportConfig.DestRoot}");
            Console.WriteLine($"Dest structure: {ImportConfig.DestStructure}");
            Console.WriteLine($"Date format: {ImportConfig.DateFormat}");
            Console.WriteLine($"Folder: {Folder}");
            Console.WriteLine($"Force: {Force}");
            Console.WriteLine($"Run no actions: {NoActions}");
            Console.WriteLine($"Verbose output: {Verbose}");

            _ = Processes.Start(operation: Operation.Import, folder: Folder, verbose: Verbose, tmpConfig: ImportConfig, force: Force, noactions: NoActions, auto: true);

            return RedirectToPage("./Output");
        }
    }
}
