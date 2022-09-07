using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FDR.UI.Models;
using System.Reactive;
using ReactiveUI;

namespace FDR.UI.ViewModels
{
    public class ImageBrowserViewModel : ViewModelBase
    {
        private ImageDirectory imageDirectory;
        private List<object> fileSystemViewModels = new List<object>();

        internal ImageBrowserViewModel(ImageDirectory imgDir)
        {
            imageDirectory = imgDir;
            _ = imageDirectory.LoadAsync(new CancellationToken());
            fileSystemViewModels = new List<object>();
        }

        public List<object> FileSystemViewModels
        {
            get
            {
                return fileSystemViewModels;
            }
         }

        public string DirectoryPath => @"F:\FDR\2022\220603_Pacsmag";
    }
}
