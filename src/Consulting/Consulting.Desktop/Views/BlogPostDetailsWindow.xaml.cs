using System.Windows;

using Consulting.Models;

namespace Consulting.Desktop.Views {
    public partial class BlogPostDetailsWindow : Window {
        public BlogPostDetailsWindow(BlogPost blogPost) {
            DataContext = blogPost ?? throw new ArgumentNullException(nameof(blogPost));
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
