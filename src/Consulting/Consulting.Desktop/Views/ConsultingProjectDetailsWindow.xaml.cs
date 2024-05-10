using System.Windows;

namespace Consulting.Desktop.Views {
    public partial class ConsultingProjectDetailsWindow : Window {
        public ConsultingProjectDetailsWindow() {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
