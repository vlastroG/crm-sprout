namespace Consulting.Models {
    public class AuthResponse {
        public string Id { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public int ExpiresInSec { get; set; } = 0;
    }
}
