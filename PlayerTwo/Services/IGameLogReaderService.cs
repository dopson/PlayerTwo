using System.Collections.Generic;

namespace PlayerTwo.Services
{
    public interface IGameLogReaderService
    {
        List<string> GetNewActions();

        void AddNewAction(string action);
    }
}
