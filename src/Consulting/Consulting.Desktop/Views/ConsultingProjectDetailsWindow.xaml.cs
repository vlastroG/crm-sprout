using System.Windows;

using Consulting.Models;

namespace Consulting.Desktop.Views {
    public partial class ConsultingProjectDetailsWindow : Window {
        public ConsultingProjectDetailsWindow(ConsultingProject consultingProject) {
            DataContext = consultingProject ?? throw new ArgumentNullException(nameof(consultingProject));
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
