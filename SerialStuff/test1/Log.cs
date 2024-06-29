
namespace test1;

public class Log
{
    private string Path;
    private string Filename;
    private string Fullpath;

    private bool Open = false;
    private FileStream? FStream;

    public Log(string _path, string _filename, bool _clear = false)
    {
        Path = _path;
        Filename = _filename;
        Fullpath = Path.Combine(_path, _filename);

        OpenLog();
    }

    public void ClearLog()
    {

    }

    public bool OpenLog()
    {
        if (FStream != null || Open) return false;

        FStream = new FileStream(Fullpath, FileMode.Append,
        FileAccess.ReadWrite, FileShare.None);
        IsOpen = true;
    }

    public void CloseLog()
    {

    }

    public bool IsOpen() { return Open; }
}