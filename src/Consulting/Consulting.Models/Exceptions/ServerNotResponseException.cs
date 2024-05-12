namespace Consulting.Models.Exceptions {
    public class ServerNotResponseException : Exception {
        public ServerNotResponseException() : base() { }


        public ServerNotResponseException(string message) : base(message) { }
    }
}
