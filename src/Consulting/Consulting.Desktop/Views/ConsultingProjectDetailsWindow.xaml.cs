using System.Windows;

namespace Consulting.Desktop.Views {
    public partial class ConsultingProjectDetailsWindow : Window {
        public ConsultingProjectDetailsWindow() {
            Owner = System.Windows.Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
