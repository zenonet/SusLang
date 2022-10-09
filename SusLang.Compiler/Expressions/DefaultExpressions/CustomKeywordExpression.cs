using System;
using SoftCircuits.Collections;
using SusLang.CodeAnalysis;

namespace SusLang.Expressions.DefaultExpressions;

public class CustomKeywordExpression : Expression
{
    public static readonly OrderedDictionary<string, ExecutionContext> CustomKeywords = new();

    private Crewmate leftColor;
    private ExecutionContext keyword;

    private Crewmate[] outerParams;

    protected override bool OnParse(ref string code)
    {
        string line = code.Split(Environment.NewLine)[0];
        string[] words = line.Split(' ');

        int parameterCount = words.Length - 1;

        //This means that as long as the parameterCount is not 0, the keyword will be at index 1
        int keywordIndex = parameterCount > 0 ? 1 : 0;

        if (CustomKeywords.ContainsKey(words[keywordIndex]))
        {
            //Set keyword to the keyword that was found
            keyword = CustomKeywords[words[keywordIndex]].CloneAsNew();
        }
        else
        {
            //Set keyword to the keyword a crewmate points to
            
            Diagnosis keywordNotDefinedDiagnosis = new(Context,
                $"Keyword '{words[1]}' is not defined",
                InspectionSeverity.Error,
                Context.LineNumber);

            //Check if the keyword is a Crewmate (that might point to a CustomKeyword)
            Crewmate crewmate = Crewmate.Parse(words[keywordIndex], Context);

            if (crewmate == null)
                Compiler.Logging.LogError(keywordNotDefinedDiagnosis);

            if (Crewmates.ContainsKey(crewmate!))
            {
                if (CustomKeywords.Count < Crewmates[crewmate])
                    Compiler.Logging.LogError(keywordNotDefinedDiagnosis);
                

                // Set keyword to the custom keyword that the crewmate points to
                keyword = CustomKeywords.ByIndex[Crewmates[crewmate]];
            }
        }

        if (keyword.Parameters.Length != PreparsedColors.Length)
            Compiler.Logging.LogError(new Diagnosis(Context,
                $"Invalid parameters in call of keyword {words[keywordIndex]}",
                InspectionSeverity.Error,
                Context.LineNumber));

        //If there are no parameters, we can just return
        if (PreparsedColors.Length <= 0)
            goto codeSplitting;

        leftColor = PreparsedColors[0];

        //Initialize the outerParams array
        if (PreparsedColors.Length > 1)
        {
            outerParams = new[]
            {
                leftColor,
                PreparsedColors[1],
            };
        }
        else
        {
            outerParams = new[]
            {
                leftColor,
            };
        }

        codeSplitting:
        code = code[line.Length..];

        return true;
    }

    public override bool Execute()
    {
        //Copy the values from the outer scope into the custom keyword
        for (int i = 0; i < keyword.Parameters.Length; i++)
        {
            keyword.Crewmates[keyword.Parameters[i]] = Context.Crewmates[outerParams[i]];
        }

        //Execute the custom keyword
        keyword.Continue();

        //Copy the values from the custom keyword back to the outer scope
        for (int i = 0; i < keyword.Parameters.Length; i++)
        {
            Crewmates[outerParams[i]] = keyword.Crewmates[keyword.Parameters[i]];
        }

        return true;
    }
}