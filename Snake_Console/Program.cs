using System;
using static System.Console;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Snake_Console
{
    internal class Program
    {
        private const int MapWidth = 30; //глобальная переменная ширина карты
        private const int MapHaight = 20; //глобальная переменная высота карты

        private const int ScreenWidth = MapWidth * 3; //глобальная переменная ширина карты
        private const int ScreenHaight = MapHaight * 3; //глобальная переменная высота карты

        private const int FrameMs = 200;

        private const ConsoleColor BorderColor = ConsoleColor.Gray;

        private const ConsoleColor HeadColor = ConsoleColor.DarkBlue;
        private const ConsoleColor BodyColor = ConsoleColor.Cyan;

        private const ConsoleColor FoodColor = ConsoleColor.Green;

        private static readonly Random Random = new Random();

        static void Main()
        {
            //класическая настройка консоли под вывод изображения 
            SetWindowSize(ScreenWidth, ScreenHaight); //устанавливаем размер окна
            SetBufferSize(ScreenWidth, ScreenHaight); //устанавливаем размер буфера
            CursorVisible = false; //отключаем курсор

            while (true)
            {
                StartGame();

                Thread.Sleep(1000);
                ReadKey();
            }
        }

        static void StartGame()
        {
            Clear();

            DrawBorber();
            Direction currentMovement = Direction.Right;

            var snake = new Snake(10, 5, HeadColor, BodyColor);

            Pixel food = GenFood(snake);
            food.Draw();

            int score = 0;
            int lagMs = 0;

            var sw = new Stopwatch();

            while (true)
            {
                sw.Restart();

                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMs - lagMs)
                {
                    if (currentMovement == oldMovement)
                    {
                        currentMovement = ReadMovement(currentMovement);
                    }
                }

                sw.Restart();


                if(snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);
                    food = GenFood(snake);
                    food.Draw();
                    score++;

                    Task.Run(() => Beep(1200, 200));
                }
                else
                {
                    snake.Move(currentMovement);
                }
                                

                if (snake.Head.X == MapWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MapHaight - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;

                lagMs = (int)sw.ElapsedMilliseconds;

                //Thread.Sleep(200);
            }

            snake.Clear();
            food.Clear();

            SetCursorPosition(ScreenWidth / 3, ScreenHaight / 2);
            WriteLine($"Game Over, scor: {score}!");

            Task.Run(() => Beep(200, 600));
        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;
            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHaight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y
                       || snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }

        static Direction ReadMovement(Direction currentDirection)
        {
            if (!KeyAvailable)
                return currentDirection;
            ConsoleKey key = ReadKey(true).Key;

            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                _ => currentDirection
            };

            return currentDirection;
        }

        static void DrawBorber()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw();
                new Pixel(i, MapHaight - 1, BorderColor).Draw();
            }

            for (int i = 0; i < MapHaight; i++)
            {
                new Pixel(0, i, BorderColor).Draw();
                new Pixel(MapWidth - 1, i, BorderColor).Draw();
            }
        }
    }
}
