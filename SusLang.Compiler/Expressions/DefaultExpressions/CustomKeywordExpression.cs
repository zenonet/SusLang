using System;
using System.Collections.Generic;
using SusLang.CodeAnalysis;

namespace SusLang.Expressions.DefaultExpressions
{
    public class CustomKeywordExpression : Expression
    {
        public static readonly Dictionary<string, ExecutionContext> CustomKeywords = new();

        private Crewmate leftColor;
        private ExecutionContext keyword;

        private Crewmate[] outerParams;

        protected override bool OnParse(ref string code)
        {
            string line = code.Split(Environment.NewLine)[0];
            string[] words = line.Split(' ');

            keyword = CustomKeywords[words[1]].CloneAsNew();

            if (keyword.Parameters.Length != PreparsedColors.Length)
                Compiler.Logging.LogError(new Diagnosis(Context,
                    $"Invalid parameters in call of keyword {words[1]}",
                    InspectionSeverity.Error,
                    Context.LineNumber));
            
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
}