using System.Windows;

namespace Consulting.Desktop.Views {
    public partial class BlogPostEditingWindow : Window {
        public BlogPostEditingWindow() {
            Owner = System.Windows.Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
