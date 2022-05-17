
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SusLangStringCreator
{
    public static class StringCreator
    {
        private static readonly Dictionary<string, int> Modifiers = new()
        {
            {"vented", 1},
            {"killed", 10},
            {"wasWithMe", -1},
            {"didVisual", -10},
        };

        public static void Test()
        {
            //This method is never executed. I just compile it as a .Net Static Method so that I can quickly test this project
            
            //Generate the code
            string output = CreateSusLangScriptForString("\n", "cyan");
            
            //Add some code to output 
            output += "\nsus cyan\nemergencyMeeting";
            SusLang.Compiler.Execute(output);
        }

        public static string CreateSusLangScriptForString(string input, string variableToUse)
        {
            StringBuilder output = new StringBuilder();

            byte[] characters = Encoding.ASCII.GetBytes(input);
            
            foreach (byte character in characters)
            {
                int currentValue = 0;

                while (currentValue < character)
                {
                    int difference = character - currentValue;
                    KeyValuePair<string, int> modifier = Modifiers.Where(x => x.Value <= difference).OrderByDescending(x => x.Value).First();
                    
                    output.AppendLine($"{variableToUse} {modifier.Key}");

                    currentValue += modifier.Value;
                }
                
                
                
            }

            return output.ToString();
        }
    }
}