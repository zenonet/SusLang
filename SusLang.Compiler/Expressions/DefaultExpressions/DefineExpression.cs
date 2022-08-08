using System;
using System.Net;

namespace SusLang.Expressions.DefaultExpressions
{
    public class DefineExpression : Expression
    {
        protected override bool IsCuttingCode() => true;

        public static bool IsParsingKeywordDefinition = false;

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

                    Crewmates.Add(words[2].ToLower(), 0);
                    break;


                case "suspect":
                    if (words.Length != 3)
                    {
                        Compiler.Logging.LogError($"Invalid #define pattern: {line}");
                        return false;
                    }

                    Crewmate color = ParseColor(words[2]);
                    if (color == null)
                        return false;

                    Crewmates[color] = 65;

                    break;
                case "keyword":
                    if (words.Length != 3)
                    {
                        Compiler.Logging.LogError($"Invalid #define pattern: {line}");
                        return false;
                    }

                    if (words[2] == "end")
                        break;

                    if (IsParsingKeywordDefinition)
                    {
                        Compiler.Logging.LogError("Nested keyword definitions aren't supported");
                    }

                    //Parse keyword
                    IsParsingKeywordDefinition = true;

                    int endIndex =
                        code
                            .IndexOf("#define keyword end", StringComparison.Ordinal);

                    if (endIndex == -1)
                    {
                        Compiler.Logging.LogError("Keyword definition is not closed");
                        return false;
                    }

                    IsParsingKeywordDefinition = false;

                    CustomKeywordExpression.CustomKeywords.Add(
                        words[2].ToLower(), Compiler.CreateAst(code[..endIndex][line.Length..])
                    );


                    break;
            }

            code = code.Substring(line.Length + Environment.NewLine.Length);


            return true;
        }
    }
}