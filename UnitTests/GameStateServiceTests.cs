using Moq;
using NUnit.Framework;
using PlayerTwo.Models;
using PlayerTwo.Services;
using PlayerTwo.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class GameStateServiceTests
    {
        public GameStateService Service;
        private static Mock<IGameLogReaderService> _gameLogReaderService;
        private static Mock<IGameLogParserService> _gameLogParserService;
        private static Mock<ICardService> _cardService;

        public GameStateServiceTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            _gameLogReaderService = new Mock<IGameLogReaderService>();
            _cardService = new Mock<ICardService>();
            _gameLogParserService = new Mock<IGameLogParserService>();

            Service = new GameStateService(_gameLogReaderService.Object, _cardService.Object, _gameLogParserService.Object);
        }

        [Test]
        public void TestGetHandGetsAppended()
        {
            var newStates = new List<string>
            {
                "1",
                "2",
                "3"
            };

            var newEvent = new GameEvent
            {
                Id = "dsafa",
                Type = GameEventType.Card,
                PlayerId = PlayerId.Self,
                ToZone = Zone.HAND,
                CardId = "CS1_042"
            };

            var newCard = new Card
            {
                Id = newEvent.CardId,
                GameEventId = newEvent.Id,
            };

            var gameLog = Service.GetHand();

            Assert.IsEmpty(gameLog);

            _gameLogReaderService.Setup(m => m.GetNewActions()).Returns(newStates);
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            gameLog = Service.GetHand();

            Assert.AreEqual(newStates.Count(), gameLog.Count());

            gameLog = Service.GetHand();

            Assert.AreEqual(newStates.Count() * 2, gameLog.Count());
        }

        [Test]
        public void TestHandGetsReduced()
        {
            var newStates = new List<string>
            {
                "1"
            };

            var newEvent = new GameEvent
            {
                Id = "dsafa",
                Type = GameEventType.Card,
                PlayerId = PlayerId.Self,
                ToZone = Zone.HAND,
                CardId = "CS1_042"
            };

            var newCard = new Card
            {
                Id = newEvent.CardId,
                GameEventId = newEvent.Id,
            };

            _gameLogReaderService.Setup(m => m.GetNewActions()).Returns(newStates);
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            var gameLog = Service.GetHand();

            Assert.AreEqual(newStates.Count(), gameLog.Count());

            newEvent.ToZone = Zone.INVALID;
            newEvent.FromZone = Zone.HAND;

            gameLog = Service.GetHand();

            Assert.AreEqual(0, gameLog.Count());
        }

        [Test]
        public void TestOpponentHandGetsAppended()
        {
            var newStates = new List<string>
            {
                "1",
                "2",
                "3"
            };

            var newEvent = new GameEvent
            {
                Id = "dsafa",
                Type = GameEventType.Card,
                PlayerId = PlayerId.Opponent,
                ToZone = Zone.HAND,
            };

            var gameLog = Service.GetOpponentHand();

            Assert.IsEmpty(gameLog);

            _gameLogReaderService.Setup(m => m.GetNewActions()).Returns(newStates);
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns((Card)null);

            gameLog = Service.GetOpponentHand();

            Assert.AreEqual(newStates.Count(), gameLog.Count());

            gameLog = Service.GetOpponentHand();

            Assert.AreEqual(newStates.Count() * 2, gameLog.Count());
        }

        [Test]
        public void TestOpponentHandGetsReduced()
        {
            var newStates = new List<string>
            {
                "1",
            };

            var newEvent = new GameEvent
            {
                Id = "dsafa",
                Type = GameEventType.Card,
                PlayerId = PlayerId.Opponent,
                ToZone = Zone.HAND,
            };

            _gameLogReaderService.Setup(m => m.GetNewActions()).Returns(newStates);
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns((Card)null);

            var gameLog = Service.GetOpponentHand();

            Assert.AreEqual(newStates.Count(), gameLog.Count());

            newEvent.ToZone = Zone.INVALID;
            newEvent.FromZone = Zone.HAND;

            gameLog = Service.GetOpponentHand();

            Assert.AreEqual(0, gameLog.Count());
        }

        [Test]
        public void TestBoardGetsAppendedByOwnPlay()
        {
            var newStates = new List<string>
            {
                "1"
            };

            var newEvent = new GameEvent
            {
                Id = "1",
                Type = GameEventType.Card,
                CardId = "5321",
                PlayerId = PlayerId.Self,
                FromZone = Zone.HAND,
                ToZone = Zone.PLAY
            };

            var newCard = new Card
            {
                Id = newEvent.CardId
            };

            var gameLog = Service.GetBoard();

            Assert.IsEmpty(gameLog);

            _gameLogReaderService.Setup(m => m.GetNewActions()).Returns(newStates);
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            gameLog = Service.GetBoard();

            Assert.AreEqual(newStates.Count(), gameLog.Count());

            gameLog = Service.GetBoard();

            Assert.AreEqual(newStates.Count()*2, gameLog.Count());
        }

        [Test]
        public void TestBoardGetsAppendedByDeckPull()
        {
            var newStates = new List<string>
            {
                "1"
            };

            var newEvent = new GameEvent
            {
                Id = "1",
                Type = GameEventType.Card,
                CardId = "5321",
                ToPlayer = PlayerId.Self,
                FromZone = Zone.DECK,
                ToZone = Zone.PLAY
            };

            var newCard = new Card
            {
                Id = newEvent.CardId
            };

            var gameLog = Service.GetBoard();

            Assert.IsEmpty(gameLog);

            _gameLogReaderService.Setup(m => m.GetNewActions()).Returns(newStates);
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            gameLog = Service.GetBoard();

            Assert.AreEqual(newStates.Count(), gameLog.Count());

            gameLog = Service.GetBoard();

            Assert.AreEqual(newStates.Count() * 2, gameLog.Count());
        }

        [Test]
        public void TestBoardGetsReduced()
        {
            var newStates = new List<string>
            {
                "1"
            };

            var newEvent = new GameEvent
            {
                Id = "1",
                Type = GameEventType.Card,
                CardId = "5321",
                PlayerId = PlayerId.Self,
                FromZone = Zone.HAND,
                ToZone = Zone.PLAY
            };

            var newCard = new Card
            {
                Id = newEvent.CardId
            };

            _gameLogReaderService.Setup(m => m.GetNewActions()).Returns(newStates);
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            var gameLog = Service.GetBoard();

            Assert.AreEqual(newStates.Count(), gameLog.Count());

            newEvent.FromZone = Zone.PLAY;
            newEvent.ToZone = Zone.GRAVEYARD;
            gameLog = Service.GetBoard();

            Assert.AreEqual(0, gameLog.Count());
        }

        [Test]
        public void TestOpponentBoardGetsAppendedByHisPlay()
        {
            var newStates = new List<string>
            {
                "1"
            };

            var newEvent = new GameEvent
            {
                Id = "1",
                Type = GameEventType.Card,
                CardId = "5321",
                PlayerId = PlayerId.Opponent,
                FromZone = Zone.HAND,
                ToZone = Zone.PLAY
            };

            var newCard = new Card
            {
                Id = newEvent.CardId
            };

            var gameLog = Service.GetOpponentBoard();

            Assert.IsEmpty(gameLog);

            _gameLogReaderService.Setup(m => m.GetNewActions()).Returns(newStates);
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            gameLog = Service.GetOpponentBoard();

            Assert.AreEqual(newStates.Count(), gameLog.Count());

            gameLog = Service.GetOpponentBoard();

            Assert.AreEqual(newStates.Count() * 2, gameLog.Count());
        }

        [Test]
        public void TestOpponentBoardGetsAppendedByDeckPull()
        {
            var newStates = new List<string>
            {
                "1"
            };

            var newEvent = new GameEvent
            {
                Id = "1",
                Type = GameEventType.Card,
                CardId = "5321",
                ToPlayer = PlayerId.Opponent,
                FromZone = Zone.DECK,
                ToZone = Zone.PLAY
            };

            var newCard = new Card
            {
                Id = newEvent.CardId
            };

            var gameLog = Service.GetOpponentBoard();

            Assert.IsEmpty(gameLog);

            _gameLogReaderService.Setup(m => m.GetNewActions()).Returns(newStates);
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            gameLog = Service.GetOpponentBoard();

            Assert.AreEqual(newStates.Count(), gameLog.Count());

            gameLog = Service.GetOpponentBoard();

            Assert.AreEqual(newStates.Count() * 2, gameLog.Count());
        }
    }
}
