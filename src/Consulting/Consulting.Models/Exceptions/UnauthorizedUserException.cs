namespace Consulting.Models.Exceptions {
    public class UnauthorizedUserException : Exception {
        public UnauthorizedUserException() : base() {

        }

        public UnauthorizedUserException(string msg) : base(msg) {

        }
    }
}
