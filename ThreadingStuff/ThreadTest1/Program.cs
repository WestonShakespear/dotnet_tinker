
using wshakespear.UART;

namespace Program;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Program Start");

        UARTStream stream = new UARTStream();

        stream.Open();
        
        while (true)
        {
            string? message = Console.ReadLine();

            if (message != null)
            {
                if (message == "quit") break;

                stream.Send(message);
            }
        }


        stream.Close();
    }
}