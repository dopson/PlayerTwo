namespace PlayerTwo.Utils
{
    public static class TeamParser
    {
        public const string OPPOSING = "OPPOSING";
        public const string FRIENDLY = "FRIENDLY";

        public static PlayerId GetPlayerId(string team)
        {
            switch (team)
            {
                case OPPOSING:
                    return PlayerId.Opponent;
                case FRIENDLY:
                    return PlayerId.Self;
                default:
                    return PlayerId.Unknown;
            };
        }

        public static PlayerId GetPlayerIdById(string playerId)
        {
            var parsedPlayerId = PlayerId.Unknown;
            PlayerId.TryParse(playerId, true, out parsedPlayerId);

            return parsedPlayerId;
        }
    }
}
