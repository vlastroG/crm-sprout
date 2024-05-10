using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Consulting.Desktop.Converters {
    [ValueConversion(typeof(byte[]), typeof(BitmapImage))]
    public class ByteArrayToImageConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            byte[]? bytes = value as byte[];
            if(bytes != null && bytes.Length > 0) {
                try {
                    using(var ms = new System.IO.MemoryStream(bytes)) {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;
                        image.EndInit();
                        return image;
                    }
                } catch(NotSupportedException) {
                    return new BitmapImage();
                }
            } else {
                return new BitmapImage();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotSupportedException();
        }
    }
}
