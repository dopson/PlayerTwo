using System.Collections.Generic;

namespace PlayerTwo.Models
{
    public class Card
    {
        public string Id { get; set; }
        public string GameEventId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Rarity { get; set; }
        public string Type { get; set; }
        public int Cost { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public bool Collectible { get; set; }
        public string Set { get; set; }
        public string Faction { get; set; }
        public string Artist { get; set; }
        public string Flavor { get; set; }
        public List<string> Mechanics { get; set; }
        public List<int> Dust { get; set; }
    }
}
