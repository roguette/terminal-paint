using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public class ConfirmationMenu : Menu
    {
        public override string title { get; set; } = "Confirm pls";
        public override int width { get; set; } = 10;
        public override int height { get; set; } = 6;
        public bool IsResultReady { get; set; } = false;
        public string ResultOption = "";
        private void AdjustButtonIndex(int i)
        {
            activeButtonId = (i + 2) % 2;
        }
        public ConfirmationMenu(string text) : base()
        {
            this.width = text.Length + 4;
            this.Open();

            while (active)
            {
                Console.SetCursorPosition(originX + 2, originY + 1);
                Console.Write(text);

                Console.SetCursorPosition(originX + 2, originY + 3);
                int color = activeButtonId == 0 ? 0 : 255;
                colorText(color, color, color, "Yes");

                Console.SetCursorPosition(originX + 2, originY + 4);
                color = activeButtonId == 1 ? 0 : 255;
                colorText(color, color, color, "No");

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            AdjustButtonIndex(0);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            AdjustButtonIndex(1);
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            if (activeButtonId == 0) {
                                this.ResultOption = "Yes";
                                this.IsResultReady = true;
                            } 
                            else
                            {
                                this.ResultOption = "No";
                                this.IsResultReady = true;
                            }
                            this.active = false;
                            break;
                        }
                }


            }
            this.Close();
        }
    }
}
