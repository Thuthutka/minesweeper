using System;
using System.Threading;

namespace PSP
{
    public class MainData
    {
        private int mines = 0;
        private int x, y;
        protected int rows;
        protected int columns;
        protected int mineNumber;
        protected int[,] board = new int[20, 20];
        protected char[,] gameBoard = new char[20, 20];
        protected int howManyTillWin;
        protected bool didPlayerLose;

        public MainData()
        {
            rows = Program.chooseRows();
            columns = Program.chooseColumns();
            mineNumber = Program.chooseMineNumber();
            didPlayerLose = false;
            howManyTillWin = rows * columns - mineNumber;
        }

        public void drawGameBoard()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    gameBoard[i,j] = '+';
                }
            }
        }

        public void putMines()
        {
            while (mines != mineNumber)
            {
                Random random = new Random();
                x = random.Next(rows);
                y = random.Next(columns);

                if (board[x,y] == 0)
                {
                    board[x, y]++;
                    mines++;
                    //Console.WriteLine("Mina padeta");
                }
            }
        }

        public bool canPlayerPut(int xCoordinate1, int yCoordinate1)
        {
            return (xCoordinate1 >= 0) && (xCoordinate1 < rows) && (yCoordinate1 >= 0) && (yCoordinate1 < columns);
        }

        public bool isMine(int xCoordinate2, int yCoordinate2)
        {
            if (board[xCoordinate2, yCoordinate2] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public char howManyMinesHaveBeenPut(int xCoordinate, int yCoordinate)
        {
            short howMany = 0;

            if(canPlayerPut(xCoordinate - 1, yCoordinate - 1) && isMine(xCoordinate - 1, yCoordinate - 1))
            {
                howMany++;
                Console.WriteLine("Mina 1");
            }

            if (canPlayerPut(xCoordinate, yCoordinate - 1) && isMine(xCoordinate, yCoordinate - 1))
            {
                howMany++;
                Console.WriteLine("Mina 2");

            }

            if (canPlayerPut(xCoordinate + 1, yCoordinate - 1) && isMine(xCoordinate + 1, yCoordinate - 1))
            {
                howMany++;
                Console.WriteLine("Mina 3");

            }

            if (canPlayerPut(xCoordinate - 1, yCoordinate) && isMine(xCoordinate - 1, yCoordinate))
            {
                howMany++;
                Console.WriteLine("Mina 4");

            }

            if (canPlayerPut(xCoordinate + 1, yCoordinate) && isMine(xCoordinate + 1, yCoordinate))
            {
                howMany++;
                Console.WriteLine("Mina 5");

            }

            if (canPlayerPut(xCoordinate - 1, yCoordinate + 1) && isMine(xCoordinate - 1, yCoordinate + 1))
            {
                howMany++;
                Console.WriteLine("Mina 6");

            }

            if (canPlayerPut(xCoordinate, yCoordinate + 1) && isMine(xCoordinate, yCoordinate + 1))
            {
                howMany++;
                Console.WriteLine("Mina 7");

            }
            if (canPlayerPut(xCoordinate + 1, yCoordinate + 1) && isMine(xCoordinate + 1, yCoordinate + 1))
            {
                howMany++;
                Console.WriteLine("Mina 8");

            }
            char aaa = Convert.ToChar(howMany);
            return Convert.ToChar(howMany, System.Globalization.CultureInfo.CurrentCulture);
        }

        public void realBoard()
        {
            Console.WriteLine();
            for (int i = 0; i < rows; i++)
            {
                Console.Write("   ");
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(board[i,j] + "  ");
                }
                Console.WriteLine();
            }
        }
    }

    public class MineSweeper : MainData
    {
        private int activeRow;
        private int activeColumn;
        private int howManyFlags;

        public MineSweeper()
        {
            activeRow = 0;
            activeColumn = 0;
            howManyFlags = mineNumber;
        }

        public void startGame()
        {
            putMines();
            drawGameBoard();
            activeCoordinate(activeRow, activeColumn);

            if (didPlayerLose)
            {
                Console.Clear();
                putMinesToBoard();
                showBoard();
                Console.WriteLine("You lost!");
            }
            else
            {
                if (howManyTillWin == 0)
                {
                    Console.Clear();
                    putMinesToBoard();
                    showBoard();
                    Console.WriteLine("You won!");
                }
            }
        }

        public void activeCoordinate(int activeRow, int activeColumn)
        {
            activeRow = 0;
            activeColumn = 0;
            while (true)
            {
                if (howManyTillWin == 0)
                {
                    break;
                }

                char temp = gameBoard[activeRow, activeColumn];
                gameBoard[activeRow, activeColumn] = ' ';
                showBoard();
                Thread.Sleep(500);
                gameBoard[activeRow, activeColumn] = temp;
                showBoard();
                Thread.Sleep(200);
                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow)
                {
                    if (canPlayerPut(activeRow, activeColumn))
                    {
                        activeRow--;
                    }
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    if (canPlayerPut(activeRow, activeColumn))
                    {
                        activeRow++;
                    }
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    if (canPlayerPut(activeRow, activeColumn))
                    {
                        activeColumn--;
                    }
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    if (canPlayerPut(activeRow, activeColumn))
                    {
                        activeColumn++;
                    }
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    if (gameBoard[activeRow, activeColumn] != 'F')
                    {
                        if (isMine(activeRow, activeColumn))
                        {
                            didPlayerLose = true;
                            break;
                        }
                        else
                        {
                            if (gameBoard[activeRow, activeColumn] == '+')
                            {
                                gameBoard[activeRow, activeColumn] = howManyMinesHaveBeenPut(activeRow, activeColumn);
                                howManyTillWin--;
                            }
                        }
                    }
                }
                else if (key == ConsoleKey.F)
                {
                    if (gameBoard[activeRow, activeColumn] == '+' && howManyFlags > 0)
                    {
                        gameBoard[activeRow, activeColumn] = 'F';
                        howManyFlags--;
                    }
                    else
                    {
                        if (gameBoard[activeRow, activeColumn] == 'F')
                        {
                            gameBoard[activeRow, activeColumn] = '+';
                            howManyFlags++;
                        }
                    }
                }
                
            }
        }

        public void showBoard()
        {
            cleanScreen();
            Console.WriteLine("You have " + howManyFlags + " flags left");
            for (int i = 0; i < rows + 2; i++)
            {
                for (int j = 0; j < columns + 2; j++)
                {
                    if (i == 0 || i == rows + 1)
                    {
                        Console.Write("% ");
                    }
                    else
                    {
                        if (j == 0)
                        {
                            Console.Write("%");
                        }
                        else
                        {
                            if (j == columns + 1)
                            {
                                Console.Write(" %");
                            }
                            else
                            {
                                Console.Write("  " + gameBoard[i - 1, j - 1]);
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("Arrows - Movement\nSpace - Click\nF - Flag ");
        }

        public void putMinesToBoard()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (board[i, j] == 1)
                    {
                        gameBoard[i, j] = '*';
                    }
                }
            }
        }

        public void cleanScreen()
        {
            Console.SetCursorPosition(0, 0);
            

        }

        public bool lost()
        {
            if (didPlayerLose)
            {
                return true;
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            char answer;
            MineSweeper game = new MineSweeper();

            while (true)
            {
                //chooseRows();
                //chooseColumns();

                //Console.Clear();
                game.startGame();

                Console.WriteLine(" ");
                //Console.Clear();

                Console.WriteLine("Do you want to play again?");
                answer = Convert.ToChar(Console.ReadLine());

                if (answer == 'Y' || answer == 'y')
                {
                    Console.Clear();
                }
                else
                {
                    break;
                }
            }
        }

        public static int chooseRows()
        {
            bool choosing = false;
            int rows;
            while (!choosing)
            {
                Console.WriteLine("Choose rows number (min 1, max 20");
                rows = Convert.ToInt32(Console.ReadLine());

                if (rows < 1 || rows > 20)
                {
                    Console.WriteLine("Wrong parameter!");
                }
                else
                {
                    return rows;
                }
            }
            return -1;
        }

        public static int chooseColumns()
        {
            bool choosing = false;
            int columns;

            while (!choosing)
            {
                Console.WriteLine("Choose columns number (min 1, max 20");
                columns = Convert.ToInt32(Console.ReadLine());
                
                if (columns < 1 || columns > 20)
                {
                    Console.WriteLine("Wrong parameter!");
                }
                else
                {
                    return columns;
                }
            }
            return -1;
        }

        public static int chooseMineNumber()
        {
            bool choosing = false;
            int mines;

            while (!choosing)
            {
                Console.WriteLine("Choose mine number (min 1, max 20");
                mines = Convert.ToInt32(Console.ReadLine());

                if (mines < 1 || mines > 20)
                {
                    Console.WriteLine("Wrong parameter!");
                }
                else
                {
                    return mines;
                }
            }
            return -1;
        }
    }

    
}
