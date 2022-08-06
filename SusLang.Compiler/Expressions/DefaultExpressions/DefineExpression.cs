
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

            switch (words[1])
            {
                case "color":
                    if(words.Length != 3)
                        Compiler.Logging.LogError($"Invalid #define pattern: #define {line}");
                    
                    Compiler.Crewmates.Add(words[2].ToLower(), 0);
                    break;
            }

            code = code.Substring(line.Length + Environment.NewLine.Length);
            
            
            return true;
        }
        
    }
}