using System;
using System.Collections.Generic;

namespace TextPacker
{
    class Program
    {
        static void Main(string[] args)
        {
            var debugMode = true;
            var debugPath = @"C:\Users\vincents\Desktop\Git repos\EB0-Text-Tool\Test.nes";

            var romPath = string.Empty;
            Console.Title = "EarthBound Zero Text Tool";

            if (args.Length == 0)
            {
                //TODO: Use the last ROM path? y/n

                if (debugMode)
                    romPath = debugPath;
                else
                {
                    //If nothing comes up, ask for a filename
                    Console.WriteLine("Please type the filename of the ROM you want to open.");
                    Console.WriteLine("(For future reference, you can drag and drop it or use command line arguments!)");
                    romPath = Console.ReadLine();
                }
            }

            romPath = ROM_IO.FileExists(romPath);
            if (romPath.StartsWith("ERROR"))
            {
                Console.WriteLine(romPath);
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Dump text or insert text?");
            var input = Console.ReadLine();

            if (input.ToLower().StartsWith("d"))
            {
                var ROM = ROM_IO.LoadROM(romPath);
                Console.WriteLine("Loading text pointer table...");

                var textPointers = ROM_IO.LoadTextPointerTable(ROM);
                if (debugMode) PrintPointers(textPointers);

                var script = TextConversion.LoadScript(ROM, textPointers);
                if (debugMode) PrintScript(script);



            }
            else
            {
                Console.WriteLine("Inserting text...");
                //TODO: Make textfile-parsing stuff
                //TODO: Make ROM-writing stuff
            }
        }

        ////////////////////////////////////////////////////////////////////////
        private static void PrintScript(List<string> script)
        {
            int i = 0;
            foreach (var line in script)
            {
                Console.WriteLine();
                var number = '-' + i.ToString("X") + ':';
                Console.WriteLine(number.PadRight(21, '-'));
                Console.WriteLine(line);
                i++;
            }
        }

        static void PrintPointers(List<uint> textPointers)
        {
            var currentPrintColumn = 0;
            foreach (var pointer in textPointers)
            {
                if (pointer == 0x060000) Console.Write("------ "); //there's a surprising amount of unused pointer table entries
                else Console.Write(pointer.ToString("X6") + ' ');

                currentPrintColumn += 1;
                if (currentPrintColumn == 16)
                {
                    currentPrintColumn = 0;
                    Console.Write("\r\n");
                }
            }
        }
    }
}
