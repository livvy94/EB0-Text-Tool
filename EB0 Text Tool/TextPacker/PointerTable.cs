using System;
using System.Collections.Generic;

namespace TextPacker
{
    public class PointerTable
    {
        const byte iNES_HEADER_LENGTH = 0x10;
        const int POINTER_TABLE_START = 0x030000 + iNES_HEADER_LENGTH; //Ends at 0x3177F
        const int SCRIPT_TEXTBANK_START = 0x060000 + iNES_HEADER_LENGTH;
        const int SCRIPT_TEXTBANK_END = 0x074000 + iNES_HEADER_LENGTH; //or possibly 070000. Due to the calculation truncating the leftmost byte, it's probably impossible to go any higher...
        const int NAMES_TEXTBANK_END = 0x09CC + iNES_HEADER_LENGTH; //This text block starts at 0x00 and ends here. TODO: double-check the value

        public static byte[] Generate(List<byte[]> messages)
        {
            var offsets = CalculateDialogOffsets(messages);

            var result = new List<byte>();
            foreach (var offset in offsets)
                result.AddRange(CalculatePointer(offset));

            return result.ToArray();
        }

        private static List<int> CalculateDialogOffsets(List<byte[]> messages)
        {
            //text lengths -> the offsets to pass into Generate
            var offsets = new List<int>();

            var currentOffset = SCRIPT_TEXTBANK_START;

            foreach (var message in messages)
            {
                offsets.Add(currentOffset);
                currentOffset += message.Length;

                var remainingFreeSpace = SCRIPT_TEXTBANK_END - currentOffset;
                if (remainingFreeSpace < 0)
                    throw new Exception("There's too much text data! It'd overwrite the sprites from the ending cutscene if it keeps going.");
            }

            return offsets;
        }

        public static byte[] CalculatePointer(int offset)
        {
            offset -= iNES_HEADER_LENGTH;

            if (offset < NAMES_TEXTBANK_END)
                offset += 0x8000;

            var result = BitConverter.GetBytes(offset);
            return new byte[] { result[0], result[1], 0x00 };
        }
    }
}
