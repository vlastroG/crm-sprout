using System.Windows;

using Consulting.Models;

namespace Consulting.Desktop.Views {
    public partial class CompanyServiceDetailsWindow : Window {
        public CompanyServiceDetailsWindow(CompanyService companyService) {
            DataContext = companyService ?? throw new ArgumentNullException(nameof(companyService));
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
