using System.Windows;

using Consulting.Desktop.ViewModels;

namespace Consulting.Desktop.Views {
    public partial class BlogPostCreationWindow : Window {
        public BlogPostCreationWindow(BlogPostCreationViewModel viewModel) {
            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
