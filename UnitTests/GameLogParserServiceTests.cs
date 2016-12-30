using NUnit.Framework;
using PlayerTwo.Models;
using PlayerTwo.Services;
using PlayerTwo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class GameLogParserServiceTests
    {
        private GameLogParserService Service;

        public GameLogParserServiceTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            Service = new GameLogParserService();
        }

        [Test]
        public void TestUnknownEventTypeIsReturned()
        {
            var gameEvent = Service.GetGameEvent("totally random string");

            Assert.AreEqual(GameEventType.Unknown, gameEvent.Type);
        }

        [Test]
        public void TestMinionOpponentPlayedIsCorrectlyFound()
        {
            // Creature changes position
            var cardEventLogEntry = "15:54:41.5033465 ZoneChangeList.ProcessChanges() - id=9 local=False [name=Goldshire Footman id=46 zone=PLAY zonePos=2 cardId=CS1_042 player=2] pos from 1 -> 2";

            Assert.Fail("Position change not yet implemented");

            var expectedGameEvent = new GameEvent
            {
                Type = GameEventType.Card,
                CardId = "CS1_042"
            };

            var gameEvent = Service.GetGameEvent(cardEventLogEntry);

            AssertGameEvent(expectedGameEvent, gameEvent);
        }

        [Test]
        public void TestOpponentSpellIsCorrectlyFound()
        {
            // Opponent backstab played
            var cardEventLogEntry = "22:01:02.2860978 ZoneChangeList.ProcessChanges() - id=11 local=False [name=Backstab id=62 zone=PLAY zonePos=2 cardId=CS2_072 player=2] zone from OPPOSING HAND -> ";

            var expectedGameEvent = new GameEvent
            {
                Type = GameEventType.Card,
                PlayerId = PlayerId.Opponent,
                FromZone = Zone.HAND,
                FromPlayer = PlayerId.Opponent,
                CardId = "CS2_072",
                Id = "62"
            };

            var gameEvent = Service.GetGameEvent(cardEventLogEntry);

            AssertGameEvent(expectedGameEvent, gameEvent);
        }

        [Test]
        public void TestOpponentSpellToGraveyardIsCorrectlyFound()
        {
            // Opponent backstab goes to graveyard
            var cardEventLogEntry = "asdsdas ZoneChangeList.ProcessChanges() - id=12 local=False [name=Backstab id=62 zone=GRAVEYARD zonePos=0 cardId=CS2_072 player=2] zone from  -> OPPOSING GRAVEYARD";

            var expectedGameEvent = new GameEvent
            {
                Type = GameEventType.Card,
                PlayerId = PlayerId.Opponent,
                ToZone = Zone.GRAVEYARD,
                ToPlayer = PlayerId.Opponent,
                CardId = "CS2_072",
                Id = "62"
            };

            var gameEvent = Service.GetGameEvent(cardEventLogEntry);

            AssertGameEvent(expectedGameEvent, gameEvent);
        }

        [Test]
        public void TestOpponentMinionDyingIsCorrectlyFound()
        {
            // Opponent minion goes to graveyard
            var cardEventLogEntry = "18:12:06.8452716 ZoneChangeList.ProcessChanges() - id=18 local=False [name=Murloc Raider id=48 zone=GRAVEYARD zonePos=1 cardId=CS2_168 player=2] zone from OPPOSING PLAY -> OPPOSING GRAVEYARD";

            var expectedGameEvent = new GameEvent
            {
                Type = GameEventType.Card,
                PlayerId = PlayerId.Opponent,
                FromPlayer = PlayerId.Opponent,
                FromZone = Zone.PLAY,
                ToPlayer = PlayerId.Opponent,
                ToZone = Zone.GRAVEYARD,
                CardId = "CS2_168",
                Id = "48"
            };

            var gameEvent = Service.GetGameEvent(cardEventLogEntry);

            AssertGameEvent(expectedGameEvent, gameEvent);
        }

        [Test]
        public void TestOwnMinionDyingIsCorrectlyFound()
        {
            // Own creature dies
            var cardEventLogEntry = "22:01:06.5662695 ZoneChangeList.ProcessChanges() - id=13 local=False [name=Mirror Image id=70 zone=GRAVEYARD zonePos=2 cardId=CS2_mirror player=1] zone from FRIENDLY PLAY -> FRIENDLY GRAVEYARD";

            var expectedGameEvent = new GameEvent
            {
                Type = GameEventType.Card,
                Id = "70",
                PlayerId = PlayerId.Self,
                CardId = "CS2_mirror",
                FromPlayer = PlayerId.Self,
                FromZone = Zone.PLAY,
                ToPlayer = PlayerId.Self,
                ToZone = Zone.GRAVEYARD
            };

            var gameEvent = Service.GetGameEvent(cardEventLogEntry);

            AssertGameEvent(expectedGameEvent, gameEvent);
        }

        [Test]
        public void TestOwnMulliganIsCorrectlyFound()
        {
            // Mulligan or some such
            var cardEventLogEntry = "D 22:06:50.0856568 ZoneChangeList.ProcessChanges() - id=3 local=False [name=Acolyte of Pain id=18 zone=DECK zonePos=2 cardId=EX1_007 player=1] zone from FRIENDLY HAND -> FRIENDLY DECK";

            var expectedGameEvent = new GameEvent
            {
                Type = GameEventType.Card,
                Id = "18",
                CardId = "EX1_007",
                PlayerId = PlayerId.Self,
                FromPlayer = PlayerId.Self,
                FromZone = Zone.HAND,
                ToPlayer = PlayerId.Self,
                ToZone = Zone.DECK
            };

            var gameEvent = Service.GetGameEvent(cardEventLogEntry);

            AssertGameEvent(expectedGameEvent, gameEvent);
        }

        [Test]
        public void TestOwnCardDrawIsCorrectlyFound()
        {
            // Card draw
            var cardEventLogEntry = "D 22:06:50.1251894 ZoneChangeList.ProcessChanges() - id=3 local=False [name=Ice Lance id=11 zone=HAND zonePos=0 cardId=CS2_031 player=1] zone from FRIENDLY DECK -> FRIENDLY HAND";

            var expectedGameEvent = new GameEvent
            {
                Type = GameEventType.Card,
                Id = "11",
                CardId = "CS2_031",
                PlayerId = PlayerId.Self,
                FromPlayer = PlayerId.Self,
                FromZone = Zone.DECK,
                ToPlayer = PlayerId.Self,
                ToZone = Zone.HAND
            };

            var gameEvent = Service.GetGameEvent(cardEventLogEntry);

            AssertGameEvent(expectedGameEvent, gameEvent);
        }

        [Test]
        public void TestOwnCardPositionChangeIsCorrectlyFound()
        {
            // Card changes position in hand
            var cardEventLogEntry = "D 22:06:50.1527091 ZoneChangeList.ProcessChanges() - id=3 local=False [name=Ice Lance id=11 zone=HAND zonePos=3 cardId=CS2_031 player=1] pos from 0 -> 3";
            Assert.Fail("Position change not yet implemented");
        }

        [Test]
        public void TestOwnPlayedSpellIsCorrectlyFound()
        {
            // Self played coin
            var cardEventLogEntry = "D 22:07:31.7512042 ZoneChangeList.ProcessChanges() - id=1 local=True [name=The Coin id=68 zone=HAND zonePos=5 cardId=GAME_005 player=1] zone from FRIENDLY HAND -> ";

            var expectedGameEvent = new GameEvent
            {
                Type = GameEventType.Card,
                Id = "68",
                CardId = "GAME_005",
                PlayerId = PlayerId.Self,
                FromPlayer = PlayerId.Self,
                FromZone = Zone.HAND
            };

            var gameEvent = Service.GetGameEvent(cardEventLogEntry);

            AssertGameEvent(expectedGameEvent, gameEvent);
        }

        [Test]
        public void TestOwnSpellToGraveyardIsCorrecltyFound()
        {
            // Self played coin goes to graveyard
            var cardEventLogEntry = "D 22:07:34.2469785 ZoneChangeList.ProcessChanges() - id=7 local=False [name=The Coin id=68 zone=GRAVEYARD zonePos=0 cardId=GAME_005 player=1] zone from  -> FRIENDLY GRAVEYARD";

            var expectedGameEvent = new GameEvent
            {
                Type = GameEventType.Card,
                Id = "68",
                CardId = "GAME_005",
                PlayerId = PlayerId.Self,
                ToPlayer = PlayerId.Self,
                ToZone = Zone.GRAVEYARD
            };

            var gameEvent = Service.GetGameEvent(cardEventLogEntry);

            AssertGameEvent(expectedGameEvent, gameEvent);
        }

        [Test]
        public void TestOwnCardFromHandToPlayIsCorrectlyFound()
        {
            // Creature from hand to play
            var cardEventLogEntry = "D 22:07:32.7182966 ZoneChangeList.ProcessChanges() - id=2 local=True [name=Loot Hoarder id=28 zone=HAND zonePos=5 cardId=EX1_096 player=1] zone from FRIENDLY HAND -> FRIENDLY PLAY";

            var expectedGameEvent = new GameEvent
            {
                Type = GameEventType.Card,
                Id = "28",
                CardId = "EX1_096",
                PlayerId = PlayerId.Self,
                FromPlayer = PlayerId.Self,
                FromZone = Zone.HAND,
                ToPlayer = PlayerId.Self,
                ToZone = Zone.PLAY
            };

            var gameEvent = Service.GetGameEvent(cardEventLogEntry);

            AssertGameEvent(expectedGameEvent, gameEvent);
        }

        [Test]
        public void TestOwnCardPositionOnBoardIsCorrectlyFound()
        {
            // Creature played and position determined
            var cardEventLogEntry = "D 22:07:32.7233031 ZoneChangeList.ProcessChanges() - id=2 local=True [name=Loot Hoarder id=28 zone=HAND zonePos=5 cardId=EX1_096 player=1] pos from 5->1";
            Assert.Fail("Position change not yet implemented");
        }

        [Test]
        public void TestWeaponSpawnFromEffectIsCorrectlyFound()
        {
            // Weapon from nzoth first mate
            var cardEventLogEntry = "D 22:37:24.0147492 ZoneChangeList.ProcessChanges() - id=6 local=False [name=Rusty Hook id=69 zone=PLAY zonePos=0 cardId=OG_058 player=1] zone from  -> FRIENDLY PLAY (Weapon)";
            Assert.Fail("Position change not yet implemented");
        }


        private void AssertGameEvent(GameEvent expected, GameEvent actual)
        {
            Assert.AreEqual(expected.CardId, actual.CardId);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.FromPlayer, actual.FromPlayer);
            Assert.AreEqual(expected.FromZone, actual.FromZone);
            Assert.AreEqual(expected.PlayerId, actual.PlayerId);
            Assert.AreEqual(expected.ToZone, actual.ToZone);
            Assert.AreEqual(expected.ToPlayer, actual.ToPlayer);
        }
    }
}
