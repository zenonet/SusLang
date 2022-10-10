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
        color1 = ParseColor(colors[1], Context, logErrors: false);


        if (color1 == null)
        {
            //Check if it might be a pointer referencing
            if (CustomKeywordExpression.CustomKeywords.ContainsKey(colors[1].Trim()))
            {
                isKeywordReference = true;
                keywordName = colors[1].Trim();
                return true;
            }
        }

        return color0 is not null && color1 is not null;
    }

    public override bool Execute()
    {
        if (isKeywordReference)
        {
            // Get the index of the keyword
            int index = CustomKeywordExpression.CustomKeywords.IndexOf(keywordName) + 1;

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