using System;
using PlayerTwo.Utils;
using System.Collections.Generic;
using System.Linq;

namespace PlayerTwo.Models
{
    public class GameEvent
    {
        public string Id { get; set; }

        public string CardId { get; set; }

        public GameEventType Type { get; set; }

        public PlayerId PlayerId { get; set; }

        public Zone FromZone { get; set; }

        public Zone ToZone { get; set; }

        public PlayerId FromPlayer { get; set; }

        public PlayerId ToPlayer { get; set; }

        public void SetValues(List<string> eventParts)
        {
            // 0 = something
            // 1 = card name
            // 2 = event id
            // 3 = card id
            // 4 = player id
            // 5 = opposing or friendly event
            // 6 = from zone
            // 7 = opposing or friendly event
            // 8 = to zone
           /* 0 = kaikki ?
1 = name
2 = eventId
2 = cardId
3 = player
4 = fromPlayer
5 = fromZone
6 = toPlayer
7 = toZone
*/

            Id = eventParts[2];
            CardId = eventParts[3];
            PlayerId = TeamParser.GetPlayerIdById(eventParts[4]);
            FromPlayer = TeamParser.GetPlayerId(eventParts[5]);
            FromZone = ZoneParser.GetZone(eventParts[6]);
            ToPlayer = TeamParser.GetPlayerId(eventParts[7]);
            ToZone = ZoneParser.GetZone(eventParts[8]);
        }
    }
}
