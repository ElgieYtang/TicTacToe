namespace TicTacToeMVC.Models
{
    public static class Scoreboard
    {
        private static int xWins = 0;
        private static int oWins = 0;
        private static int draws = 0;

        public static int XWins => xWins;
        public static int OWins => oWins;
        public static int Draws => draws;

        public static void UpdateScore(string winner)
        {
            if (winner == "X")
                xWins++;
            else if (winner == "O")
                oWins++;
            else
                draws++;
        }

        public static void ResetScores()
        {
            xWins = 0;
            oWins = 0;
            draws = 0;
        }
    }
}
