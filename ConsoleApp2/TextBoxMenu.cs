using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public class TextBoxMenu : Menu
    {
        public override string title { get; set; } = "U better start typing";
        public override int width { get; set; } = 20;
        public override int height { get; set; } = 5;
        public bool IsResultReady { get; set; } = false;
        public string ResultText = "";

        public TextBoxMenu(string text) : base()
        {
            this.width = text.Length + 8;
            this.Open();

            Console.SetCursorPosition(originX + 2, originY + 1);
            Console.Write(text);

            Console.SetCursorPosition(originX + 2, originY + 3);
            string input = Console.ReadLine();
            if (input != null)
            {
                input = input.Trim();
                this.ResultText = input;
                this.IsResultReady = true;
            }


            this.Close();
        }
    }
}
