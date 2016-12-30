namespace PlayerTwo.Utils
{
    public static class Regexes
    {
        //public static string ZoneChangeRegex = @".* ZoneChangeList.ProcessChanges\(\) \- id=\d* local=.* \[name=(.*) id=(\d*) zone=.* zonePos=\d* cardId=(.*) player=(\d)\] zone from .*";
        public static string ZoneChangeRegex = @".* ZoneChangeList.ProcessChanges\(\) \- id=\d* local=.* \[name=(.*) id=(\d*) zone=.* zonePos=\d* cardId=(.*) player=(\d)\] zone from(?:\s){0,1}(OPPOSING|FRIENDLY){0,1}(?:\s){0,1}(\w{0,10})(?:\s){0,1}\-\>(?:\s){0,1}(OPPOSING|FRIENDLY){0,1}(?:\s){0,1}(\w{0,10})";

        public static string NewPlayerRegex = @"/\[Power\] GameState\.DebugPrintPower\(\) - TAG_CHANGE Entity=(.*) tag=PLAYER_ID value=(.)$/";

        public static string GameOverRegex = @"/\[Power\] GameState\.DebugPrintPower\(\) - TAG_CHANGE Entity=(.*) tag=PLAYSTATE value=(LOST|WON|TIED)$/";
    }
}
