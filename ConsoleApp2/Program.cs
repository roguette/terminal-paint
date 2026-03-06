using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleApp2
{
    internal class Program
    {
        public static int[,,] image = new int[Console.WindowWidth, Console.WindowHeight * 2, 3];

        public static int r = 255;
        public static int g = 255;
        public static int b = 0;

        public static int x = Console.WindowWidth / 2;
        public static int y = Console.WindowHeight / 2;

        static void SettleButtons(List<Button> buttons)
        {
            int currentX = 0;
            for (int i = 0; i < buttons.Count; i++)
            {
                Console.SetCursorPosition(currentX, 0);
                Button b = buttons[i];

                (int left, int top) = Console.GetCursorPosition();

                b.x = left;
                b.y = top;
                currentX += b.text.Length + 1;

            }
        }
        static void RenderButtons(List<Button> buttons)
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < buttons.Count; i++)
            {
                Button b = buttons[i];
                b.Render();
            }
        }
        static void ScaleImage(int newX, int newY)
        {
            int[,,] temp = new int[newX, newY, 3];

            int oldX = image.GetLength(0);
            int oldY = image.GetLength(1);

            int maxX = Math.Min(oldX, newX);
            int maxY = Math.Min(oldY, newY);

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    temp[x, y, 0] = image[x, y, 0];
                    temp[x, y, 1] = image[x, y, 1];
                    temp[x, y, 2] = image[x, y, 2];
                }
            }

            image = temp;
        }
        static void BgFgCharacter(int fr, int fg, int fb, int br, int bg, int bb, string character)
        {
            Console.Write($"\x1b[38;2;{fr};{fg};{fb}m\x1b[48;2;{br};{bg};{bb}m{character}\x1b[0m");
        }
        static void RenderColorSwatch(int r, int g, int b)
        {
            for (int i = Console.WindowWidth - 1; i >= Console.WindowWidth - 5; i--)
            {
                Console.SetCursorPosition(i, 0);
                BgFgCharacter(r, g, b, r, g, b, " ");
            }
        }
        static void RenderCursor()
        {
            Console.SetCursorPosition(x, y / 2);
            if (y % 2 == 1)
            {
                BgFgCharacter(r, g, b, 255 - r, 255 - g, 255 - b, "▄");
            }
            else
            {
                BgFgCharacter(r, g, b, 255 - r, 255 - g, 255 - b, "▀");
            }
        }
        public static void RenderImage()
        {
            int windowWidth = Console.WindowWidth;
            int windowHeight = Console.WindowHeight;

            int oldX = image.GetLength(0);
            int oldY = image.GetLength(1);

            int maxX = Math.Min(oldX, windowWidth);
            int maxY = Math.Min(oldY, windowHeight * 2);

            for (int ty = 0; ty < maxY / 2; ty++)
            {
                for (int tx = 0; tx < maxX; tx++)
                {
                    int topY = ty * 2;
                    int bottomY = topY + 1;

                    if (tx >= windowWidth || ty >= windowHeight)
                        continue;

                    int tr = image[tx, topY, 0];
                    int tg = image[tx, topY, 1];
                    int tb = image[tx, topY, 2];

                    int br = image[tx, bottomY, 0];
                    int bg = image[tx, bottomY, 1];
                    int bb = image[tx, bottomY, 2];
                    Console.SetCursorPosition(tx, ty);
                    BgFgCharacter(tr, tg, tb, br, bg, bb, "▀");
 
                }
            }
        }

        public static void RenderRectangle(int x, int y, int width, int height)
        {

            for (int ty = y; ty < y + height; ty++)
            {
                for (int tx = x; tx < x + width; tx++)
                {
                    int topY = ty * 2;
                    int bottomY = topY + 1;

                    int tr = image[tx, topY, 0];
                    int tg = image[tx, topY, 1];
                    int tb = image[tx, topY, 2];

                    int br = image[tx, bottomY, 0];
                    int bg = image[tx, bottomY, 1];
                    int bb = image[tx, bottomY, 2];

                    Console.SetCursorPosition(tx, ty);
                    BgFgCharacter(tr, tg, tb, br, bg, bb, "▀");
                }
            }
        }

        static void RenderImagePixel(int tx, int ty)
        {

            int topY = ty * 2;
            int bottomY = topY + 1;
            
            int tr = image[tx, topY, 0];
            int tg = image[tx, topY, 1];
            int tb = image[tx, topY, 2];

            int br = image[tx, bottomY, 0];
            int bg = image[tx, bottomY, 1];
            int bb = image[tx, bottomY, 2];

            Console.SetCursorPosition(tx, ty);
            BgFgCharacter(tr, tg, tb, br, bg, bb, "▀");

        }

        static void ClearScreen()
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.Write("\x1b[38;2;0;0;0m\x1b[48;2;0;0;0m                                                                                                                                                                    \x1b[0m");
            }
            Console.SetCursorPosition(0, 0);
        } 
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            ClearScreen();

            List<Button> buttons = new List<Button>();

            buttons.Clear();
            buttons.Add(new Button(){text = "File"});
            buttons.Add(new Button(){text = "Edit"});
            buttons.Add(new Button(){text = "Help"});
            buttons.Add(new Button(){text = "Color"});

            SettleButtons(buttons);

            int lastWidth = Console.WindowWidth;
            int lastHeight = Console.WindowHeight;
            RenderButtons(buttons);
            while (true)
            {
                RenderCursor();

                if (Console.KeyAvailable)
                {
                    if (y <= 1)
                    {
                        RenderRectangle(0, 0, Console.WindowWidth, 2);
                    }
                    else
                    {
                        RenderColorSwatch(r, g, b);
                        RenderButtons(buttons);
                    }
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    RenderImagePixel(x, y / 2);

                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow: { y -= 1; break; }
                        case ConsoleKey.DownArrow: { y += 1; break; }
                        case ConsoleKey.LeftArrow: { x -= 1; break; }
                        case ConsoleKey.RightArrow: { x += 1; break; }
                        case ConsoleKey.Enter:
                            {

                                image[x, y, 0] = r;
                                image[x, y, 1] = g;
                                image[x, y, 2] = b;

                                //RenderImage(image);
                                break;
                            }
                        case ConsoleKey.C:
                            {
                                new ColorMenu(ref r, ref g, ref b);
                                RenderImage();
                                break;
                            }
                        case ConsoleKey.F:
                            {
                                new FileMenu();
                                RenderImage();
                                break;
                            }
                        case ConsoleKey.E:
                            {
                                new EditMenu();
                                RenderImage();
                                break;
                            }
                        case ConsoleKey.H:
                            {
                                if (Console.WindowWidth < 30 || Console.WindowHeight < 30)
                                {
                                    new MessageMenu("The window is too small to display help");
                                } 
                                else
                                {
                                    new HelpMenu();
                                }
                                RenderImage();
                                break;
                            }
                    }



                    if (y > Console.WindowHeight * 2 - 1)
                    {
                        y = Console.WindowHeight * 2 - 1;
                    }
                    else if (y < 0)
                    {
                        y = 0;
                    }
                    else if (x > Console.WindowWidth - 1)
                    {
                        x = Console.WindowWidth - 1;
                    }
                    else if (x < 0)
                    {
                        x = 0;
                    }

                }
                else
                {
                    if (y > 1)
                    {
                        RenderButtons(buttons);
                    }
                }


                if (Console.WindowWidth != lastWidth ||
                    Console.WindowHeight != lastHeight)
                {
                    int width = Console.WindowWidth;
                    int height = Console.WindowHeight;

                    while (true)
                    {
                        Thread.Sleep(500);
                        int tempWidth = Console.WindowWidth;
                        int tempHeight = Console.WindowHeight;

                        if (tempWidth != width || tempHeight != height) 
                        {
                            width = tempWidth;
                            height = tempHeight;
                        } else
                        {
                            break;
                        }
                    }
                    lastWidth = Console.WindowWidth;
                    lastHeight = Console.WindowHeight;

                    ClearScreen();
                    ScaleImage(lastWidth, lastHeight*2);
                    RenderImage();
                }
                
                


                Thread.Sleep(16);

            }
            
        }
    }
}
