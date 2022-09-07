using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Reactive;
using ReactiveUI;

namespace FDR.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string message = "Welcome to Avalonia!";

        public string Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }

        public string ButtonText => "Some Button...";

        public void ImageBrowserButton_Click()
        {
            Message = "Image browser button clicked...";
        }

        public void ExitMenu_Click()
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                desktopLifetime.MainWindow.Close();
        }
    }
}
