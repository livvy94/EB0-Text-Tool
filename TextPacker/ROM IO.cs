using System;
using System.Collections.Generic;
using System.IO;

namespace TextPacker
{
    class ROM_IO
    {
        private const byte iNES_HEADER_LENGTH = 0x10;

        internal static List<uint> LoadTextPointerTable(byte[] ROM)
        {
            var PointerTableContents = new List<uint>();
            for (int i = PointerTable.POINTER_TABLE_START; i < PointerTable.POINTER_TABLE_END; i += 3)
            {
                var currentPointer = PointerTable.BytesToOffset(ROM[i], ROM[i + 1], 0x06 );
                PointerTableContents.Add(currentPointer);
            }

            return PointerTableContents;
        }


        internal static byte[] LoadROM(string romPath)
        {
            //Crazy notion - separate the iNES Header and the rest of the ROM so I don't have to keep adding and subtracting its length from stuff
            var ROM = File.ReadAllBytes(romPath);
            var HeaderlessROM = new byte[ROM.Length - iNES_HEADER_LENGTH];
            Array.Copy(ROM, 0x10, HeaderlessROM, 0, HeaderlessROM.Length); //Is there a better technique for removing the first 0x10 bytes of an array? I wouldn't need "using System" if not for this line
            return HeaderlessROM; //I wonder if this will work lol
        }

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
    }
}
