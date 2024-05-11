using System.Windows;

namespace Consulting.Desktop.Views {
    public partial class CompanyServiceDetailsWindow : Window {
        public CompanyServiceDetailsWindow() {
            Owner = System.Windows.Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
