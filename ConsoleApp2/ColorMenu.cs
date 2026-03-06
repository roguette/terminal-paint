using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConsoleApp2
{
    public class ColorMenu : Menu 
    {
        public override string title { get; set; } = "Color Picker";
        public override int width { get; set; } = 20;
        public override int height { get; set; } = 10;
        private void AdjustButtonIndex(int i)
        {
            if (i < 0) i = 4;
            if (i > 4) i = 0;
            activeButtonId = i;
        }
        public ColorMenu(ref int original_r, ref int original_g, ref int original_b) : base(){
            this.Open();

            int new_r = original_r;
            int new_g = original_g;
            int new_b = original_b;
            

            while (active) { 

                // R range
                Console.SetCursorPosition(originX + 2, originY + 1);
                int color = activeButtonId == 0 ? 0 : 255;
                colorText(color, color, color, $"R: < {new_r} >");
                Console.Write("  ");

                Console.SetCursorPosition(originX + 2, originY + 2);
                color = activeButtonId == 1 ? 0 : 255;
                colorText(color, color, color, $"G: < {new_g} >");
                Console.Write("  ");

                Console.SetCursorPosition(originX + 2, originY + 3);
                color = activeButtonId == 2 ? 0 : 255;
                colorText(color, color, color, $"B: < {new_b} >");
                Console.Write("  ");

                Console.SetCursorPosition(originX + 2, originY + 5);
                colorText(new_r, new_g, new_b, "█████████");

                Console.SetCursorPosition(originX + 2, originY + 7);
                color = activeButtonId == 3 ? 0 : 255;
                colorText(color, color, color, "Save color");

                Console.SetCursorPosition(originX + 2, originY + 8);
                color = activeButtonId == 4 ? 0 : 255;
                colorText(color, color, color, "\u001b[4mC\u001b[0mancel");




                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            AdjustButtonIndex(activeButtonId - 1);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            AdjustButtonIndex(activeButtonId + 1);
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (activeButtonId == 0) new_r -= 1;
                            if (activeButtonId == 1) new_g -= 1;
                            if (activeButtonId == 2) new_b -= 1;
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (activeButtonId == 0) new_r += 1;
                            if (activeButtonId == 1) new_g += 1;
                            if (activeButtonId == 2) new_b += 1;
                            break;
                        }
                    case ConsoleKey.C:
                        {
                            this.Close();
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            if (activeButtonId == 3)
                            {
                                original_r = new_r;
                                original_g = new_g;
                                original_b = new_b;
                                this.Close();
                            }
                            if (activeButtonId == 4)
                            {
                                this.Close();
                            }
                            break;
                        }

                }


                new_r = Math.Clamp(new_r, 0, 255);
                new_g = Math.Clamp(new_g, 0, 255);
                new_b = Math.Clamp(new_b, 0, 255);

            }
            this.Close();
        }
        
    }

}
