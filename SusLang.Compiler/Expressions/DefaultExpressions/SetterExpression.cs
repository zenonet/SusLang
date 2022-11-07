using SoftCircuits.Collections;
using SusLang.CodeAnalysis;

namespace SusLang.Expressions.DefaultExpressions;

public class SetterExpression : Expression
{
    private Crewmate color0;
    private Crewmate color1;

    private bool isKeywordReference;

    private string keywordName;

    protected override bool OnParse(ref string code)
    {
        string[] colors = RawExpression.Split("wasWith");
        color0 = ParseColor(colors[0], Context);

        // Because we use colors[1] quite a lot in the following, we'll trim all whitespace around it here
        colors[1] = colors[1].Trim();
        
        // If there is no @ symbol specifying that a keyword is meant, then parse the second color
        if (!colors[1].StartsWith('@'))
            color1 = ParseColor(colors[1].Substring(1), Context, logErrors: false);


        //Check if it might be a pointer referencing
        if (!colors[1].StartsWith('.'))
        {
            //Remove the . symbol
            colors[1] = colors[1].Substring(1);
            if (CustomKeywordExpression.CustomKeywords.ContainsKey(colors[1].Trim()))
            {
                if (color1 == null)
                {
                    isKeywordReference = true;
                    keywordName = colors[1].Trim();
                    return true;
                }

                if (!colors[1].StartsWith('@'))
                    Compiler.Logging.LogError(new(Context,
                        $"Ambiguous identifier {color1.Name}. " + $"There is a color and a keyword called {color1.Name}. " +
                        "By default, the color is used. " +
                        "If you meant the keyword, use the keyword name with an @ before it. " +
                        $"For example: {color0.Name} wasWith @{color1.Name}" +
                        "If you meant the color and want to remove this warning, use the color name with a dot before it.",
                        InspectionSeverity.Warning,
                        Context.LineNumber));
            }
        }
        
        if (color1 == null)
        {
            Compiler.Logging.LogError(new(Context,
                "Could not find a color" + (colors[1].StartsWith('.') ? " or keyword" : "") + $" named {colors[1].Trim()}",
                InspectionSeverity.Error,
                Context.LineNumber));
        }
        
        return color0 is not null && color1 is not null;
    }

    public override bool Execute()
    {
        if (isKeywordReference)
        {
            // Get the index of the keyword
            int index = CustomKeywordExpression.CustomKeywords.IndexOf(keywordName);

            // Check if the index can be casted to a byte
            if (index > 255)
                Compiler.Logging.LogError(new(Context,
                    "Seriously? You defined more than 256 keywords? And now you even want a reference to one of them?",
                    InspectionSeverity.Error,
                    Context.LineNumber
                ));

            Crewmates[color0] = (byte) index;
            return true;
        }

        Crewmates[color0] = Crewmates[color1];
        return true;
    }
}