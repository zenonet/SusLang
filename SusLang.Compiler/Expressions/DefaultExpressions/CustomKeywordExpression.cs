using System;
using System.Collections.Generic;
using SusLang.CodeAnalysis;

namespace SusLang.Expressions.DefaultExpressions
{
    public class CustomKeywordExpression : Expression
    {
        public static readonly Dictionary<string, ExecutionContext> CustomKeywords = new();

        private Crewmate leftColor;
        private Crewmate rightColor;
        private ExecutionContext keyword;
        
        protected override bool OnParse(ref string code)
        {
            string line = code.Split(Environment.NewLine)[0];
            string[] words = line.Split(' ');

            if (PreparsedColors.Length < 1)
                Compiler.Logging.LogError(new Diagnosis(Context,
                    $"Left color of keyword {words[1]} is invalid",
                    InspectionSeverity.Error,
                    Context.LineNumber));

            leftColor = PreparsedColors[0];

            if (PreparsedColors.Length > 1)
                rightColor = PreparsedColors[1];

            keyword = CustomKeywords[words[1]].CloneAsNew();

            code = code[line.Length..];

            return true;
        }

        public override bool Execute()
        {
            keyword.Continue();
            return true;
        }
    }
}