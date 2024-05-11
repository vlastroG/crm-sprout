using System.Windows;

namespace Consulting.Desktop.Services {
    public class MessageBoxService {
        public MessageBoxService() {

        }


        public void ShowInfo(string message, string title) {
            System.Windows.MessageBox.Show(
                System.Windows.Application.Current.MainWindow,
                message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        public void ShowError(string message, string title) {
            System.Windows.MessageBox.Show(
                System.Windows.Application.Current.MainWindow,
                message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public void ShowWarning(string message, string title) {
            System.Windows.MessageBox.Show(
                System.Windows.Application.Current.MainWindow,
                message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        public bool ConfirmWarning(string message, string title) {
            return System.Windows.MessageBox.Show(
                System.Windows.Application.Current.MainWindow,
                message,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }
    }
}
