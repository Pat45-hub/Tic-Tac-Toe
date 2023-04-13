using System;
using System.Text.RegularExpressions;

namespace Tic_Tac_Toe3
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
        private string[,] grid;
        private int size;

        public Board(int size = 3)
        {
            this.size = size;
            grid = new string[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = " ";
                }
            }
        }

        public bool CheckGameOver()
        {
            // Vérifier les lignes, les colonnes et les diagonales
            for (int i = 0; i < size; i++)
            {
                if ((grid[i, 0] == grid[i, 1] && grid[i, 1] == grid[i, 2] && grid[i, 0] != " ") ||
                    (grid[0, i] == grid[1, i] && grid[1, i] == grid[2, i] && grid[0, i] != " ") ||
                    (grid[0, 0] == grid[1, 1] && grid[1, 1] == grid[2, 2] && grid[0, 0] != " ") ||
                    (grid[0, 2] == grid[1, 1] && grid[1, 1] == grid[2, 0] && grid[0, 2] != " "))
                {
                    return true;
                }
            }

            // Vérifier s'il reste des cases vides
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (grid[i, j] == " ")
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool PlaceSymbol(int x, int y, string symbol)
        {
            if (grid[x, y] == " ")
            {
                grid[x, y] = symbol;
                return true;
            }

            return false;
        }

        public void Print()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write("|{0}", grid[i, j]);
                }
                Console.WriteLine("|");
                if (i < size - 1)
                {
                    Console.WriteLine("-----");
                }
            }
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
                    validMove = board.PlaceSymbol(x, y, symbol);
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
        private Random random;

        public Computer(string symbol)
        {
            this.symbol = symbol;
            random = new Random();
        }

        public void Play(Board board)
        {
            bool validMove = false;
            while (!validMove)
            {
                int x = random.Next(0, 3);
                int y = random.Next(0, 3);
                validMove = board.PlaceSymbol(x, y, symbol);
            }
        }
    }
}

