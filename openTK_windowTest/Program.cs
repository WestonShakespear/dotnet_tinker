
namespace testOne {
    public class Program {
        public static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "Test One Window"))
            {
                game.Run();
            }
        }
    }
}