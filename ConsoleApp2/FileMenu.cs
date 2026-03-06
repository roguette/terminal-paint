using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.Json;

namespace ConsoleApp2
{

    
    public class FileMenu : Menu
    {
        public override string title { get; set; } = "File";
        public override int width { get; set; } = 22;
        public override int height { get; set; } = 10;
        private void AdjustButtonIndex(int i)
        {
            if (i < 0) i = 6;
            if (i > 6) i = 0;
            activeButtonId = i;
        }
        public static void SaveImageToFile(string path)
        {
            Bitmap bmp = new Bitmap(Program.image.GetLength(0), Program.image.GetLength(1));

            for (int x = 0; x < Program.image.GetLength(0); x++)
            {
                for (int y = 0; y < Program.image.GetLength(1); y++)
                {
                    int r = Program.image[x, y, 0];
                    int g = Program.image[x, y, 1];
                    int b = Program.image[x, y, 2];
                    bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            bmp.Save(path);
        }
        public static int[] FlattenImage()
        {
            int width = Program.image.GetLength(0);
            int height = Program.image.GetLength(1);
            int[] arr = new int[width * height * 3];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        arr[(x * height + y) * 3 + c] = Program.image[x, y, c];
                    }
                }
            }

            return arr;
        }
        public static void RestoreFlattenedImage(int[] arr, int width, int height)
        {
            Program.image = new int[width, height * 2, 3];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        Program.image[x, y, c] = arr[(x * height + y) * 3 + c];
                    }
                }
            }
        }
        public static void SaveProject(string path)
        {
            JsonData data = new JsonData()
            {
                width = Program.image.GetLength(0),
                height = Program.image.GetLength(1),
                image = FlattenImage()
            };
            string resultJson = JsonSerializer.Serialize(data);
            File.WriteAllText(path, resultJson);
        }

        public static void LoadProject(string path)
        {
            JsonData resultJson = JsonSerializer.Deserialize<JsonData>(File.ReadAllText(path));
            Console.WindowWidth = resultJson.width;
            Console.WindowHeight = resultJson.height / 2;
            RestoreFlattenedImage(resultJson.image, resultJson.width, resultJson.height);
        }
        public static void LoadImageFromFile(string path)
        {
            // bez using nie można potem zapisać do tego samego pliku
            // to jest takie with open(path) as f: ale w c#
            using (Bitmap bmp = new Bitmap(path))
            {
                int consoleWidth = Console.WindowWidth;
                int consoleHeight = Console.WindowHeight;

                int xScale = bmp.Width / consoleWidth;
                int yScale = (bmp.Height / consoleHeight) / 2;

                Program.image = new int[consoleWidth, consoleHeight * 2, 3];

                for (int x = 0; x < consoleWidth; x++)
                {
                    for (int y = 0; y < consoleHeight * 2; y++)
                    {
                        Color color = bmp.GetPixel(x * xScale, y * yScale);

                        Program.image[x, y, 0] = color.R;
                        Program.image[x, y, 1] = color.G;
                        Program.image[x, y, 2] = color.B;
                    }
                }
            }
        }
        public FileMenu() : base()
        {
            this.Open();

            while (active)
            {
                Console.SetCursorPosition(originX + 2, originY + 1);
                int color = activeButtonId == 0 ? 0 : 255;
                colorText(color, color, color, "New image");

                Console.SetCursorPosition(originX + 2, originY + 2);
                color = activeButtonId == 1 ? 0 : 255;
                colorText(color, color, color, "Open project");

                Console.SetCursorPosition(originX + 2, originY + 3);
                color = activeButtonId == 2 ? 0 : 255;
                colorText(color, color, color, "Save project");

                Console.SetCursorPosition(originX + 2, originY + 4);
                color = activeButtonId == 3 ? 0 : 255;
                colorText(color, color, color, "Import image");

                Console.SetCursorPosition(originX + 2, originY + 5);
                color = activeButtonId == 4 ? 0 : 255;
                colorText(color, color, color, "Export image");

                Console.SetCursorPosition(originX + 2, originY + 7);
                color = activeButtonId == 5 ? 0 : 255;
                colorText(color, color, color, "Exit without saving");

                Console.SetCursorPosition(originX + 2, originY + 8);
                color = activeButtonId == 6 ? 0 : 255;
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
                    case ConsoleKey.Enter:
                        {
                            if (activeButtonId == 0)
                            {
                                ConfirmationMenu menu = new ConfirmationMenu("This will erase everything. Are you sure?");
                                Program.RenderRectangle(menu.originX, menu.originY, menu.width, menu.height);

                                if (menu.ResultOption == "Yes")
                                {
                                    this.Close();
                                    Program.image = new int[Console.WindowWidth, Console.WindowHeight * 2, 3];
                                }
                                else
                                {
                                    this.Open();
                                }
                            }
                            else if (activeButtonId == 1)
                            {
                                ConfirmationMenu menu = new ConfirmationMenu("This will erase everything. Are you sure?");
                                Program.RenderRectangle(menu.originX, menu.originY, menu.width, menu.height);

                                if (menu.ResultOption == "Yes")
                                {
                                    TextBoxMenu textbox = new TextBoxMenu("Provide a filename or a path:");
                                    if (File.Exists(textbox.ResultText))
                                    {
                                        LoadProject(textbox.ResultText);
                                        new MessageMenu("Done loading");

                                    } else
                                    {
                                        new MessageMenu("File does not exist");
                                    }
                                    this.Close();
                                }
                                else
                                {
                                    this.Open();
                                }
                            }
                            else if (activeButtonId == 2)
                            {
                                TextBoxMenu textbox = new TextBoxMenu("Provide a filename or a path:");
                                if (!File.Exists(textbox.ResultText))
                                {
                                    SaveProject(textbox.ResultText);
                                    new MessageMenu($"Saved to {textbox.ResultText}");

                                }
                                else
                                {
                                    ConfirmationMenu overwriteConfirmMenu = new ConfirmationMenu("File already exists. Overwrite?");
                                    if (overwriteConfirmMenu.ResultOption == "Yes")
                                    {
                                        SaveProject(textbox.ResultText);
                                        new MessageMenu($"Saved to {textbox.ResultText}");
                                    }
                                }
                                this.Close();
                            }
                            else if (activeButtonId == 3)
                            {
                                ConfirmationMenu menu = new ConfirmationMenu("This will erase everything. Are you sure?");
                                Program.RenderRectangle(menu.originX, menu.originY, menu.width, menu.height);

                                if (menu.ResultOption == "Yes")
                                {
                                    TextBoxMenu textbox = new TextBoxMenu("Provide a filename or a path:");
                                    if (File.Exists(textbox.ResultText))
                                    {
                                        LoadImageFromFile(textbox.ResultText);
                                        new MessageMenu("Done loading");

                                    }
                                    else
                                    {
                                        new MessageMenu("File does not exist");
                                    }
                                    this.Close();
                                }
                                else
                                {
                                    this.Open();
                                }
                            }
                            else if (activeButtonId == 4)
                            {
                                TextBoxMenu textbox = new TextBoxMenu("Provide a filename or a path:");
                                if (!File.Exists(textbox.ResultText))
                                {
                                    SaveImageToFile(textbox.ResultText);
                                    new MessageMenu($"Saved to {textbox.ResultText}");

                                }
                                else
                                {
                                    ConfirmationMenu overwriteConfirmMenu = new ConfirmationMenu("File already exists. Overwrite?");
                                    if (overwriteConfirmMenu.ResultOption == "Yes")
                                    {
                                        SaveImageToFile(textbox.ResultText);
                                        new MessageMenu($"Saved to {textbox.ResultText}");
                                    }
                                }
                                this.Close();
                            }
                            else if (activeButtonId == 5)
                            {
                                Environment.Exit(0);
                            }
                            else if (activeButtonId == 6)
                            {
                                this.Close();
                            }
                            break;
                        }
                    case (ConsoleKey.C):
                    {
                        this.Close();
                        break;
                    }
                }

            }
            this.Close();
        }
        
    }
}
