using NUnit.Framework;
using PlayerTwo.Models;
using PlayerTwo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class CardServiceTests
    {
        private CardService Service;
        private static string _cardFilePath = "C:\\Users\\vilit\\Documents\\Visual Studio 2015\\Projects\\cards.json";

        public CardServiceTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            Service = new CardService(_cardFilePath);
        }

        [Test]
        public void TestCorrectCardIsReturned()
        {
            var leeroyCard = new Card
            {
                Id = "EX1_116",
                Name = "Leeroy Jenkins",
                Cost = 5,
                Attack = 6,
                Health = 2,
                Mechanics = new List<string>
                {
                    "BATTLECRY",
                    "CHARGE"
                }
            };

            var card = Service.GetCard(leeroyCard.Id);

            AssertCardEquality(leeroyCard, card);
        }

        private void AssertCardEquality(Card expected, Card actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Cost, actual.Cost);
            Assert.AreEqual(expected.Attack, actual.Attack);
            Assert.AreEqual(expected.Health, actual.Health);
            Assert.AreEqual(expected.Mechanics, actual.Mechanics);
        }
    }
}
