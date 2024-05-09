namespace Consulting.Models.Exceptions {
    public class AccessDeniedException : Exception {
        public AccessDeniedException() : base() {

        }

        public AccessDeniedException(string msg) : base(msg) { }
    }
}
