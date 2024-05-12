using System.Windows;

namespace Consulting.Desktop.Views {
    public partial class ConsultingTaskDetailsWindow : Window {
        public ConsultingTaskDetailsWindow() {
            Owner = System.Windows.Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
