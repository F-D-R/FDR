using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FDR.UI
{
    public partial class ImageBrowser : Window
    {
        public ImageBrowser()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
