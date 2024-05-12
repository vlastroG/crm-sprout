using System.IO;

namespace Consulting.Desktop.Services {
    public class ImageProvider {
        public ImageProvider() {

        }


        /// <exception cref="OperationCanceledException"></exception>
        public FileInfo GetImage() {
            OpenFileDialog fileDialog = new() {
                Title = "Выберите изображение",
                InitialDirectory = Environment.SpecialFolder.Desktop.ToString(),
                Filter = "Jpeg images|*.jpg;*jpeg"
            };

            if(fileDialog.ShowDialog() == DialogResult.OK) {
                return new FileInfo(fileDialog.FileName);
            } else {
                throw new OperationCanceledException();
            }
        }
    }
}
