using System;
using System.Collections.Generic;

namespace EB0_Text_Tool
{
    class Text
    {
        public string Decode(byte[] hexNumbers)
        {
            var result = string.Empty;
            //throw new NotImplementedException();
            foreach (var num in hexNumbers)
            {
                result += num switch
                {
                    0x00 => "[END]\r\n",
                    0x01 => "\r\n",
                    0x02 => "[PAUSE THEN OVERWRITE]",
                    0x03 => "[NEXT]\r\n",
                    0x05 => "[PADDING]",

                    0x80 => "[in]",
                    0x81 => "[il]",
                    0x82 => "[ll]",
                    0x83 => "['s]",
                    0x94 => '○',
                    0x95 => '•',
                    0x96 => '♪',
                    //8485=of
                    //8789=the
                    //8a93=blank
                    //97 to 9f - SMAAAASH!!

                    0xA0 => ' ',
                    0xA1 => '!',
                    0xA2 => '?',
                    0xA3 => '…',
                    0xA4 => '$',
                    0xA5 => '•',
                    0xA6 => '"',
                    0xA7 => "'",
                    0xA8 => '(',
                    0xA9 => ')',
                    0xAA => ':',
                    0xAB => ';',
                    0xAC => ',',
                    0xAD => '-',
                    0xAE => '.',
                    0xAF => '/',

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
                    0xBA => "[money]",
                    0xBB => 'α',
                    0xBC => 'ß',
                    0xBD => 'τ',
                    0xBE => 'π',
                    0xBF => 'Ω',

                    0xC0 => '*',
                    0xC1 => 'A',
                    0xC2 => 'B',
                    0xC3 => 'C',
                    0xC4 => 'D',
                    0xC5 => 'E',
                    0xC6 => 'F',
                    0xC7 => 'G',
                    0xC8 => 'H',
                    0xC9 => 'I',
                    0xCA => 'J',
                    0xCB => 'K',
                    0xCC => 'L',
                    0xCD => 'M',
                    0xCE => 'N',
                    0xCF => 'O',
                    0xD0 => 'P',
                    0xD1 => 'Q',
                    0xD2 => 'R',
                    0xD3 => 'S',
                    0xD4 => 'T',
                    0xD5 => 'U',
                    0xD6 => 'V',
                    0xD7 => 'W',
                    0xD8 => 'X',
                    0xD9 => 'Y',
                    0xDA => 'Z',
                    0xE0 => '→',
                    0xE1 => 'a',
                    0xE2 => 'b',
                    0xE3 => 'c',
                    0xE4 => 'd',
                    0xE5 => 'e',
                    0xE6 => 'f',
                    0xE7 => 'g',
                    0xE8 => 'h',
                    0xE9 => 'i',
                    0xEA => 'j',
                    0xEB => 'k',
                    0xEC => 'l',
                    0xED => 'm',
                    0xEE => 'n',
                    0xEF => 'o',
                    0xF0 => 'p',
                    0xF1 => 'q',
                    0xF2 => 'r',
                    0xF3 => 's',
                    0xF4 => 't',
                    0xF5 => 'u',
                    0xF6 => 'v',
                    0xF7 => 'w',
                    0xF8 => 'x',
                    0xF9 => 'y',
                    0xFA => 'z',

                    0xFF => '►',
                    _ => "[" + num.ToString("X2") + "]",
                };
            }
            return result;
        }

        public byte[] Encode(string message)
        {
            //The txt file will be broken up into individual messages, this method will handle the encoding of one of them.

            var result = new List<byte>();

            var currentControlCode = string.Empty;

            foreach (char letter in message)
            {
                if (currentControlCode != string.Empty)
                {
                    if (letter == ']')
                    {
                        //it's complete! convert it
                        currentControlCode = string.Empty;
                    }

                    currentControlCode += letter; //build it up until we have a full code we can compare to the list of codes
                }

                //If the code reaches this part, it wasn't a control code. Convert as char
                //insert switch statement here

            }


            throw new NotImplementedException();

            return result.ToArray();
        }
    }
}
