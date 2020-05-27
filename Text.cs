using System;
using System.Collections.Generic;

namespace EB0_Text_Tool
{
    class Text
    {
        //NOTE: The order here is based on letter frequency in general fiction, according to http://letterfrequency.org
        //Will this make decoding faster? Who knows! Thought it was a cool idea though
        const string TEXT_FOR_00 = "[END]";
        const string TEXT_FOR_01 = "\r\n";
        const string TEXT_FOR_02 = "[PAUSE THEN OVERWRITE]";
        const string TEXT_FOR_03 = "[NEXT]";
        const string TEXT_FOR_05 = "[PADDING]";

        public string Decode(byte[] hexNumbers)
        {
            var result = string.Empty;
            //throw new NotImplementedException();
            foreach (var num in hexNumbers)
            {
                result += num switch
                {
                    //If the CHR bank gets changed, those changes will need to be taken into account here
                    0x00 => TEXT_FOR_00,
                    0x01 => TEXT_FOR_01,
                    0x02 => TEXT_FOR_02,
                    0x03 => TEXT_FOR_03,
                    0x05 => TEXT_FOR_05,

                    //Punctuation
                    0xC0 => '*',
                    0xA0 => ' ',
                    0xAE => '.',
                    0xAC => ',',
                    0xA1 => '!',
                    0xA2 => '?',
                    0xA3 => '…',

                    //Lowercase letters
                    0xE5 => 'e',
                    0xF4 => 't',
                    0xE1 => 'a',
                    0xEF => 'o',
                    0xE8 => 'h',
                    0xEE => 'n',
                    0xE9 => 'i',
                    0xF3 => 's',
                    0xF2 => 'r',
                    0xE4 => 'd',
                    0xEC => 'l',
                    0xF5 => 'u',
                    0xF7 => 'w',
                    0xED => 'm',
                    0xE3 => 'c',
                    0xE7 => 'g',
                    0xE6 => 'f',
                    0xF9 => 'y',
                    0xF0 => 'p',
                    0xF6 => 'v',
                    0xEB => 'k',
                    0xE2 => 'b',
                    0xEA => 'j',
                    0xF8 => 'x',
                    0xFA => 'z',
                    0xF1 => 'q',

                    //Uppercase letters
                    0xC5 => 'E',
                    0xD4 => 'T',
                    0xC1 => 'A',
                    0xCF => 'O',
                    0xC8 => 'H',
                    0xCE => 'N',
                    0xC9 => 'I',
                    0xD3 => 'S',
                    0xD2 => 'R',
                    0xC4 => 'D',
                    0xCC => 'L',
                    0xD5 => 'U',
                    0xD7 => 'W',
                    0xCD => 'M',
                    0xC3 => 'C',
                    0xC7 => 'G',
                    0xC6 => 'F',
                    0xD9 => 'Y',
                    0xD0 => 'P',
                    0xD6 => 'V',
                    0xCB => 'K',
                    0xC2 => 'B',
                    0xCA => 'J',
                    0xD8 => 'X',
                    0xDA => 'Z',
                    0xD1 => 'Q',

                    //DTE
                    0x80 => "[in]",
                    0x81 => "[il]",
                    0x82 => "[ll]",
                    0x83 => "['s]",
                    //8485=of
                    //8789=the
                    //8a93=blank
                    //97 to 9f - SMAAAASH!!

                    //Numbers
                    0xB0 => '0',
                    0xB1 => '1',
                    0xB2 => '2',
                    0xB3 => '3',
                    0xB4 => '4',
                    0xB5 => '5',
                    0xB6 => '6',
                    0xB7 => '7',
                    0xB8 => '8',
                    0xB9 => '9',

                    //Chars that are almost never in the script text
                    0xA4 => '$',
                    0xA5 => '•',
                    0xA6 => '"',
                    0xA7 => "'",
                    0xA8 => '(',
                    0xA9 => ')',
                    0xAA => ':',
                    0xAB => ';',
                    0xAD => '-',
                    0xAF => '/',
                    0xBA => "[money]",
                    0xBB => 'α',
                    0xBC => 'ß',
                    0xBD => 'τ',
                    0xBE => 'π',
                    0xBF => 'Ω',
                    0x94 => '○',
                    0x95 => '•', //TODO: this is a duplicate value of 0xA5. Double check what the PPU actually looks like when you get home
                    0x96 => '♪',
                    0xE0 => '→',
                    0xFF => '►',
                    _ => "[" + num.ToString("X2") + "]", //Anything that isn't listed here will just be dumped as [hex number]
                };
            }
            return result;
        }

        public byte[] Encode(string message)
        {
            //The txt file will be broken up into individual messages, this method will handle the encoding of one of them.

            //The input string will be multiple lines, so split it on \r\n
            string[] splitString = message.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var result = new List<byte>();
            var currentControlCode = string.Empty;

            foreach (var line in splitString)
            {
                foreach (char letter in line)
                {
                    if (currentControlCode != string.Empty)
                    {
                        if (letter == ']')
                        {
                            //We now have a complete control code, ready for conversion

                            if (currentControlCode.Length == 4) //For straight-up hex numbers like "[05]"
                                result.Add(ConvertHexString(currentControlCode));
                            else
                            {
                                switch (currentControlCode)
                                {
                                    case TEXT_FOR_01:
                                        result.Add(0x01); break;
                                    case TEXT_FOR_02:
                                        result.Add(0x02); break;
                                    case TEXT_FOR_03:
                                        result.Add(0x03); break;
                                    case TEXT_FOR_00:
                                        result.Add(0x00); break;
                                    case TEXT_FOR_05:
                                        result.Add(0x05); break;

                                    case "[ll]":
                                        result.Add(0x82); break;
                                    case "[il]":
                                        result.Add(0x81); break;
                                    case "['s]":
                                        result.Add(0x83); break;
                                    case "[in]":
                                        result.Add(0x80); break;
                                    case "[money]":
                                        result.Add(0xBA); break;
                                }
                            }

                            currentControlCode = string.Empty;
                        }

                        currentControlCode += letter; //build it up until we have a full code we can compare to the list of codes
                    }

                    //If the code reaches this part, it wasn't a control code. Convert as char
                    //insert switch statement here

                }
            }




            throw new NotImplementedException();

            return result.ToArray();
        }

        private byte ConvertHexString(string input)
        {
            if (input[0] == '[')
                input = RemoveBrackets(input);

            return byte.Parse(input, System.Globalization.NumberStyles.HexNumber);
        }

        private string RemoveBrackets(string input)
        {
            return input.TrimStart('[').TrimEnd(']');
        }
    }
}
