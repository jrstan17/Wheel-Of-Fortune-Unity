using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class Utilities {
        public static bool IsVowel(char c) {
            c = char.ToUpper(c);
            return (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U');
        }
    }