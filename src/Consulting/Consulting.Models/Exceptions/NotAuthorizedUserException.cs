namespace Consulting.Models.Exceptions {
    public class NotAuthorizedUserException : Exception {
        public NotAuthorizedUserException() : base() {

        }

        public NotAuthorizedUserException(string msg) : base(msg) {

        }
    }
}
