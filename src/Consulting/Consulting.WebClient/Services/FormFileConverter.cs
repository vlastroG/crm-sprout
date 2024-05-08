namespace Consulting.WebClient.Services {
    public class FormFileConverter {
        public FormFileConverter() {

        }


        public async Task<byte[]> ConvertToByteArray(IFormFile? formFile) {
            if(formFile is not null && formFile.Length > 0) {
                using(var ms = new MemoryStream()) {
                    await formFile.CopyToAsync(ms);
                    return ms.ToArray();
                }
            } else {
                return Array.Empty<byte>();
            }
        }
    }
}
