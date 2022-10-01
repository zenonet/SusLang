using System;
using System.IO;
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

                    if (words[2] == "end")
                        break;

                    if (words.Length is < 3 or > 6)
                    {
                        Compiler.Logging.LogError(
                            new Diagnosis(Context,
                                "Invalid keyword definition",
                                InspectionSeverity.Error,
                                Context.LineNumber)
                        );
                        return false;
                    }

                    if (IsParsingKeywordDefinition)
                    {
                        Compiler.Logging.LogError(
                            new Diagnosis(Context,
                                "Nested keyword definitions aren't supported",
                                InspectionSeverity.Error,
                                Context.LineNumber)
                        );
                        return false;
                    }

                    //Parse keyword
                    IsParsingKeywordDefinition = true;

                    int endIndex =
                        code.IndexOf("#define keyword end", StringComparison.Ordinal);

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

                    string definition = code[..endIndex][line.Length..];
                    ExecutionContext executionContext = Compiler.CreateAst(definition);

                    executionContext.Parameters = new Crewmate[words.Length - 3];

                    for (int i = 0; i < words.Length - 3; i++)
                    {
                        executionContext.Parameters[i] = Crewmate.Parse(words[i + 3], executionContext);
                    }

                    CustomKeywordExpression.CustomKeywords.Add(
                        words[2].ToLower(), executionContext
                    );

                    IsParsingKeywordDefinition = false;

                    //+19 because "#define keyword end" is 19 characters long and
                    //we want to cut it too
                    code = code[(endIndex + 19)..];

                    return true;
                case "import":
                    //Get the path (using a range index because i want to allow spaces in the path)
                    string path = string.Join("", words[2..]);

                    if (!File.Exists(path))
                    {
                        Compiler.Logging.LogError(
                            new Diagnosis(Context,
                                "Invalid import path",
                                InspectionSeverity.Error,
                                Context.LineNumber)
                        );
                        return false;
                    }

                    //Read the file
                    string file = File.ReadAllText(path);
                    
                    //Parse the file in a way that the context of all the expressions is the current context
                    ExecutionContext context = Compiler.CreateAst(file, contextToSet:Context);

                    //Execute it in the current context
                    Context.ExecuteInThisContext(context);

                    break;
            }

            code = code[(line.Length + 
                         (code.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0)
                         )..];

            return true;
        }
    }
}