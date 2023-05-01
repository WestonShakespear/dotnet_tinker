
namespace testOne {

    using System;
using System.Runtime.InteropServices;

public class Win32 {
     [DllImport("user32.dll", CharSet=CharSet.Auto)]
     public static extern IntPtr MessageBox(int hWnd, String text,
                     String caption, uint type);
}
    public class Program {
        public static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "Test One Window"))
            {
                //Win32.MessageBox(0, "hello", "platform invoke", 0);
                game.Run();
            }
        }
    }
}