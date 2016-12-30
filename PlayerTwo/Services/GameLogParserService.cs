using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerTwo.Models;
using PlayerTwo.Utils;
using System.Text.RegularExpressions;

namespace PlayerTwo.Services
{
    public class GameLogParserService : IGameLogParserService
    {
        public GameEvent GetGameEvent(string logEntry)
        {
            var gameEvent = new GameEvent
            {
                Type = GameEventType.Unknown
            };

            if (Regex.IsMatch(logEntry, Regexes.ZoneChangeRegex))
            {
                gameEvent.Type = GameEventType.Card;

                var eventParts = Regex.Matches(logEntry, Regexes.ZoneChangeRegex)[0].Groups.Cast<Group>().Select(match => match.ToString()).ToList();

                gameEvent.SetValues(eventParts);
            }

            return gameEvent;
        }
    }
}
