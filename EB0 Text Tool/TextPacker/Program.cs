using System;

namespace TextPacker
{
    class Program
    {
        static void Main(string[] args)
        {
            var romPath = string.Empty;
            Console.Title = "EarthBound Zero Text Tool";

            if (args is null)
            {
                //If nothing comes up, ask for a filename
                Console.WriteLine("Please type the filename of the ROM you want to open.");
                Console.WriteLine("(For future reference, you can drag and drop it or use command line arguments!)");

                romPath = Console.ReadLine();
            }

            romPath = ROM_IO.FileExists(romPath);

            if (romPath.StartsWith("ERROR")) //existence check
            {
                Console.WriteLine(romPath);
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Dump text or insert text? d/i");
            var input = Console.ReadLine();

            if (input.ToLower().StartsWith("d"))
            {
                Console.WriteLine("Dumping text...");
            }
            else //just hitting enter will insert, since that's probably what people'll be doing 99% of the time they use this tool
            {
                Console.WriteLine("Inserting text...");
                //TODO: Make ROM-writing class
            }
        }
    }
}
