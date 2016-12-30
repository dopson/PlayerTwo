using System;
using PlayerTwo.Services;
using System.Threading;
using PlayerTwo.Utils;
using System.Linq;
using System.Text;

namespace PlayerTwo
{
    class Program
    {
        private static IGameStateService _gameStateService;
        private static string _logFilePath = "C:\\Program Files (x86)\\Hearthstone\\Logs\\";
        private static string _logFileName = "Zone.log";
        private static string _cardFilePath = "\\Assets\\cards.json";
        private static string _delimiter = "\r\n";

        static void Main(string[] args)
        {
            Initialize();

            while(Run() != 0)
            {
                Thread.Sleep(1000);
            }

            Console.WriteLine("Press any key to quit");
            Console.ReadLine();
        }

        private static void Initialize()
        {
            _gameStateService = new GameStateService(
                new GameLogReaderService(
                        new LogFileMonitor(_logFilePath + _logFileName, _delimiter)
                    ),
                new CardService(_cardFilePath),
                new GameLogParserService()
            );
        }

        private static int Run()
        {
            try
            {
                var handCards = _gameStateService.GetHand();

                if (handCards.Any())
                {
                    var allCardsString = new StringBuilder();

                    foreach (var card in handCards)
                    {
                        allCardsString.Append(card.Name);
                        allCardsString.Append(" - ");
                    }

                    Console.WriteLine("My cards: " + allCardsString);
                }

                var board = _gameStateService.GetBoard();

                if (board.Any())
                {
                    var allBoardString = new StringBuilder();

                    foreach (var card in board)
                    {
                        allBoardString.Append(card.Name);
                        allBoardString.Append(" - ");
                    }

                    Console.WriteLine("My board: " + allBoardString);
                }

                var opponentCards = _gameStateService.GetOpponentHand();
                if (opponentCards.Any())
                {
                    Console.WriteLine("Opponent has " + opponentCards.Count() + " cards");
                }

                var opponentBoard = _gameStateService.GetOpponentBoard();
                if (opponentBoard.Any())
                {
                    var opponentBoardString = new StringBuilder();

                    foreach (var card in opponentBoard)
                    {
                        opponentBoardString.Append(card.Name);
                        opponentBoardString.Append(" - ");
                    }

                    Console.WriteLine("Opponent's board: " + opponentBoardString);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }

            return 1;
        }
    }
}
