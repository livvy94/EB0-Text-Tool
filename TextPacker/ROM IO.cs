using System;
using System.Collections.Generic;
using System.IO;

namespace TextPacker
{
    class ROM_IO
    {
        public const byte iNES_HEADER_LENGTH = 0x10;

        internal static string FileExists(string romPath)
        {
            //Check as a full path
            if (File.Exists(romPath))
                return romPath;

            //Check as [Current Directory]/romPath
            var longPath = Path.Combine(Directory.GetCurrentDirectory(), romPath);
            if (File.Exists(longPath))
                return longPath;

            return "ERROR! Couldn't find " + romPath;
        }

        internal byte[] LoadROM(string romPath)
        {
            //Crazy notion - separate the iNES Header and the rest of the ROM so I don't have to keep adding and subtracting its length from stuff
            var ROM = File.ReadAllBytes(romPath);
            var HeaderlessROM = new byte[ROM.Length - iNES_HEADER_LENGTH];
            Array.Copy(ROM, 0x10, HeaderlessROM, 0, HeaderlessROM.Length);
            return HeaderlessROM; //I wonder if this will work lol
        }

        internal List<byte[]> LoadPointerTable(byte[] ROM)
        {
            //go to 030000
            //read up to the end
            var PointerTableContents = new List<byte[]>();
            for (int i = 0x030000 + iNES_HEADER_LENGTH; i < 0x3176F + iNES_HEADER_LENGTH; i += 3)
            {
                PointerTableContents.Add(new byte[] { ROM[i], ROM[i + 1], ROM[i + 2] });
            }
            return PointerTableContents;
        }
    }
}
