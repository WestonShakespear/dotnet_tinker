
using System.Collections.Concurrent;
using System.Text;

namespace wshakespear.UART;

public class UARTStream
{
    private UARTReader Reader;
    private UARTWriter Writer;
    private ConcurrentQueue<byte> RXQueue;
    private ConcurrentQueue<byte> TXQueue;
    private object RXLock = new();
    private object TXLock = new();

    public UARTStream()
    {
        RXQueue = new ConcurrentQueue<byte>();
        TXQueue = new ConcurrentQueue<byte>();
        Reader = new UARTReader(RXQueue, RXLock);
        Writer = new UARTWriter(RXQueue, TXQueue, TXLock);
    }

    public void Open()
    {
        // Open stream

        // Start reader/writer threads
        Start();
    }

    public void Close()
    {
        // Stop reader/writer threads
        Reader.Stop();
        Writer.Stop();
    }

    private void Start()
    {
        Reader.Start();
        Writer.Start();
    }

    private void Stop()
    {

    }

    public void Send(string message)
    {
        byte[] data = new byte[Encoding.ASCII.GetByteCount(message)];
        Encoding.ASCII.GetBytes(message, data);

        lock (TXLock)
        {
            foreach (byte b in data)
            {
                TXQueue.Enqueue(b);
            }
        }
        
    }
}