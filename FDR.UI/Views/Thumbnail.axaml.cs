using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FDR.UI.Views
{
    public partial class Thumbnail : UserControl
    {
        public Thumbnail()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
