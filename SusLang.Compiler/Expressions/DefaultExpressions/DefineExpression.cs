using System;
using SusLang.CodeAnalysis;

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
                        Compiler.Logging.LogError(
                            new Diagnosis(Context,
                                "Invalid color definition",
                                InspectionSeverity.Error,
                                Context.LineNumber)
                        );
                        return false;
                    }

                    Crewmates.Add(words[2].ToLower(), 0);
                    break;


                case "suspect":
                    if (words.Length != 3)
                    {
                        Compiler.Logging.LogError(
                            new Diagnosis(Context,
                                "Invalid suspect definition",
                                InspectionSeverity.Error,
                                Context.LineNumber)
                        );
                        return false;
                    }

                    Crewmate color = ParseColor(words[2], Context);
                    if (color == null)
                        return false;

                    Crewmates[color] = 65;

                    break;
                case "keyword":
                    if (words.Length != 3)
                    {
                        Compiler.Logging.LogError(
                            new Diagnosis(Context,
                                "Invalid keyword definition",
                                InspectionSeverity.Error,
                                Context.LineNumber)
                        );
                        return false;
                    }

                    if (words[2] == "end")
                        break;

                    if (IsParsingKeywordDefinition)
                    {
                        Compiler.Logging.LogError(
                            new Diagnosis(Context,
                                "Nested keyword definitions aren't supported",
                                InspectionSeverity.Warning,
                                Context.LineNumber)
                        );
                        return true;
                    }

                    //Parse keyword
                    IsParsingKeywordDefinition = true;

                    int endIndex =
                        code
                            .IndexOf("#define keyword end", StringComparison.Ordinal);

                    if (endIndex == -1)
                    {
                        Compiler.Logging.LogError(
                            new Diagnosis(Context,
                                "Keyword definition is not closed",
                                InspectionSeverity.Error,
                                Context.LineNumber)
                        );
                        return false;
                    }

                    IsParsingKeywordDefinition = false;

                    CustomKeywordExpression.CustomKeywords.Add(
                        words[2].ToLower(), Compiler.CreateAst(code[..endIndex][line.Length..])
                    );

                    //+19 because "#define keyword end" is 19 characters long and
                    //we want to cut it too
                    code = code[(endIndex + 19)..];

                    return true;
            }

            code = code[(line.Length + Environment.NewLine.Length)..];

            return true;
        }
    }
}