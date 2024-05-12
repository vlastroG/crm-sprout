using System.Windows;

namespace Consulting.Desktop.Views {
    public partial class BlogPostDetailsWindow : Window {
        public BlogPostDetailsWindow() {
            Owner = System.Windows.Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
