using System;

class MinesweeperGame
{
    static void Main()
    {
        int numRows = 8;
        int numCols = 8;
        int numMines = 10;
        char[,] gameBoard = new char[numRows, numCols];
        bool[,] mineLocations = new bool[numRows, numCols];
        bool[,] flaggedCells = new bool[numRows, numCols];
        bool gameOver = false;

        InitializeBoard(gameBoard);
        PlaceMines(mineLocations, numMines);

        while (!gameOver)
        {
            Console.Clear();
            PrintBoard(gameBoard);

            Console.WriteLine("Entrée 'R' pour révéler une case, 'F' pour ajouter un drapeau pour indiquer la présence d'une mine, ou 'U' pour effacé un drapeau: ");
            string input = Console.ReadLine().ToUpper();

            if (input == "R")
            {
                Console.Write("Enter row and column (e.g., 1 2) to reveal: ");
                string[] revealInput = Console.ReadLine().Split();
                int row = int.Parse(revealInput[0]) - 1;
                int col = int.Parse(revealInput[1]) - 1;

                if (!flaggedCells[row, col])
                {
                    gameOver = RevealCell(gameBoard, mineLocations, row, col);
                }
                else
                {
                    Console.WriteLine("Cannot reveal a flagged cell. Please unflag it first.");
                    Console.ReadLine();
                }
            }
            else if (input == "F")
            {
                Console.Write("Enter row and column (e.g., 1 2) to flag: ");
                string[] flagInput = Console.ReadLine().Split();
                int row = int.Parse(flagInput[0]) - 1;
                int col = int.Parse(flagInput[1]) - 1;

                if (gameBoard[row, col] == '.')
                {
                    flaggedCells[row, col] = true;
                    gameBoard[row, col] = 'F';
                }
                else
                {
                    Console.WriteLine("Cannot flag an already revealed cell.");
                    Console.ReadLine();
                }
            }
            else if (input == "U")
            {
                Console.Write("Enter row and column (e.g., 1 2) to unflag: ");
                string[] unflagInput = Console.ReadLine().Split();
                int row = int.Parse(unflagInput[0]) - 1;
                int col = int.Parse(unflagInput[1]) - 1;

                if (flaggedCells[row, col])
                {
                    flaggedCells[row, col] = false;
                    gameBoard[row, col] = '.';
                }
                else
                {
                    Console.WriteLine("Cannot unflag a cell that is not flagged.");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'R' to reveal, 'F' to flag, or 'U' to unflag.");
                Console.ReadLine();
            }

            if (gameOver)
            {
                Console.Clear();
                PrintBoard(gameBoard);

                if (IsVictory(gameBoard, mineLocations))
                {
                    Console.WriteLine("Bravo! Vous avez gagné!");
                }
                else
                {
                    Console.WriteLine("Game Over! Vous avez touché une mine.");
                    Console.WriteLine("Entrée 'rejouer' pour recommencer");
                }
                string rejouer = Console.ReadLine().ToUpper();
                if(rejouer != "REJOUER" || rejouer != "REJOUÉ" || rejouer != "REJOUE")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Main();
                }
            }
        }
    }

    static void InitializeBoard(char[,] board)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = '.';
            }
        }
    }

    static void PlaceMines(bool[,] mineLocations, int numMines)
    {
        Random random = new Random();

        while (numMines > 0)
        {
            int row = random.Next(mineLocations.GetLength(0));
            int col = random.Next(mineLocations.GetLength(1));

            if (!mineLocations[row, col])
            {
                mineLocations[row, col] = true;
                numMines--;
            }
        }
    }

    static void PrintBoard(char[,] board)
    {
        Console.WriteLine("Minesweeper Game");
        Console.WriteLine("   1 2 3 4 5 6 7 8");
        Console.WriteLine("  ------------------");

        for (int i = 0; i < board.GetLength(0); i++)
        {
            Console.Write(i + 1 + " |");

            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.Write(" " + board[i, j]);
            }

            Console.WriteLine();
        }

        Console.WriteLine("  ------------------");
    }

    static bool RevealCell(char[,] board, bool[,] mines, int row, int col)
    {
        if (mines[row, col])
        {
            board[row, col] = 'X';
            return true; // Game over
        }

        int count = CountAdjacentMines(mines, row, col);
        board[row, col] = count.ToString()[0];

        if (count == 0)
        {
            // Auto-reveal adjacent cells with no mines
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < mines.GetLength(0) && j >= 0 && j < mines.GetLength(1) && board[i, j] == '.')
                    {
                        RevealCell(board, mines, i, j);
                    }
                }
            }
        }

        return false; // Game not over
    }

    static int CountAdjacentMines(bool[,] mines, int row, int col)
    {
        int count = 0;

        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = col - 1; j <= col + 1; j++)
            {
                if (i >= 0 && i < mines.GetLength(0) && j >= 0 && j < mines.GetLength(1) && mines[i, j])
                {
                    count++;
                }
            }
        }

        return count;
    }

    static bool IsVictory(char[,] board, bool[,] mines)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == '.' && !mines[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
