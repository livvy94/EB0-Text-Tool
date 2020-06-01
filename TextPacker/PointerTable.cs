using System;
using System.Collections.Generic;

namespace TextPacker
{
    public class PointerTable
    {
        public const int POINTER_TABLE_START = 0x030000;
        public const int POINTER_TABLE_END = 0x03177F; //TODO: double-check the value
        public const int SCRIPT_TEXTBANK_START = 0x060000;
        public const int SCRIPT_TEXTBANK_END = 0x070000;
        public const int NAMES_TEXTBANK_END = 0x09CC; //This text block starts at 0x00 and ends here. TODO: double-check the value

        public static byte[] Generate(List<byte[]> messages)
        {
            //takes in a list of the byte arrays TextConversion.Encode() produces
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
                    throw new Exception("The script text bank is full! If it kept going, the pointers would wrap back around and it wouldn't work.");
            }

            return offsets;
        }

        public static byte[] CalculatePointer(int offset)
        {
            //offset -= ROM_IO.iNES_HEADER_LENGTH;

            if (offset < NAMES_TEXTBANK_END)
                offset += 0x8000;

            var result = BitConverter.GetBytes(offset);
            return new byte[] { result[0], result[1], 0x00 };
        }

        public static uint BytesToOffset(byte input1, byte input2, byte currentBank)
        {
            var RawPointer = new byte[] { input1, input2, currentBank, 0x00 };
            return BitConverter.ToUInt32(RawPointer);
        }
    }
}
