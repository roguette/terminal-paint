using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public class Button
    {
        public int x { get; set; } = 0; 
        public int y { get; set; } = 0;
        public string text { get; set; } = "string.Empty";

        private const string UNDERLINE = "\x1B[4m";
        private const string RESET = "\x1B[0m";

        public void Render()
        {
            (int left, int top) = Console.GetCursorPosition();
            int width = Console.WindowWidth;

            string toPrint = this.text;

            Console.SetCursorPosition(x, y);
            if (left + this.text.Length > width)
            {
                toPrint = this.text.Substring(0, width - x);
            }

            Console.Write(UNDERLINE);
            Console.Write(toPrint[0]);
            Console.Write(RESET);
            Console.Write(toPrint.Substring(1));


        }
        public Button() 
        { 

        }
    }
}
