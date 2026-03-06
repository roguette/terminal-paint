using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public class MessageMenu : Menu
    {
        public override string title { get; set; } = "HELLO";
        public override int width { get; set; } = 20;
        public override int height { get; set; } = 5;

        public MessageMenu(string text) : base()
        {
            this.width = text.Length + 4;
            this.Open();

            while (active)
            {
                Console.SetCursorPosition(originX + 2, originY + 1);
                Console.Write(text);

                Console.SetCursorPosition(originX + 2, originY + 3);
                int color = activeButtonId == 0 ? 0 : 255;
                colorText(color, color, color, "Ok");

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        {
                            this.active = false;
                            break;
                        }
                }


            }
            this.Close();
        }
    }
}
