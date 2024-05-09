using System.Windows;

namespace Consulting.Desktop.Services {
    public class MessageBoxService {
        public MessageBoxService() {

        }


        public void ShowInfo(string message, string title) {
            MessageBox.Show(
                Application.Current.MainWindow,
                message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        public void ShowError(string message, string title) {
            MessageBox.Show(
                Application.Current.MainWindow,
                message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public bool ConfirmWarning(string message, string title) {
            return MessageBox.Show(
                Application.Current.MainWindow,
                message,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }
    }
}
