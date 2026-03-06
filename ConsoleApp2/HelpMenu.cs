using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public class HelpMenu : Menu
    {
        public override string title { get; set; } = "Help";
        public override int width { get; set; } = Console.WindowWidth-5;
        public override int height { get; set; } = Console.WindowHeight-3;
        private void fancyWrite(int fr, int fg, int fb, string text)
        {
            Console.Write($"\x1b[38;2;{fr};{fg};{fb}m\x1b[48;2;0;0;0m{text}\x1b[0m");
        }
        private void WriteFancyText(string text)
        {
            (int left, int top) = Console.GetCursorPosition();
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (c == ' ')
                {
                    fancyWrite(0, 0, 0, " ");
                }
                else if (c == '~')
                {
                    fancyWrite(19, 97, 8, "~");
                }
                else if (c == '@')
                {
                    fancyWrite(138, 206, 0, "@");
                }
            }
            Console.SetCursorPosition(left, top + 1);
        }
        public HelpMenu()
        {
            this.Open();
            Console.SetCursorPosition(4, 3);
            WriteFancyText("                                        ~                          ~~~~~                                         ");
            WriteFancyText("                                      ~~                             ~~~                                         ");
            WriteFancyText("                                   ~~~   @@                           ~~~                                        ");
            WriteFancyText("                                  ~     @@@@@        @@@@ @@           ~~                                        ");
            WriteFancyText("                    @@@@@@@@@         @@@   @@          @ @@@@@@        ~~~~~~~ ~~  ~                            ");
            WriteFancyText("               ~   @@       @@        @       @            @   @@        ~ ~~ ~ ~ ~~~~~~~ ~~~~~~~~               ");
            WriteFancyText("             ~~    @         @       @@       @           @             @@@                  ~~~~~~~~~~~~~~~~~~~ ");
            WriteFancyText("          ~~~       @@      @@       @@@@@@@@@@@          @            @@@@    @@@                       ~~~~~~  ");
            WriteFancyText("      ~~~~           @@@@@@@        @@         @         @@           @@ @@   @@@    @@@@ @             ~~~~~    ");
            WriteFancyText("  ~~~~~               @@            @          @@        @           @@  @@   @@@       @@@@@ @      ~~~~~ ~     ");
            WriteFancyText("~~~~                   @@          @            @        @          @@    @   @@        @@@ @@@@@  ~ @@ ~        ");
            WriteFancyText("~~~~~~~~~~~~~~~~~~~~ ~  @@        @@             @    @@@@@@@      @@@   @@@ @@         @@       ~~~@@           ");
            WriteFancyText("                  ~~~~   @@       @              @     @ @@@@@@@   @@      @@@         @@       ~ @@@            ");
            WriteFancyText("                                                                           @@         @@      ~ ~@@              ");
            WriteFancyText("                               ~~                                          @          @         @                ");
            WriteFancyText("                              ~~                       ~~                            @@                          ");
            WriteFancyText("                              ~~               ~ ~~~~~~~                       ~~            @@@                 ");
            WriteFancyText("                             ~~          ~ ~~ ~~ ~      ~~~                     ~~            @@                 ");
            WriteFancyText("                            ~~       ~ ~~~~~               ~ ~ ~~~               ~~                              ");
            WriteFancyText("                            ~  ~~~~ ~~~                         ~ ~~~             ~~~                            ");
            WriteFancyText("                           ~ ~~~~                                  ~ ~~ ~          ~~~                           ");
            WriteFancyText("                          ~~~~                                        ~~~~ ~~       ~~~                          ");
            WriteFancyText("                                                                           ~~~~~ ~    ~~                         ");
            WriteFancyText("                                                                               ~~~~~~  ~~                        ");
            WriteFancyText("                                                                                    ~~~~~                        ");

            Console.SetCursorPosition(6, 17);
            Console.Write("1. Arrows to move paintbrush");

            Console.SetCursorPosition(6, 18);
            Console.Write("2. Enter to draw");

            Console.SetCursorPosition(6, 19);
            Console.Write("3. Look at underlined letters for hotkeys");

            Console.SetCursorPosition(6, 20);
            Console.Write("4. up/down arrows to navigate menus");

            Console.SetCursorPosition(6, 21);
            Console.Write("5. left/right arrows to interact with sliders");

            Console.SetCursorPosition(6, 23);
            fancyWrite(12, 255, 255, "ENTER TO RETURN");

            while (active)
            { 
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    this.Close();
                }
            }
        }
    }
}
