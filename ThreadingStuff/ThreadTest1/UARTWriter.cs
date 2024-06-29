
using System.Collections.Concurrent;

namespace wshakespear.UART;

public class UARTWriter : BaseThread
{
    private ConcurrentQueue<byte> TXQueue;
    private object TXLock;
    public UARTWriter(ConcurrentQueue<byte> _rxqueue, ConcurrentQueue<byte> _txqueue, object _txlock) : base(_rxqueue)
    {
        TXQueue = _txqueue;
        TXLock = _txlock;
        MyThread.Name = "Writer";
    }

    protected override void Work()
    {
        // Check for values in send queue
        if (TXQueue.Count > 0)
        {
            Output($"TXQueue count is: {TXQueue.Count} Obtaining Lock");
            lock (TXLock)
            {
                Output("Obtained lock, reading from queue into an array");
                // Store the amount to dequeue and create empty array for vals
                int step = TXQueue.Count;
                byte[] values = new byte[step];
                
                // Fetch all values
                for (int i = 0; i < step;i++)
                {
                    byte tx;
                    bool res;
                    do
                    {
                        res = TXQueue.TryDequeue(out tx);
                    }
                    while (!res);

                    values[i] = tx;
                }
                
                // Send values
                Write(values);
            }
            
        }
    }

    private void Write(byte[] tx)
    {
        string output = "";
        foreach (byte b in tx)
        {
            output += $"'{b}' ";
        }
        Output(output);
    }
}