using System;

namespace CheckersGame
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckersGame game = new CheckersGame();
            game.Play();
        }
    }

    class CheckersGame
    {
        private CheckersBoard board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;

        public CheckersGame()
        {
            board = new CheckersBoard();
            player1 = new Player('X'); // Player 1 uses 'X'
            player2 = new Player('O'); // Player 2 uses 'O'
            currentPlayer = player1;
        }

        public void Play()
        {
            bool isPlaying = true;

            while (isPlaying)
            {
                Console.Clear();
                board.RenderBoard();
                Console.WriteLine($"{currentPlayer.Symbol}'s turn. Enter move (e.g. H3 G4): ");
                string move = Console.ReadLine();

                if (ProcessPlayerMove(move))
                {
                    if (board.IsGameOver())
                    {
                        Console.Clear();
                        board.RenderBoard();
                        Console.WriteLine($"{currentPlayer.Symbol} wins!");
                        isPlaying = false;
                    }
                    else
                    {
                        SwapTurn();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid move! Press Enter to try again.");
                    Console.ReadLine();
                }
            }
        }

        private bool ProcessPlayerMove(string move)
        {
            string[] moveParts = move.Split(' ');
            if (moveParts.Length != 2)
                return false;

            (int startX, int startY) = ConvertPositionToIndex(moveParts[0]);
            (int endX, int endY) = ConvertPositionToIndex(moveParts[1]);

            if (!IsValidPosition(startX, startY) || !IsValidPosition(endX, endY))
                return false;

            return board.MovePiece(startX, startY, endX, endY, currentPlayer.Symbol);
        }

        private void SwapTurn()
        {
            currentPlayer = currentPlayer == player1 ? player2 : player1;
        }

        private (int, int) ConvertPositionToIndex(string position)
        {
            char column = position[0];
            int row = int.Parse(position[1].ToString());

            int x = row - 1;
            int y = column - 'A';
            return (x, y);
        }

        private bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
    }

    class Player
    {
        public char Symbol { get; private set; }

        public Player(char symbol)
        {
            Symbol = symbol;
        }
    }

    class CheckersBoard
    {
        private char[,] boardGrid;

        public CheckersBoard()
        {
            boardGrid = new char[8, 8];
            SetupBoard();
        }

        public void SetupBoard()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    boardGrid[i, j] = ' ';

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 8; j++)
                    if ((i + j) % 2 == 1) boardGrid[i, j] = 'X';

            for (int i = 5; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if ((i + j) % 2 == 1) boardGrid[i, j] = 'O';
        }

        public void RenderBoard()
        {
            Console.WriteLine("   A B C D E F G H");
            for (int i = 0; i < 8; i++)
            {
                Console.Write($"{i + 1} ");
                for (int j = 0; j < 8; j++)
                {
                    char cell = boardGrid[i, j];
                    if ((i + j) % 2 == 1)
                        Console.Write(cell == ' ' ? "□ " : $"{cell} ");
                    else
                        Console.Write("░ ");
                }
                Console.WriteLine();
            }
        }

        public bool MovePiece(int startX, int startY, int endX, int endY, char playerSymbol)
        {
            if (boardGrid[startX, startY] != playerSymbol || boardGrid[endX, endY] != ' ')
                return false;

            if (Math.Abs(startX - endX) == 1 && Math.Abs(startY - endY) == 1)
            {
                boardGrid[startX, startY] = ' ';
                boardGrid[endX, endY] = playerSymbol;
                return true;
            }

            if (Math.Abs(startX - endX) == 2 && Math.Abs(startY - endY) == 2)
            {
                int middleX = (startX + endX) / 2;
                int middleY = (startY + endY) / 2;

                if (boardGrid[middleX, middleY] != playerSymbol && boardGrid[middleX, middleY] != ' ')
                {
                    boardGrid[startX, startY] = ' ';
                    boardGrid[middleX, middleY] = ' ';
                    boardGrid[endX, endY] = playerSymbol;
                    return true;
                }
            }

            return false;
        }

        public bool IsGameOver()
        {
            bool player1HasPieces = false, player2HasPieces = false;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (boardGrid[i, j] == 'X') player1HasPieces = true;
                    if (boardGrid[i, j] == 'O') player2HasPieces = true;
                }
            }

            return !(player1HasPieces && player2HasPieces);
        }
    }
}