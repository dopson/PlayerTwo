using System;

namespace PlayerTwo.Utils
{
    public interface ILogFileMonitor
    {
        void Start();
        void Stop();
        void SetPath(string path);
        void SetDelimiter(string delimiter);
        event EventHandler<LogFileMonitorLineEventArgs> OnLine;
    }
}
