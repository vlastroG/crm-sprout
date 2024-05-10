using System.Windows;

using Consulting.Models;

namespace Consulting.Desktop.Views {
    public partial class ConsultingTaskDetailsWindow : Window {
        public ConsultingTaskDetailsWindow(ConsultingTask consultingTask) {
            DataContext = consultingTask ?? throw new ArgumentNullException(nameof(consultingTask));
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
