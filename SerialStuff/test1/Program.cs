using System;
using System.Data.SqlTypes;
using System.IO.Ports;
using System.Linq.Expressions;
using System.Text;
using System.Threading;


namespace test1;

public class Program
{
    static bool Continue;
    static bool Echo = true;
    static string MessageRX = "";
    static string MessageTX = "";
    static byte[] BytesRX = new byte[0];
    static byte[] BytesTX = new byte[0];
    static SerialPort Port;
    static Thread ReaderThread;
    static Thread WriterThread;

    enum BB
    {
        NOP = 0X00,     // 00000000     // BBIOx
        SPI = 0X01,     // 00000001     // SPI1
        I2C = 0X02,     // 00000010     // 12C1
        UART = 0X03,    // 00000011     // ART1
        WIRE = 0X04,    // 00000100     // 1W01
        RAW = 0X05,     // 00000101     // RAW1
        EXIT = 0X0F     // 00001111
    };

    enum BBUART
    {
        EXIT = 0X00,        // 00000000     // BBIOx
        DVERSION = 0X01,    // 00000001     // ARTx
        ECHO_ON = 0X02,     // 00000010     // 0X01
        ECHO_OFF = 0X03,    // 00000011     // 0X01
        BRIDGE = 0X0F,      // 00001111     // ?

        SPEED_115200 = 0    // 0110 1010    // 0X01
    };

    public static void Main(string[] args)
    {
        Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKey);

        ReaderThread = new Thread(Reader);
        WriterThread = new Thread(Writer);

        Port = new SerialPort()
        {
            PortName = "/dev/buspirate",
            BaudRate = 115200,
            Parity = Parity.None,
            DataBits = 8,
            StopBits = StopBits.One,
            Handshake = Handshake.None,

            ReadTimeout = 1,
            WriteTimeout = 1
        };
        Port.Open();

        Continue = true;
        ReaderThread.Start();
        WriterThread.Start();
        
        MessageTX = "\n";

        while (Continue)
        {
            string? message = Console.ReadLine();

            if (message != null)
            {
                message += "\n";
                if (message.Contains("quit")) break;

                if (message == "bb\n") BitBang();
                if (message == "tt\n") TermBang();

                if (message == ":\n") Thing();

                else { MessageTX = message; }
            }
        }

        EndPrgm();
    }

    public static void Thing()
    {
        // Console.WriteLine("Check TT");
        // Console.WriteLine(Send("\n", timeout:10, expected: "\nHiZ>"));
     
        Console.WriteLine("Check BB");
        
        // MessageRX = "";
        Console.WriteLine(Send(0x00, timeout:100, expected: "BBIO1"));



        // MessageRX = "";
        // Console.WriteLine(Send(0x03, timeout:1000, expected: "ART1"));
    }

    public static void CancelKey(object sender, ConsoleCancelEventArgs args)
    {
        EndPrgm();
        Environment.Exit(0);
    }

    public static bool Send(string message, int timeout = 100, string expected = "")
    {
        MessageTX = message;
        while (MessageTX != "")
        {
            if (--timeout <= 0) break;
            Thread.Sleep(1);
        }

        return MessageRX.Contains(expected);
    }

    public static bool Send(byte[] messageBytes, int timeout = 100, byte[]? expectedBytes = null)
    {
        BytesTX = messageBytes;
        while (MessageTX != "")
        {
            if (--timeout <= 0) break;
            Thread.Sleep(1);
        }

        if (expectedBytes == null) return false;
        else
        {
            return BytesRX == expectedBytes;
        }
    }
    public static bool Send(byte[] messageBytes, int timeout = 100, string expected = "")
    {
        BytesTX = messageBytes;
        while (MessageTX != "")
        {
            if (--timeout <= 0) break;
            Thread.Sleep(1);
        }
        Console.WriteLine($"Got: {MessageRX}");
        Console.WriteLine($"Exp: {expected}");
        return MessageRX.Contains(expected);
    }

    public static bool Send(byte messageByte, int timeout = 100, byte expectedByte = 0x00)
    {
        return Send(new byte[] { messageByte }, timeout:timeout, expectedBytes: new byte[] { expectedByte });
    }

    public static bool Send(byte messageByte, int timeout = 100, string expected = "")
    {
        return Send(new byte[] { messageByte }, timeout:timeout, expected: expected);
    }

    public static void BitBang()
    {
        Echo = false;
        MessageRX = "";
        byte send = 0;
        for (int i = 0; i < 30; i++)
        {
            Console.Write("*");
            if (MessageRX.Length > 0) break;
            Send(send, timeout:0, expected:"");
            Thread.Sleep(10);
        }
        // Thread.Sleep(5000);
        Port.DiscardInBuffer();
        Echo = true;
    }

    public static void TermBang()
    {   
        Echo = false;
        MessageRX = "";
        byte send = 0x0F;
        for (int i = 0; i < 10; i++)
        {
            Console.Write("*");
            if (MessageRX.Length > 0) break;
            Send(send, timeout:0, expected:"");
            Thread.Sleep(10);
        }
        Console.Write(MessageRX);
        Echo = true;
    }

    public static void EndPrgm()
    {
        Console.WriteLine("\nCleaning Up...");
        Continue = false;
        
        ReaderThread.Join();
        Console.WriteLine("Reader thread joined and exited");
        
        WriterThread.Join();
        Console.WriteLine("Writer thread joined and exited");

        Port.Close();
    }

    public static void Reader()
    {
        Console.WriteLine("Reader started");
        while (Continue)
        {
            if (Port.BytesToRead > 0)
            {
                try
                {
                    BytesRX = new byte[Port.BytesToRead];
                    int status = Port.Read(BytesRX, 0, Port.BytesToRead);
                    
                    MessageRX = Encoding.UTF8.GetString(BytesRX);
                    
                    if (Echo) { Console.Write(MessageRX); }
                }
                catch (TimeoutException)
                {
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Trap: {e} {e.Message}");
                }
            }
            
        }
    }
    
    public static void Writer()
    {
        Console.WriteLine("Writer started");
        while (Continue)
        {
            if (MessageTX.Length == 0 && BytesTX.Length == 0) continue;
            
            try
            {
                if (MessageTX.Length > 0) Port.Write(MessageTX);
                if (BytesTX.Length > 0) Port.Write(BytesTX, 0, BytesTX.Length);
                MessageTX = "";
                BytesTX = new byte[0];

            }
            catch (Exception e)
            {
                Console.WriteLine($"Trap: {e} {e.Message}"); 
            }
        }
    }
}