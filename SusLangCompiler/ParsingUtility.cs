using System.Collections.Generic;
using System.Linq;

namespace SusLang
{
    internal static class ParsingUtility
    {
        public static string FindBetweenBrackets(ref string code)
        {
            
            //Removes the first bracket if it is inside of code
            if (code[0] == '[')
            {
                code = code.Substring(1);
            }
            
            int openedBraces = 1;

            int relaxIndex = 0;
            //Tracks all brackets until it finds the matching one
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == '[')
                    openedBraces++;

                if(code[i] == ']')
                    openedBraces--;

                if (openedBraces != 0) continue;
                
                relaxIndex = i;
                break;
            }

            //Removes the bracket itself from the string
            List<char> list = code.ToList();
            list.RemoveAt(relaxIndex);
            code = new string(list.ToArray());
            
            //Return everything before relaxIndex. If relaxIndex is 0 (no closing bracket was found) return null
            string ret = relaxIndex == 0 ? null : code[..relaxIndex];
            code = code[relaxIndex..];
            return ret;
        }
    }
}