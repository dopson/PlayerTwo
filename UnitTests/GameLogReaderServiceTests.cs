using Moq;
using NUnit.Framework;
using PlayerTwo.Services;
using PlayerTwo.Utils;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class GameLogReaderServiceTests
    {
        public GameLogReaderService Service; 
        private static Mock<ILogFileMonitor> _logFileMonitor;

        public GameLogReaderServiceTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            _logFileMonitor = new Mock<ILogFileMonitor>();

            Service = new GameLogReaderService(_logFileMonitor.Object);
        }

        [Test]
        public void TestGetNewActionsOnlyReturnsNewActions()
        {
            var firstAction = "123";
            var secondAction = "321";

            var actions = Service.GetNewActions();

            Assert.IsEmpty(actions);

            Service.AddNewAction(firstAction);

            actions = Service.GetNewActions();

            Assert.AreEqual(1, actions.Count());
            Assert.AreEqual(actions.First(), firstAction);

            actions = Service.GetNewActions();

            Assert.IsEmpty(actions);

            Service.AddNewAction(firstAction);
            Service.AddNewAction(secondAction);

            actions = Service.GetNewActions();

            Assert.AreEqual(2, actions.Count());
            Assert.AreEqual(actions.First(), firstAction);
            Assert.AreEqual(actions.Last(), secondAction);
        }
    }
}
