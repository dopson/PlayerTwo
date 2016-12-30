using System.Collections.Generic;
using System.Linq;
using PlayerTwo.Models;
using System.IO;
using Newtonsoft.Json;

namespace PlayerTwo.Services
{
    public class CardService : ICardService
    {
        private List<Card> _cards;

        public CardService(string cardFilePath)
        {
            _cards = new List<Card>();
            LoadCards(cardFilePath);
        }

        public Card GetCard(string cardId)
        {
            return _cards.FirstOrDefault(card => card.Id == cardId);
        }

        public void LoadCards(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                var json = r.ReadToEnd();

                _cards = JsonConvert.DeserializeObject<List<Card>>(json);
            }
        }
    }
}
