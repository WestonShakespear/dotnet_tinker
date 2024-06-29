
using System.Collections.Concurrent;

namespace wshakespear.UART;

public abstract class BaseThread
{
    protected Thread MyThread;
    private bool Run = true;

    protected ConcurrentQueue<byte> RXQueue;

    public BaseThread(ConcurrentQueue<byte> _rx_queue)
    {
        MyThread = new Thread(Loop);
        RXQueue = _rx_queue;
    }

    public void Start() => MyThread.Start();
    public void Stop()
    {
        Run = false;
        MyThread.Join();
        Output("Thread Stopped");
    }

    private void Loop()
    {
        Output("Thread Started");
        while (Run)
        {
            Work();
        }
    }

    protected void Output(string message)
    {
        string fmt = "tid:{0}\t| {1}";
        string output = String.Format(fmt, Thread.CurrentThread.Name, message);

        Console.WriteLine(output);
    }

    protected abstract void Work();
}