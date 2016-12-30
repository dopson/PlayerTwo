using PlayerTwo.Utils;
using System;
using System.Collections.Generic;

namespace PlayerTwo.Services
{
    public class GameLogReaderService : IGameLogReaderService
    {
        private List<string> _newActions;
        private static ILogFileMonitor _logFileMonitor;

        public GameLogReaderService(ILogFileMonitor logFileMonitor)
        {
            _newActions = new List<string>();
            _logFileMonitor = logFileMonitor;
            _logFileMonitor.OnLine += new EventHandler<LogFileMonitorLineEventArgs>(NewLineAdded);
            _logFileMonitor.Start();
        }

        public List<string> GetNewActions()
        {
            var newActions = new List<string>(_newActions);

            _newActions.Clear();

            return newActions;
        }

        public void AddNewAction(string action)
        {
            _newActions.Add(action);
        }

        private void NewLineAdded(object sender, LogFileMonitorLineEventArgs e)
        {
            AddNewAction(e.Line);
        }
    }
}
