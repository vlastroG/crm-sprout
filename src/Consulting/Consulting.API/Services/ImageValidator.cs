using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace Consulting.API.Services {
    public class ImageValidator {
        public ImageValidator() {

        }


        /// <summary>
        /// Validate image size, height and width
        /// </summary>
        /// <param name="imgBytes">Raster image</param>
        /// <param name="requiredWidth"></param>
        /// <param name="requiredHeight"></param>
        /// <param name="maxSize">Max size of image in bytes</param>
        /// <param name="tolerance">Tolerance of height and width</param>
        /// <returns></returns>
        public bool ImageIsValid(
            byte[] imgBytes,
            int requiredWidth,
            int requiredHeight,
            int maxSize = 128 * 1024,
            double tolerance = 0.1) {

            if(imgBytes is null || imgBytes.Length == 0) { return true; }

            if(imgBytes.Length <= maxSize) {
                using(var ms = new MemoryStream(imgBytes)) {
                    IImage image = PlatformImage.FromStream(ms, ImageFormat.Jpeg);
                    var imgWidth = image.Height; //don't know why
                    var imgHeight = image.Width; //don't know why
                    return Math.Round(Math.Abs(imgWidth - requiredWidth), 5) <= tolerance
                        && Math.Round(Math.Abs(imgHeight - requiredHeight), 5) <= tolerance;
                }
            } else {
                return false;
            }
        }
    }
}
