using System;
using System.Collections.Generic;

namespace SusLang.Expressions.DefaultExpressions
{
    public class CustomKeywordExpression : Expression
    {
        public static readonly Dictionary<string, ExecutionContext> CustomKeywords = new();

        private Crewmate leftColor;
        private Crewmate rightColor;
        private string keyword;

        protected override bool OnParse(ref string code)
        {
            string line = code.Split(Environment.NewLine)[0];
            string[] words = line.Split(' ');

            leftColor = PreparsedColors[0];
            rightColor = PreparsedColors[1];
            keyword = words[1];

            code = code.Substring(line.Length + Environment.NewLine.Length);

            return true;
        }
    }
}