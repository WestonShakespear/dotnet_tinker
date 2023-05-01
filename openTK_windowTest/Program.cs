
namespace testOne {
    public class Program {
        public static void Main(string[] args)
        {
            string fontPath = @"C:\Windows\Fonts\calibri.ttf";
            float fontSize = 25f;

            using (Game game = new Game(800, 600, "Test One Window", fontPath, fontSize))
            {
                //Win32.MessageBox(0, "hello", "platform invoke", 0);
                game.Run();
            }
        }
    }
}