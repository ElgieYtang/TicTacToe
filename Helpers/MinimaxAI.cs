namespace TicTacToeMVC.Helpers
{
    public class MinimaxAI
    {
        private const int AI_PLAYER = 1;  // AI represents 'O'
        private const int HUMAN_PLAYER = -1; // Human represents 'X'

        public int[] BestMove(int[,] board)
        {
            int bestScore = int.MinValue;
            int[] bestMove = { -1, -1 };

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == 0)
                    {
                        board[row, col] = AI_PLAYER;
                        int score = Minimax(board, 0, false);
                        board[row, col] = 0;

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove[0] = row;
                            bestMove[1] = col;
                        }
                    }
                }
            }
            return bestMove;
        }

        private int Minimax(int[,] board, int depth, bool isMaximizing)
        {
            int result = Evaluate(board);
            if (result != 0)
                return result;
            if (IsBoardFull(board))
                return 0;

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (board[row, col] == 0)
                        {
                            board[row, col] = AI_PLAYER;
                            int score = Minimax(board, depth + 1, false);
                            board[row, col] = 0;
                            bestScore = Math.Max(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (board[row, col] == 0)
                        {
                            board[row, col] = HUMAN_PLAYER;
                            int score = Minimax(board, depth + 1, true);
                            board[row, col] = 0;
                            bestScore = Math.Min(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
        }

        private int Evaluate(int[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] != 0 && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    return board[i, 0];

                if (board[0, i] != 0 && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                    return board[0, i];
            }

            if (board[0, 0] != 0 && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                return board[0, 0];

            if (board[0, 2] != 0 && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                return board[0, 2];

            return 0;
        }

        private bool IsBoardFull(int[,] board)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == 0)
                        return false;
            return true;
        }
    }
}
