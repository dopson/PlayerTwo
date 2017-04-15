﻿using System;
using PlayerTwo.Utils;
using System.Collections.Generic;
using System.Linq;

namespace PlayerTwo.Models
{
    public class GameEvent
    {
        public string GlobalEventId { get; set; }

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
            // 2 = global event id
            // 3 = event id
            // 4 = card id
            // 5 = player id
            // 6 = opposing or friendly event
            // 7 = from zone
            // 8 = opposing or friendly event
            // 9 = to zone

            GlobalEventId = eventParts[2];
            Id = eventParts[3];
            CardId = eventParts[4];
            PlayerId = TeamParser.GetPlayerIdById(eventParts[5]);
            FromPlayer = TeamParser.GetPlayerId(eventParts[6]);
            FromZone = ZoneParser.GetZone(eventParts[7]);
            ToPlayer = TeamParser.GetPlayerId(eventParts[8]);
            ToZone = ZoneParser.GetZone(eventParts[9]);
        }
    }
}
