using System.Windows;

namespace Consulting.Desktop.Views {
    public partial class ConsultingProjectCreationWindow : Window {
        public ConsultingProjectCreationWindow() {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
