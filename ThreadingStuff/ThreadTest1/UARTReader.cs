
using System.Collections.Concurrent;

namespace wshakespear.UART;

public class UARTReader : BaseThread
{
    private object RXLock;
    public UARTReader(ConcurrentQueue<byte> _rxqueue, object _rxlock) : base(_rxqueue)
    {
        RXLock = _rxlock;
        MyThread.Name = "Reader";
    }
    protected override void Work()
    {
        // Output("reader - work");
        Thread.Sleep(200);
    }
}