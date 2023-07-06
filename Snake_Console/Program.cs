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
        private const int MapWidth = 30; //ширина карты
        private const int MapHaight = 20; //высота карты

        private const int ScreenWidth = MapWidth * 3; //размер экрана консоли по ширине
        private const int ScreenHaight = MapHaight * 3; //размер экрана консоли по высоте

        private const int FrameMs = 200;    //перерыв между кадрами мс

        private const ConsoleColor BorderColor = ConsoleColor.Gray; // задаем цвет бортиков серый

        private const ConsoleColor HeadColor = ConsoleColor.DarkBlue; // задаем цвет головы
        private const ConsoleColor BodyColor = ConsoleColor.Cyan;   //задаем цвет тела

        private const ConsoleColor FoodColor = ConsoleColor.Green;  //задаем цвет пикселей

        private static readonly Random Random = new Random();   //генерируем жрачку

        static void Main()
        {
            //класическая настройка консоли под вывод изображения 
            SetWindowSize(ScreenWidth, ScreenHaight); //устанавливаем размер окна
            SetBufferSize(ScreenWidth, ScreenHaight); //устанавливаем размер буфера
            CursorVisible = false; //отключаем курсор

            //запуск игры с перезагрузкой
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

            var snake = new Snake(10, 5, HeadColor, BodyColor); //создаем экземпляр змеи

            Pixel food = GenFood(snake);    //генирируем что пожевать
            food.Draw();                    //рисуем что пожевать

            int score = 0;
            int lagMs = 0;

            var sw = new Stopwatch();

            while (true)
            {
                sw.Restart();

                Direction oldMovement = currentMovement;

                //запрещаем вигатся в ротивоположном направлении и считывам направление только если его меняем
                while (sw.ElapsedMilliseconds <= FrameMs - lagMs)
                {
                    if (currentMovement == oldMovement)
                    {
                        currentMovement = ReadMovement(currentMovement);
                    }
                }

                sw.Restart();

                //проверяем попала ли еда на голову
                if(snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);
                    food = GenFood(snake);              //отрисовка и генерация новой еды
                    food.Draw();
                    score++;

                    Task.Run(() => Beep(1200, 200));    //отдельный поток с частотой и длительностью
                }
                else
                {
                    snake.Move(currentMovement);
                }

                //условие game over
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

        static Pixel GenFood(Snake snake)   //медот возвращающий что пожевать, не в змее!!!!
        {
            Pixel food;
            //случайно генерируем еду, продолжаем до съедения и добавления его в тело
            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHaight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y
                       || snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }

        static Direction ReadMovement(Direction currentDirection)   //метод чтения управления с клавы, принемаем текущее напр
        {
            if (!KeyAvailable)
                return currentDirection;
            ConsoleKey key = ReadKey(true).Key;

            currentDirection = key switch   //управление завязанно на стрелки, передаем параметры с вшкусешщт
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
            for (int i = 0; i < MapWidth; i++)//проходимся по ширине экрана
            {
                // создаем объекты пикселя и рисуем по горизонтали
                new Pixel(i, 0, BorderColor).Draw();
                new Pixel(i, MapHaight - 1, BorderColor).Draw();
            }

            for (int i = 0; i < MapHaight; i++)//проходимся по высоте экрана
            {
                //создаем пиксели и рисуем по вертикали
                new Pixel(0, i, BorderColor).Draw();
                new Pixel(MapWidth - 1, i, BorderColor).Draw();
            }
        }
    }
}
