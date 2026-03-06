using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ConsoleApp2
{
    public abstract class Menu
    {
        public bool active = false;
        public abstract int width { get; set; }
        public abstract int height { get; set; }
        public abstract string title { get; set; }

        public int originX = 0;
        public int originY = 0;
        public int activeButtonId = 0;
        protected void colorText(int fr, int fg, int fb, string text)
        {
            Console.Write($"\x1b[38;2;{fr};{fg};{fb}m\x1b[48;2;{255 - fr};{255 - fg};{255 - fb}m{text}\x1b[0m");
        }
        private void DrawRectangle(int x, int y, int width, int height)
        {

            Console.SetCursorPosition(x, y);
            Console.Write("┌");
            for (int i = 0; i < width - 2; i++) Console.Write("─");
            Console.Write("┐");
            for (int i = 0; i < height - 2; i++)
            {
                Console.SetCursorPosition(x, y + i + 1);
                Console.Write("|");
                for (int j = 0; j < width - 2; j++) Console.Write(" ");
                Console.Write("|");
            }

            Console.SetCursorPosition(x, y + height - 1);
            Console.Write("└");
            for (int i = 0; i < width - 2; i++) Console.Write("─");
            Console.Write("┘");

        }


        public void Open()
        {
            this.active = true;
            int x = Console.WindowWidth / 2;
            int y = Console.WindowHeight / 2;



            x = x - width / 2;
            y = y - height / 2;

            this.originX = x;
            this.originY = y;
            
            DrawRectangle(x, y, width, height);

            // górna krawędź
            int centerX = Console.WindowWidth / 2;
            Console.SetCursorPosition(centerX - title.Length / 2, y);
            Console.Write(title);
        }


        public void Close(Action? onClosed = null)
        {
            this.active = false;
            onClosed?.Invoke();
        }
    }
}
