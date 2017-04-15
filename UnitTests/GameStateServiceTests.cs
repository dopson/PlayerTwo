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
        private static Mock<IGameLogParserService> _gameLogParserService;
        private static Mock<ICardService> _cardService;
        private static Mock<ILogFileMonitor> _logFileMonitor;

        public GameStateServiceTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            _logFileMonitor = new Mock<ILogFileMonitor>();
            _cardService = new Mock<ICardService>();
            _gameLogParserService = new Mock<IGameLogParserService>();

            Service = new GameStateService(_logFileMonitor.Object, _cardService.Object, _gameLogParserService.Object);
        }

        [Test]
        public void TestGetHandGetsAppended()
        {
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
            
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetHand();

            Assert.AreEqual(1, gameLog.Count());


            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetHand();

            Assert.AreEqual(2, gameLog.Count());
        }

        [Test]
        public void TestHandGetsReduced()
        {
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

            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            var gameLog = Service.GetHand();

            Assert.AreEqual(1, gameLog.Count());

            newEvent.ToZone = Zone.INVALID;
            newEvent.FromZone = Zone.HAND;

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetHand();

            Assert.AreEqual(0, gameLog.Count());
        }

        [Test]
        public void TestOpponentHandGetsAppended()
        {
            var newEvent = new GameEvent
            {
                Id = "dsafa",
                Type = GameEventType.Card,
                PlayerId = PlayerId.Opponent,
                ToZone = Zone.HAND,
            };

            var gameLog = Service.GetOpponentHand();

            Assert.IsEmpty(gameLog);

            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns((Card)null);

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetOpponentHand();

            Assert.AreEqual(1, gameLog.Count());

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetOpponentHand();

            Assert.AreEqual(2, gameLog.Count());
        }

        [Test]
        public void TestOpponentHandGetsReduced()
        {
            var newEvent = new GameEvent
            {
                Id = "dsafa",
                Type = GameEventType.Card,
                PlayerId = PlayerId.Opponent,
                ToZone = Zone.HAND,
            };

            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns((Card)null);

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            var gameLog = Service.GetOpponentHand();

            Assert.AreEqual(1, gameLog.Count());

            newEvent.ToZone = Zone.INVALID;
            newEvent.FromZone = Zone.HAND;

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetOpponentHand();

            Assert.AreEqual(0, gameLog.Count());
        }

        [Test]
        public void TestBoardGetsAppendedByOwnPlay()
        {
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

            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetBoard();

            Assert.AreEqual(1, gameLog.Count());

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetBoard();

            Assert.AreEqual(2, gameLog.Count());
        }

        [Test]
        public void TestBoardGetsAppendedByDeckPull()
        {
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

            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);


            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetBoard();

            Assert.AreEqual(1, gameLog.Count());

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetBoard();

            Assert.AreEqual(2, gameLog.Count());
        }

        [Test]
        public void TestBoardGetsReduced()
        {
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

            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            var gameLog = Service.GetBoard();

            Assert.AreEqual(1, gameLog.Count());

            newEvent.FromZone = Zone.PLAY;
            newEvent.ToZone = Zone.GRAVEYARD;
            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });
            gameLog = Service.GetBoard();

            Assert.AreEqual(0, gameLog.Count());
        }

        [Test]
        public void TestOpponentBoardGetsAppendedByHisPlay()
        {
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
            
        
            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetOpponentBoard();

            Assert.AreEqual(1, gameLog.Count());

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetOpponentBoard();

            Assert.AreEqual(2, gameLog.Count());
        }

        [Test]
        public void TestOpponentBoardGetsAppendedByDeckPull()
        {
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

            _gameLogParserService.Setup(m => m.GetGameEvent(It.IsAny<string>())).Returns(newEvent);
            _cardService.Setup(m => m.GetCard(It.IsAny<string>())).Returns(newCard);

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetOpponentBoard();

            Assert.AreEqual(1, gameLog.Count());

            _logFileMonitor.Raise(m => m.OnLine += null, new LogFileMonitorLineEventArgs { Line = "something" });

            gameLog = Service.GetOpponentBoard();

            Assert.AreEqual(2, gameLog.Count());
        }
    }
}
