using System;
using System.Collections.Generic;

namespace EB0_Text_Tool
{
    public class TextPointerTable
    {
        const int TABLE_OFFSET = 0x030010; //Ends at 0x3177F

        public static byte[] Generate(List<byte[]> messages)
        {
            var offsets = CalculateOffsets(messages);

            var result = new List<byte>();
            foreach (var offset in offsets)
                result.AddRange(CalculatePointer(offset));

            return result.ToArray();
        }

        private static List<int> CalculateOffsets(List<byte[]> messages)
        {
            //text lengths -> the offsets to pass into Generate
            var offsets = new List<int>();
            const int TEXT_OFFSET = 0x060010;
            const int TOO_MUCH = 0x074010; //or possibly 070000. It might not be possible to go any higher...

            var currentOffset = TEXT_OFFSET;

            foreach (var message in messages)
            {
                offsets.Add(currentOffset);
                currentOffset += message.Length;
                if (currentOffset > TOO_MUCH)
                    throw new Exception("There's too much text data! It'd overwrite the sprites from the ending cutscene if it keeps going.");
            }

            return offsets;
        }

        public static byte[] CalculatePointer(int offset)
        {
            //take in an int, spit out [XX XX 00]
            var pointer = new List<byte>();
            //TODO: calculation goes here

            //account for the iNES header

            //060010
            //minus 10 for the header (or plus? I forget)
            //turn into bytes, ditch the 06 so it can fit in an Int16 (7FFF)
            //swap the bytes

            pointer.Add(0x00);
            return pointer.ToArray();
        }
    }
}
