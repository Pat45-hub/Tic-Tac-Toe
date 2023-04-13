using System;
using System.Text.RegularExpressions;

namespace Tic_Tac_Toe5
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    public class Game
    {
        private Board board;
        private Player player;
        private Computer computer;
        private bool isGameOver;

        public void Start()
        {
            board = new Board();
            player = new Player("X");
            computer = new Computer("O");

            while (!isGameOver)
            {
                Console.Clear();
                board.Print();
                player.Play(board);
                if (board.CheckGameOver())
                {
                    break;
                }
                Console.Clear();
                board.Print();
                computer.Play(board);
                isGameOver = board.CheckGameOver();
            }

            Console.Clear();
            board.Print();
            End();
        }

        public void End()
        {
            Console.WriteLine("Fin du jeu.");
            Console.ReadKey();
        }
    }

    public class Board
    {
        private string[] grid;

        public Board()
        {
            grid = new string[10];
            for (int i = 1; i < 10; i++)
            {
                grid[i] = " ";
            }
        }

        public bool PlaceSymbol(int position, string symbol)
        {
            if (grid[position] == " ")
            {
                grid[position] = symbol;
                return true;
            }
            return false;
        }

        public string GetWinner()
        {
            // Check for win
            for (int i = 1; i <= 9; i += 3)
            {
                if (grid[i] == grid[i + 1] && grid[i] == grid[i + 2] && grid[i] != " ")
                {
                    return grid[i];
                }
            }

            for (int i = 1; i <= 3; i++)
            {
                if (grid[i] == grid[i + 3] && grid[i] == grid[i + 6] && grid[i] != " ")
                {
                    return grid[i];
                }
            }

            if ((grid[1] == grid[5] && grid[1] == grid[9] && grid[1] != " ") || (grid[3] == grid[5] && grid[3] == grid[7] && grid[3] != " "))
            {
                return grid[5];
            }

            // Check for draw
            bool isDraw = true;
            for (int i = 1; i < 10; i++)
            {
                if (grid[i] == " ")
                {
                    isDraw = false;
                    break;
                }
            }

            if (isDraw) return "D";

            return null;
        }

        public bool CheckGameOver()
        {
            string winner = GetWinner();
            return winner != null;
        }

        public void Print()
        {
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", grid[1], grid[2], grid[3]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", grid[4], grid[5], grid[6]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", grid[7], grid[8], grid[9]);
            Console.WriteLine("     |     |      ");
        }

        public Board Clone()
        {
            Board newBoard = new Board();
            Array.Copy(grid, newBoard.grid, grid.Length);
            return newBoard;
        }
    }

    public class Player
    {
        private string symbol;

        public Player(string symbol)
        {
            this.symbol = symbol;
        }

        public void Play(Board board)
        {
            bool validMove = false;
            while (!validMove)
            {
                Console.WriteLine("Entrez votre coup sous la forme X,Y : ");
                string input = Console.ReadLine();
                Regex regex = new Regex(@"^(\d),(\d)$");
                Match match = regex.Match(input);
                if (match.Success)
                {
                    int x = int.Parse(match.Groups[1].Value);
                    int y = int.Parse(match.Groups[2].Value);
                    int position = (x - 1) * 3 + y;
                    validMove = board.PlaceSymbol(position, symbol);
                    if (!validMove)
                    {
                        Console.WriteLine("Coup invalide, veuillez réessayer.");
                    }
                }
                else
                {
                    Console.WriteLine("Format invalide, veuillez réessayer.");
                }
            }
        }
    }

    public class Computer
    {
        private string symbol;
        private string opponentSymbol;

        public Computer(string symbol)
        {
            this.symbol = symbol;
            opponentSymbol = symbol == "X" ? "O" : "X";
        }

        public void Play(Board board)
        {
            int bestScore = int.MinValue;
            int bestMove = -1;

            for (int i = 1; i < 10; i++)
            {
                Board newBoard = board.Clone();
                if (newBoard.PlaceSymbol(i, symbol))
                {
                    int score = BestMove(newBoard, false);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = i;
                    }
                }
            }

            board.PlaceSymbol(bestMove, symbol);
        }

        private int BestMove(Board board, bool isMaximizing)
        {
            string winner = board.GetWinner();
            if (winner != null)
            {
                if (winner == symbol) return 1;
                if (winner == opponentSymbol) return -1;
                return 0;
            }

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int i = 1; i < 10; i++)
                {
                    Board newBoard = board.Clone();
                    if (newBoard.PlaceSymbol(i, symbol))
                    {
                        int score = BestMove(newBoard, false);
                        bestScore = Math.Max(score, bestScore);
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int i = 1; i < 10; i++)
                {
                    Board newBoard = board.Clone();
                    if (newBoard.PlaceSymbol(i, opponentSymbol))
                    {
                        int score = BestMove(newBoard, true);
                        bestScore = Math.Min(score, bestScore);
                    }
                }
                return bestScore;
            }
        }
    }
}

