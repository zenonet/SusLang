using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using SusLang.CodeAnalysis;
using SusLang.Expressions.DefaultExpressions;

namespace SusLang.Expressions
{
    public class Expression
    {
        public ExecutionContext Context;
        protected Crewmate Selected => Context.Crewmates.Selected;

        public string RawExpression;

        protected Crewmate[] PreparsedColors;


        public CrewmateList Crewmates =>
            Context.Crewmates;

        public virtual bool Execute() => true;

        protected static Crewmate ParseColor(string code, ExecutionContext context, bool logErrors = true)
        {
            //Prepare
            string colorString = code.ToLower().Trim();

            if (colorString is "he" or "him" or "her" or "she")
                return Crewmate.RefToSelectedInstance;

            //Parse the color:

            Crewmate color = Crewmate.Parse(colorString, context);

            if (color != null)
                return color;

            if (logErrors)
            {
                Compiler.Logging.LogError(new(context,
                    $"Color not found: {code}\n" +
                    "     Consider defining it using '#define color <name>'", InspectionSeverity.Error, context.LineNumber));
            }

            return null;
        }

        private static readonly Dictionary<string, Type> Patterns = new()
        {
            {@"^sus (\w+)", typeof(SusExpression)},
            {@"^emergencyMeeting", typeof(OutputExpression)},
            {@"^report", typeof(OutputExpression)},
            {@"^who\?", typeof(WhoExpression)},
            {@"^\[(?:.|\s)*", typeof(Loop)},
            {@"^\w+ wasWith (?:@|.)?\w+", typeof(SetterExpression)},
            {@"^#define ", typeof(DefineExpression)},
            {@"^breakpoint", typeof(Breakpoint)},

            {@"^(\w+) vented", typeof(ValueModificator)},
            {@"^(\w+) killed", typeof(ValueModificator)},
            {@"^(\w+) wasWithMe", typeof(ValueModificator)},
            {@"^(\w+) didVisual", typeof(ValueModificator)},

            {@"^(?:\s|\n|\r|\t)+", typeof(DummyExpression)},
            {@"^(?:\/\/.*|(trashtalk).*)", typeof(DummyExpression)},

            {@"^(?:(\w+) )?\w+(?: (\w+))?", typeof(CustomKeywordExpression)}
        };

        public static Expression Parse(ref string code, ExecutionContext context)
        {
            Expression expression = null;
            string restBuffer = code;

            foreach (KeyValuePair<string, Type> pair in Patterns)
            {
                Match match = Regex.Match(code, pair.Key);

                if (!match.Success)
                    continue;

                Crewmate[] colors = new Crewmate[match.Groups.Count - 1];
                if (match.Groups.Count > 1)
                {
                    List<Crewmate> colorsList = new();
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        Group group = match.Groups[i];
                        string color = group.Value.Trim();
                        if (color.Length < 1)
                            continue;

                        Crewmate crewmate = ParseColor(color, context);

                        colorsList.Add(crewmate);
                    }

                    colors = colorsList.ToArray();
                }

                expression = FormatterServices.GetUninitializedObject(pair.Value) as Expression;
                //expression = Activator.CreateInstance(pair.Value) as Expression;

                if (expression is null)
                {
                    Compiler.Logging.LogError(new(context,
                        $"There was a problem parsing '{Regex.Match(code, @"[^\s\\]+").Value}'",
                        InspectionSeverity.Error,
                        context.LineNumber));
                    return null;
                }

                expression.Context = context;

                expression.PreparsedColors = colors;

                expression.RawExpression = match.Value;
                restBuffer = code[match.Length..];

                //Call the subclasses OnParse callback
                bool success = expression.OnParse(ref code);

                if (!success)
                    return null;

                break;

                nextPattern: ;
            }

            if (expression == null)
            {
                Compiler.Logging.LogError(new Diagnosis(context,
                    $"Couldn't parse '{Regex.Match(restBuffer, @"[^\s\\]+").Value}'",
                    InspectionSeverity.Error,
                    context.LineNumber));

                if (Compiler.DontLog)
                    return null;
            }

            if (!expression!.IsCuttingCode())
                code = restBuffer;
            return expression;
        }

        protected virtual bool OnParse(ref string code)
        {
            return true;
        }

        protected virtual bool IsCuttingCode() => false;

        /// <summary>
        /// Should be overriden by expressions which wrap other expressions
        /// There, it should call SetContextRecursively on the wrapped expressions
        /// </summary>
        /// <param name="new">The context to set to</param>
        public virtual void SetContextRecursively(ExecutionContext @new)
        {
            Context = @new;
        }

        public Expression()
        {
        }
    }
}