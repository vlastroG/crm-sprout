namespace Consulting.Desktop {
    public class Program {
        [STAThread]
        public static void Main() {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
