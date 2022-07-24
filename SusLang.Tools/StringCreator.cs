
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SusLang.Tools
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
            string output = CreateSusLangScriptForString("Hello World, I'd like to buy some coffee", "cyan", true, true, false);
            
            output = "sus cyan\n" + output;
            Console.WriteLine(output + "\n\n\n\n\n\n");
            
            SusLang.Compiler.Execute(output);
        }

        public static string CreateSusLangScriptForString(string input, string variableToUse, bool comment = false, bool useHeSyntax = false, bool tacticallyOvershoot = true)
        {
            StringBuilder output = new StringBuilder();

            byte[] characters = Encoding.ASCII.GetBytes(input);

            if (useHeSyntax)
            {
                output.Append($"sus {variableToUse}\n");
                variableToUse = "he";
            }



            int currentValue = 0;
            foreach (byte character in characters)
            {
                if (comment)
                    output.Append($"\n//Print {Encoding.ASCII.GetString(new []{character})}:\n");
                
                while (currentValue != character)
                {
                    int difference = character - currentValue;

                    KeyValuePair<string, int> modifier;

                    if (float.IsNegative(difference))
                    {
                        if (tacticallyOvershoot && Math.Abs(difference) is > 5 and < 10)
                            modifier = new KeyValuePair<string, int>("didVisual", -10);
                        else
                            modifier = Modifiers.Where(x => x.Value >= difference).OrderByDescending(x => x.Value).Last();
                    }
                    else
                    {
                        //Tactically overshoot
                        if (tacticallyOvershoot && difference is > 5 and < 10)
                            modifier = new KeyValuePair<string, int>("killed", 10);
                        else
                            modifier = Modifiers.Where(x => x.Value <= difference).OrderByDescending(x => x.Value).First();
                        
                    }

                    output.AppendLine($"{variableToUse} {modifier.Key}");

                    currentValue += modifier.Value;
                    
                    

                }

                //Output the character
                output.AppendLine("emergencyMeeting");


            }
                

            return output.ToString().TrimStart('\n');
        }
    }
}