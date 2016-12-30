using PlayerTwo.Models;
namespace PlayerTwo.Services
{
    public interface IGameLogParserService
    {
        GameEvent GetGameEvent(string logEntry);
    }
}
