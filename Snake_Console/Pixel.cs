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
        private const char PixelChar = '▓'; //задаем отрисовку
        public Pixel(int x, int y, ConsoleColor color, int pixelSize = 3) //инициализация координат
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
            Console.ForegroundColor = Color;    //добавляем цвет пикселя к курсору в консоли

            //цикл для отрисовки пикселя 3х3
            for (int x = 0;x<PixelSize; x++)
            {
                for(int y = 0;y<PixelSize; y++)
                {
                    Console.SetCursorPosition(X*PixelSize+ x, Y*PixelSize+y); //выставляем курсор на позицию х,у и делаем отступ
                    Console.Write(PixelChar);
                }
            }

        }

        public void Clear()  //метод очистки пикселя идентичен отрисовке, вместо отрисовки ставим пробел
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
