using System.Windows;

namespace Consulting.Desktop.Views {
    public partial class BlogPostDetailsWindow : Window {
        public BlogPostDetailsWindow() {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
