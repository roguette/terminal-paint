using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace ConsoleApp2
{
    public class EditMenu : Menu
    {
        public override string title { get; set; } = "Edit";
        public override int width { get; set; } = 27;
        public override int height { get; set; } = 11;
        private void AdjustButtonIndex(int i)
        {
            if (i < 0) i = 7;
            if (i > 7) i = 0;
            activeButtonId = i;
        }

        private void fillWithCurrentColor()
        {
            for (int x = 0; x < Program.image.GetLength(0); x++)
            {
                for (int y = 0; y < Program.image.GetLength(1); y++)
                {
                    Program.image[x, y, 0] = Program.r;
                    Program.image[x, y, 1] = Program.g;
                    Program.image[x, y, 2] = Program.b;
                }
            }
        }

        private void fillWithRandomColors()
        {
            Random random = new Random();
            for (int x = 0; x < Program.image.GetLength(0); x++)
            {
                for (int y = 0; y < Program.image.GetLength(1); y++)
                {
                    Program.image[x, y, 0] = random.Next(0,255);
                    Program.image[x, y, 1] = random.Next(0, 255);
                    Program.image[x, y, 2] = random.Next(0, 255);
                }
            }
        }

        private void invertColors()
        {
            for (int x = 0; x < Program.image.GetLength(0); x++)
            {
                for (int y = 0; y < Program.image.GetLength(1); y++)
                {
                    Program.image[x, y, 0] = 255 - Program.image[x, y, 0];
                    Program.image[x, y, 1] = 255 - Program.image[x, y, 1];
                    Program.image[x, y, 2] = 255 - Program.image[x, y, 2];
                }
            }
        }

        private void removeChannel(string channel)
        {
            for (int x = 0; x < Program.image.GetLength(0); x++)
            {
                for (int y = 0; y < Program.image.GetLength(1); y++)
                {
                    if (channel == "r") Program.image[x, y, 0] = 0;
                    if (channel == "g") Program.image[x, y, 1] = 0;
                    if (channel == "b") Program.image[x, y, 2] = 0;
                }
            }
        }

        private void invertChannel(string channel)
        {
            for (int x = 0; x < Program.image.GetLength(0); x++)
            {
                for (int y = 0; y < Program.image.GetLength(1); y++)
                {
                    if (channel == "r") Program.image[x, y, 0] = 255 - Program.image[x, y, 0];
                    if (channel == "g") Program.image[x, y, 1] = 255 - Program.image[x, y, 1];
                    if (channel == "b") Program.image[x, y, 2] = 255 - Program.image[x, y, 2];
                }
            }
        }
        private void verticalFlip()
        {
            for (int x = 0; x < Program.image.GetLength(0); x++)
            {
                for (int y = 0; y < Program.image.GetLength(1) / 2; y++)
                {
                    int opposite = Program.image.GetLength(1) - y - 1;

                    for (int z = 0; z < 3; z++)
                    {
                        (Program.image[x, y, z], Program.image[x, opposite, z]) = (Program.image[x, opposite, z], Program.image[x, y, z]);
                    }

                }
            }
        }

        private void horizontalFlip()
        {
            for (int x = 0; x < Program.image.GetLength(0) / 2; x++)
            {
                int opposite = Program.image.GetLength(0) - x - 1;
                for (int y = 0; y < Program.image.GetLength(1); y++)
                {

                    for (int z = 0; z < 3; z++)
                    {
                        (Program.image[x, y, z], Program.image[opposite, y, z]) = (Program.image[opposite, y, z], Program.image[x, y, z]);
                    }

                }
            }
        }

        public EditMenu() : base()
        {
            this.Open();

            while (active)
            {

                Console.SetCursorPosition(originX + 2, originY + 1);
                int color = activeButtonId == 0 ? 0 : 255;
                colorText(color, color, color, $"Fill with current color");

                Console.SetCursorPosition(originX + 2, originY + 2);
                color = activeButtonId == 1 ? 0 : 255;
                colorText(color, color, color, $"Fill with random colors");

                Console.SetCursorPosition(originX + 2, originY + 3);
                color = activeButtonId == 2 ? 0 : 255;
                colorText(color, color, color, $"Invert colors");

                Console.SetCursorPosition(originX + 2, originY + 4);
                color = activeButtonId == 3 ? 0 : 255;
                colorText(color, color, color, $"Remove a channel (r/g/b)");

                Console.SetCursorPosition(originX + 2, originY + 5);
                color = activeButtonId == 4 ? 0 : 255;
                colorText(color, color, color, $"Invert a channel (r/g/b)");

                Console.SetCursorPosition(originX + 2, originY + 6);
                color = activeButtonId == 5 ? 0 : 255;
                colorText(color, color, color, $"Horizontal flip");

                Console.SetCursorPosition(originX + 2, originY + 7);
                color = activeButtonId == 6 ? 0 : 255;
                colorText(color, color, color, $"Vertical flip");

                Console.SetCursorPosition(originX + 2, originY + 9);
                color = activeButtonId == 7 ? 0 : 255;
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
                    case ConsoleKey.C:
                        {
                            this.Close();
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            switch (activeButtonId)
                            {
                                case 0:
                                    {
                                        fillWithCurrentColor();
                                        this.Close();
                                        break;
                                    }
                                case 1:
                                    {
                                        fillWithRandomColors();
                                        this.Close();
                                        break;
                                    }
                                case 2:
                                    {
                                        invertColors();
                                        this.Close();
                                        break;
                                    }
                                case 3:
                                    {
                                        TextBoxMenu menu = new TextBoxMenu("Select channel (r/g/b)");
                                        string result = menu.ResultText.ToLower();

                                        if (result == "r" || result == "g" || result == "b")
                                        {
                                            removeChannel(result);
                                        } 
                                        else
                                        {
                                            new MessageMenu("Type in something good for once (r/g/b)");
                                        }

                                        this.Close();
                                        break;
                                    }
                                case 4:
                                    {
                                        TextBoxMenu menu = new TextBoxMenu("Select channel (r/g/b)");
                                        string result = menu.ResultText.ToLower();

                                        if (result == "r" || result == "g" || result == "b")
                                        {
                                            invertChannel(result);
                                        }
                                        else
                                        {
                                            new MessageMenu("Type in something good for once (r/g/b)");
                                        }

                                        this.Close();
                                        break;
                                    }
                                case 5:
                                    {
                                        horizontalFlip();
                                        this.Close();
                                        break;
                                    }
                                case 6:
                                    {
                                        verticalFlip();
                                        this.Close();
                                        break;
                                    }
                            }
                            break;
                        }
                }


            }
            this.Close();
        }
    }
}
