using System;
using System.Collections.Generic;

namespace Snake_Console
{
    public class Snake
    {
        private readonly ConsoleColor headColor;    //цвет головы
        private readonly ConsoleColor bodyColor;    //цвет тела

        public Snake(int initialX, int initialY, 
            ConsoleColor headColor,
            ConsoleColor bodyColor, 
            int bodyLength = 3)                     //конструктор змеи
        {
            this.headColor = headColor;             //цвета
            this.bodyColor = bodyColor;

            Head = new Pixel(initialX, initialY, headColor);    //инициализация головы
            
            for(int i = bodyLength; i>=0; i--)      //добавляем пиксели начиная с хвоста
            {
                Body.Enqueue(new Pixel(Head.X - i - 1, initialY, bodyColor));   //Enqeue добавляют элименты в очередь
            }

            Draw(); //отрисовка
        }

        public Pixel Head { get; private set; } //добавляем пиксель головы

        public Queue<Pixel> Body { get; } = new Queue<Pixel>(); //добавляем очередь тела

        public void Move(Direction direction, bool eat = false)
        {
            Clear();

            //работа метода: очищаем экран, добавляем пиксель в тело на месте головы, удаляем пиксель из очереди, а точнее из хвоста и бегаем в зависимости от направления

            Body.Enqueue(new Pixel(Head.X, Head.Y, bodyColor));

            if(!eat)Body.Dequeue();

            Head = direction switch
            {
                Direction.Right => new Pixel(Head.X + 1, Head.Y, headColor),
                Direction.Left => new Pixel(Head.X - 1, Head.Y, headColor),
                Direction.Up => new Pixel(Head.X, Head.Y - 1, headColor),
                Direction.Down => new Pixel(Head.X, Head.Y + 1, headColor),
                _ => Head
            };

            Draw();

        }

        public void Draw()  //добавление змеи на экран
        {
            Head.Draw();
            foreach(Pixel pixel in Body)
            {
                pixel.Draw();
            }
        }

        public void Clear() //очистка змеи
        {
            Head.Clear();
            foreach (Pixel pixel in Body)
            {
                pixel.Clear();
            }
        }

    }
}
