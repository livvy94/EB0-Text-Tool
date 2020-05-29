using System;
using System.Collections.Generic;

namespace TextPacker
{
    public class TextConversion
    {
        //NOTE: The order here is based on letter frequency in general fiction, according to http://letterfrequency.org
        //Will this make decoding faster? Who knows! Thought it was a cool idea though

        const string TEXT_FOR_00 = "[END]";
        const string TEXT_FOR_01 = "\r\n"; //I wonder if this will look good when I get dumping working...
        const string TEXT_FOR_02 = "[PAUSE THEN OVERWRITE]";
        const string TEXT_FOR_03 = "[NEXT]";
        const string TEXT_FOR_05 = "[PADDING]";
        //TODO: Control codes that are only present in the bank-related parts of the script

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
                foreach (var letter in line)
                {
                    if (letter == '[')
                        currentControlCode = "["; //start building a control code)

                    if (currentControlCode != string.Empty)
                    {
                        if (letter == ']')
                        {
                            //We now have a complete control code, ready for conversion

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

                                default: //If it's not any of these things, the only other thing it could be is a hex literal like [05]
                                    result.Add(ConvertHexString(currentControlCode));
                                    break;
                            }

                            currentControlCode = string.Empty;
                            continue;
                        }

                        currentControlCode += letter; //build it up until we have a full code we can compare to the list of codes
                    }

                    //If the code reaches this part, it wasn't a control code. Convert as char
                    //insert switch statement here
                    switch (letter)
                    {
                        //Punctuation
                        case '*': result.Add(0xC0); break;
                        case ' ': result.Add(0xA0); break;
                        case '.': result.Add(0xAE); break;
                        case ',': result.Add(0xAC); break;
                        case '!': result.Add(0xA1); break;
                        case '?': result.Add(0xA2); break;
                        case '…': result.Add(0xA3); break;

                        //Lowercase letters
                        case 'e': result.Add(0xE5); break;
                        case 't': result.Add(0xF4); break;
                        case 'a': result.Add(0xE1); break;
                        case 'o': result.Add(0xEF); break;
                        case 'h': result.Add(0xE8); break;
                        case 'n': result.Add(0xEE); break;
                        case 'i': result.Add(0xE9); break;
                        case 's': result.Add(0xF3); break;
                        case 'r': result.Add(0xF2); break;
                        case 'd': result.Add(0xE4); break;
                        case 'l': result.Add(0xEC); break;
                        case 'u': result.Add(0xF5); break;
                        case 'w': result.Add(0xF7); break;
                        case 'm': result.Add(0xED); break;
                        case 'c': result.Add(0xE3); break;
                        case 'g': result.Add(0xE7); break;
                        case 'f': result.Add(0xE6); break;
                        case 'y': result.Add(0xF9); break;
                        case 'p': result.Add(0xF0); break;
                        case 'v': result.Add(0xF6); break;
                        case 'k': result.Add(0xEB); break;
                        case 'b': result.Add(0xE2); break;
                        case 'j': result.Add(0xEA); break;
                        case 'x': result.Add(0xF8); break;
                        case 'z': result.Add(0xFA); break;
                        case 'q': result.Add(0xF1); break;

                        //Uppercase letters
                        case 'E': result.Add(0xC5); break;
                        case 'T': result.Add(0xD4); break;
                        case 'A': result.Add(0xC1); break;
                        case 'O': result.Add(0xCF); break;
                        case 'H': result.Add(0xC8); break;
                        case 'N': result.Add(0xCE); break;
                        case 'I': result.Add(0xC9); break;
                        case 'S': result.Add(0xD3); break;
                        case 'R': result.Add(0xD2); break;
                        case 'D': result.Add(0xC4); break;
                        case 'L': result.Add(0xCC); break;
                        case 'U': result.Add(0xD5); break;
                        case 'W': result.Add(0xD7); break;
                        case 'M': result.Add(0xCD); break;
                        case 'C': result.Add(0xC3); break;
                        case 'G': result.Add(0xC7); break;
                        case 'F': result.Add(0xC6); break;
                        case 'Y': result.Add(0xD9); break;
                        case 'P': result.Add(0xD0); break;
                        case 'V': result.Add(0xD6); break;
                        case 'K': result.Add(0xCB); break;
                        case 'B': result.Add(0xC2); break;
                        case 'J': result.Add(0xCA); break;
                        case 'X': result.Add(0xD8); break;
                        case 'Z': result.Add(0xDA); break;
                        case 'Q': result.Add(0xD1); break;

                        //Numbers
                        case '0': result.Add(0xB0); break;
                        case '1': result.Add(0xB1); break;
                        case '2': result.Add(0xB2); break;
                        case '3': result.Add(0xB3); break;
                        case '4': result.Add(0xB4); break;
                        case '5': result.Add(0xB5); break;
                        case '6': result.Add(0xB6); break;
                        case '7': result.Add(0xB7); break;
                        case '8': result.Add(0xB8); break;
                        case '9': result.Add(0xB9); break;

                        //Chars that are almost never in the script text
                        case '$': result.Add(0xA4); break;
                        case '•': result.Add(0xA5); break;
                        case '"': result.Add(0xA6); break;
                        case '\'': result.Add(0xA7); break;
                        case '(': result.Add(0xA8); break;
                        case ')': result.Add(0xA9); break;
                        case ':': result.Add(0xAA); break;
                        case ';': result.Add(0xAB); break;
                        case '-': result.Add(0xAD); break;
                        case '/': result.Add(0xAF); break;
                        case 'α': result.Add(0xBB); break;
                        case 'ß': result.Add(0xBC); break;
                        case 'τ': result.Add(0xBD); break;
                        case 'π': result.Add(0xBE); break;
                        case 'Ω': result.Add(0xBF); break;
                        case '○': result.Add(0x94); break;
                        //case '•': result.Add(0x95); break; //TODO: Yup yup duplicate lol
                        case '♪': result.Add(0x96); break;
                        case '→': result.Add(0xE0); break;
                        case '►': result.Add(0xFF); break;

                        default:
                            throw new Exception("Found an unknown letter! → " + letter.ToString());
                    }
                }
            }

            return result.ToArray();
        }

        private byte ConvertHexString(string input)
        {
            input = TrimHexString(input);

            try
            {
                return byte.Parse(input, System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
                throw new Exception("ERROR: Unknown control code, DTE character, or hex number → " + input);
            }
        }

        private string TrimHexString(string input)
        {
            var result = input.TrimStart('[').TrimEnd(']').Trim();

            if (result.StartsWith("0x"))
                result = result.Remove(0, 2);

            return result;
        }
    }
}
