using System.Windows;

namespace Consulting.Desktop.Views {
    public partial class BlogPostCreationWindow : Window {
        public BlogPostCreationWindow() {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
