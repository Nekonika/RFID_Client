namespace RFID_Client
{
    internal static class DisplayHelper
    {
        public const int ConsoleWidth = 40;
        public const int ConsoleHeight = 30;

        private static List<List<Letter>> _FrameBuffer = [];

        static DisplayHelper()
        {
            SetBuffer(GetEmptyFrameBuffer());
        }

        public static List<List<Letter>> GetEmptyFrameBuffer()
        {
            List<List<Letter>> Buffer = [];

            for (int y = 0; y < ConsoleHeight; y++)
            {
                List<Letter> Line = [];
                for (int x = 0; x < ConsoleWidth; x++)
                    Line.Add(new Letter());

                Buffer.Add(Line);
            }

            return Buffer;
        }

        public static void BufferToScreen(bool forceOverwrite = false)
        {
            for (int y = 0; y < ConsoleHeight; y++)
            {
                for (int x = 0; x < ConsoleWidth; x++)
                {
                    Letter Letter = _FrameBuffer[y][x];
                    if (!forceOverwrite && !Letter.Changed) continue;
                    Letter.WriteToScreen(x, y);
                }
            }

            Console.SetCursorPosition(0, ConsoleHeight);
        }

        public static Letter GetLetter(int x, int y) => _FrameBuffer[y][x];
        public static void SetLetter(Letter letter, int x, int y)
            => _FrameBuffer[y][x] = letter;

        public static List<List<Letter>> GetBuffer() => _FrameBuffer;
        public static void SetBuffer(List<List<Letter>> buffer)
            => _FrameBuffer = buffer;

        public static ConsoleColor GetForegroundColor(int x, int y) => _FrameBuffer[y][x].ForegroundColor;
        public static void SetForegroundColor(ConsoleColor fgColor)
        {
            for (int y = 0; y < ConsoleHeight; y++)
                for (int x = 0; x < ConsoleWidth; x++)
                    SetForegroundColor(fgColor, x, y);
        }
        public static void SetForegroundColor(ConsoleColor fgColor, int x, int y)
        {
            Letter Letter = _FrameBuffer[y][x];
            if (Letter.ForegroundColor != fgColor)
            { 
                Letter.ForegroundColor = fgColor;
                Letter.Changed = true;
            }
        }

        public static ConsoleColor GetBackgroundColor(int x, int y) => _FrameBuffer[y][x].BackgroundColor;
        public static void SetBackgroundColor(ConsoleColor bgColor)
        {
            for (int y = 0; y < ConsoleHeight; y++)
                for (int x = 0; x < ConsoleWidth; x++)
                    SetBackgroundColor(bgColor, x, y);
        }
        public static void SetBackgroundColor(ConsoleColor bgColor, int x, int y)
        {
            Letter Letter = _FrameBuffer[y][x];
            if (Letter.BackgroundColor != bgColor)
            {
                Letter.BackgroundColor = bgColor;
                Letter.Changed = true;
            }
        }

        public static void TextToBuffer(string text)
        {
            string[] Lines = text.Split(Environment.NewLine);
            for (int y = 0; y < ConsoleHeight; y++)
            {
                if (y >= Lines.Length) continue;
                string Line = Lines[y];

                for (int x = 0; x < ConsoleWidth; x++)
                {
                    char Char = Line.Length > x ? Line[x] : ' ';
                    Letter PrevLetter = GetLetter(x, y);
                    Letter Letter = new(ConsoleColor.White, ConsoleColor.Black, Char);
                    if (!PrevLetter.Equals(Letter)) SetLetter(Letter, x, y);
                }
            }
        }

        internal class Letter
        {
            public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;
            public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
            public char Char { get; set; } = ' ';
            public bool Changed { get; set; } = true;

            public Letter() { }
            public Letter(bool changed) { Changed = changed; }
            public Letter(ConsoleColor foregroundColor, ConsoleColor backgroundColor, char chr, bool changed = true)
            {
                ForegroundColor = foregroundColor;
                BackgroundColor = backgroundColor;
                Char = chr;
                Changed = changed;
            }

            public void WriteToScreen(int x, int y)
            {
                if (!Changed) return;

                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = ForegroundColor;
                Console.BackgroundColor = BackgroundColor;
                Console.Write(Char);

                Changed = false;
            }

            public override bool Equals(object? obj)
            {
                if (obj is Letter Cmp)
                { 
                    return ForegroundColor == Cmp.ForegroundColor && 
                        BackgroundColor == Cmp.BackgroundColor && 
                        Char == Cmp.Char;
                }

                return base.Equals(obj);
            }

            public override int GetHashCode()
                => throw new NotImplementedException();
        }

        public static class Screens
        {
            public static string SearchingServer =>
                """
                


                
                            | SUCHE SERVER |
                            | BITTE WARTEN |
                                                        
                           
                            
                                        
                           :=+*#%%@@@@%%#*+=:           
                       -*%@@%*+=--::::--=+*%@@%*=.      
                    -#@@@+:                  :+%@@#:    
                  =%@%+:          .....         .+%@%=  
                 .%#-      .-*#@@@@@@@@@@#*=.      -#%: 
                        :+@@@*=-:.    ..-=*@@%+:        
                       #@%+:                :+@@%       
                       -=       .-=++=-.       --       
                             -#@@@%##%@@@*=             
                            *@#-.      .=%@#            
                                                        
                                 .+##*-                 
                                 %@@@@@-                
                                 %@@@@@:                
                                  -++=.                 
                                                        
                                    
                                  

                
                """;
            
            public static string AwaitingRfid =>
                """    


                           | WARTE AUF RFID |
                
                           

                         ..                  ...        
                      .-===                  ====.      
                    .-===:   .            .   :====.    
                   :===:  .-===          -===:  .===-   
                  -==-  .-===:            .-===.  -==-  
                 :==-  :===:  .-==    ==-:  .===-  :=== 
                .===  .===   ====:    :-===.  ===:  ===:
                -==:  ===. .===:  .:::  .===.  ===  .==-
                ===. .===  :==-  -=====  :===  -==:  ===
                ===. .===  :==-  -=====  :===  -==:  ===
                -==:  ===. .===:  .:::  .===.  ===  .==-
                .===  .===   ====:    :-===.  ===:  ===:
                 :==-  :===:  .-==    -=-:  .===:  :==- 
                  -==-  .-===:            :====.  -==-  
                   :===:  .-===          -==-.  .===-   
                    .-===:   .            .   :====.    
                      .-===.                 ===-.      
                         ..                  ..         

                




                """;
        }
    }
}
