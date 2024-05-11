using System.Windows;

using Consulting.Desktop.ViewModels;

namespace Consulting.Desktop.Views {
    public partial class ConsultingProjectCreationWindow : Window {
        public ConsultingProjectCreationWindow(ConsultingProjectCreationViewModel viewModel) {
            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            Owner = System.Windows.Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
