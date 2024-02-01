using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class RenameModel : PageModel
    {
        private readonly Processes Processes;

        public RenameModel(Processes processes)
        {
            Processes = processes;
        }

        public AppConfig? AppConfig { get; set; }

        [BindProperty]
        public string? ConfigKey { get; set; }

        [BindProperty]
        public RenameConfig RenameConfig { get; set; } = new RenameConfig();

        [DisplayName("Verbose output")]
        [BindProperty]
        public bool Verbose { get; set; } = false;

        [Required(ErrorMessage = "Folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "ValidateFolder", ErrorMessage = "Folder doesn't exist!")]
        [BindProperty]
        public string? Folder { get; set; }

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
            RenameConfig ??= new RenameConfig();
        }

        public JsonResult OnPostValidateFolder()
        {
            var folder = new DirectoryInfo(Folder!);
            return new JsonResult(folder.Exists);
        }

        private FileInfo CreateTmpJpgFile()
        {
            var path = Path.Combine(Path.GetTempPath(), "rename_example.jpg");
            var date = new DateTime(2001, 2, 3, 4, 5, 6);
            using (var image = new Image<Argb32>(8, 8))
            {
                const string EXIF_DATE_FORMAT = "yyyy:MM:dd HH:mm:ss";
                image.Metadata.ExifProfile = new ExifProfile();

                image.Metadata.ExifProfile.SetValue(ExifTag.DateTime, date.ToString(EXIF_DATE_FORMAT));
                image.Metadata.ExifProfile.SetValue(ExifTag.DateTimeOriginal, date.ToString(EXIF_DATE_FORMAT));
                image.Metadata.ExifProfile.SetValue(ExifTag.DateTimeDigitized, date.ToString(EXIF_DATE_FORMAT));
                image.SaveAsJpeg(path);
                System.IO.File.SetCreationTime(path, date);
                System.IO.File.SetLastWriteTime(path, date);
            }
            return new FileInfo(path);
        }

        public IActionResult OnGetCalculateExample(string? pattern)
        {
            Console.WriteLine($"RenameModel.OnGetCalculateExample... {pattern}");

            FileInfo? file = null;
            try
            {
                file = CreateTmpJpgFile();
                if (file == null) return Content("");
                return Content(Rename.EvaluateFileNamePattern(pattern??"", file, 1));
            }
            finally
            {
                if (file != null && file.Exists) file.Delete();
            }
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

            _ = Processes.Start(operation: Operation.Rename, folder: Folder, verbose: Verbose, tmpConfig: RenameConfig);

            return RedirectToPage("./Output");
        }
    }
}
