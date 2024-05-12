using System.Windows;

using Consulting.Desktop.ViewModels;

namespace Consulting.Desktop.Views {
    public partial class MainWindow : Window {
        public MainWindow(MainWindowViewModel mainWindowViewModel) {
            DataContext = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
            InitializeComponent();
        }
    }
}
