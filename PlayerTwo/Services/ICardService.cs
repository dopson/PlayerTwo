using PlayerTwo.Models;

namespace PlayerTwo.Services
{
    public interface ICardService
    {
        Card GetCard(string cardId);

        void LoadCards(string filePath);
    }
}
