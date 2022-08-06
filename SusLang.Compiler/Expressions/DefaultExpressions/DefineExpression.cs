
using System;
using System.Linq;

namespace SusLang.Expressions.DefaultExpressions
{
    public class DefineExpression : Expression
    {
        protected override bool IsCuttingCode() => true;

        protected override bool OnParse(ref string code)
        {
            string line = code.Split(Environment.NewLine)[0].Trim();
            
            string[] words = line.Split(' ');

            switch (words[1].ToLower())
            {
                case "color":
                    if (words.Length != 3)
                    {
                        Compiler.Logging.LogError($"Invalid #define pattern: {line}");
                        return false;
                    }
                    Compiler.Crewmates.Add(words[2].ToLower(), 0);
                    break;
                
                
                case "suspect":
                    if(words.Length != 3)
                    {
                        Compiler.Logging.LogError($"Invalid #define pattern: {line}");
                        return false;
                    }

                    Crewmate color = ParseColor(words[2]);
                    if (color == null)
                        return false;

                    Compiler.Crewmates[color] = 65;
                    
                    break;
            }

            code = code.Substring(line.Length + Environment.NewLine.Length);
            
            
            return true;
        }
        
    }
}