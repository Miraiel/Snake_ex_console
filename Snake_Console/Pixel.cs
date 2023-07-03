// используем структуру т.к. это приметивный тип, и все его свойства являются значимыми типами
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Console
{
    public struct Pixel
    {
        private const char PixelChar = '▓';
        public Pixel(int x, int y, ConsoleColor color, int pixelSize = 3)
        {
            X = x;
            Y = y;
            Color = color;
            PixelSize = pixelSize;
        }
        //задаем координаты и цвет пикселя
        public int X { get; }

        public int Y { get; }

        public ConsoleColor Color { get; }
        public int PixelSize { get; }

        public void Draw()  //метод отрисовки пикселя
        {
            Console.ForegroundColor = Color;

            for (int x = 0;x<PixelSize; x++)
            {
                for(int y = 0;y<PixelSize; y++)
                {
                    Console.SetCursorPosition(X*PixelSize+ x, Y*PixelSize+y);
                    Console.Write(PixelChar);
                }
            }

        }

        public void Clear()  //метод очистки пикселя
        {
            for (int x = 0; x < PixelSize; x++)
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    Console.SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Console.Write(' ');
                }
            }

        }
    }
}
