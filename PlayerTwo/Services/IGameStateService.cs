using PlayerTwo.Models;
using System.Collections.Generic;

namespace PlayerTwo.Services
{
    public interface IGameStateService
    {
        IEnumerable<Card> GetHand();

        IEnumerable<Card> GetOpponentHand();

        IEnumerable<Card> GetBoard();

        IEnumerable<Card> GetOpponentBoard();
    }
}
