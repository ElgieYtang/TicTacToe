using System;
using System.Collections.Generic;

namespace TicTacToeMVC.Models
{
    public class TicTacToeModel
    {
        public char[,] Board { get; set; } = new char[3, 3];
        public char CurrentPlayer { get; private set; } = 'X';
        public bool GameOver { get; private set; } = false;
        public int[] WinningLine { get; private set; } = new int[3];

        public int XWins { get; set; }
        public int OWins { get; set; }
        public int Draws { get; set; }

        private Random random = new Random();

        public TicTacToeModel()
        {
            ResetBoard();
        }

        public void ResetBoard()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    Board[i, j] = ' ';
            CurrentPlayer = 'X';
            GameOver = false;
            WinningLine = new int[3];
        }

        public bool MakeMove(int row, int col)
        {
            if (Board[row, col] == ' ' && !GameOver)
            {
                Board[row, col] = CurrentPlayer;
                if (CheckWin())
                {
                    GameOver = true;
                    if (CurrentPlayer == 'X') XWins++;
                    else OWins++;
                }
                else if (IsDraw())
                {
                    GameOver = true;
                    Draws++;
                }
                else
                {
                    CurrentPlayer = (CurrentPlayer == 'X') ? 'O' : 'X';
                }
                return true;
            }
            return false;
        }

        public void MakeAIMove()
        {
            if (GameOver) return;

            List<(int, int)> emptyCells = new List<(int, int)>();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Board[i, j] == ' ') emptyCells.Add((i, j));

            if (emptyCells.Count > 0)
            {
                var (row, col) = emptyCells[random.Next(emptyCells.Count)];
                MakeMove(row, col);
            }
        }

        private bool CheckWin()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Board[i, 0] != ' ' && Board[i, 0] == Board[i, 1] && Board[i, 1] == Board[i, 2])
                {
                    WinningLine = new int[] { i * 3, i * 3 + 1, i * 3 + 2 };
                    return true;
                }
                if (Board[0, i] != ' ' && Board[0, i] == Board[1, i] && Board[1, i] == Board[2, i])
                {
                    WinningLine = new int[] { i, i + 3, i + 6 };
                    return true;
                }
            }

            if (Board[0, 0] != ' ' && Board[0, 0] == Board[1, 1] && Board[1, 1] == Board[2, 2])
            {
                WinningLine = new int[] { 0, 4, 8 };
                return true;
            }

            if (Board[0, 2] != ' ' && Board[0, 2] == Board[1, 1] && Board[1, 1] == Board[2, 0])
            {
                WinningLine = new int[] { 2, 4, 6 };
                return true;
            }

            return false;
        }

        private bool IsDraw()
        {
            foreach (var cell in Board)
                if (cell == ' ') return false;
            return true;
        }
    }
}
