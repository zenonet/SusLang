using Xunit.Abstractions;

namespace SusLang.Tests;

public class FunctionPointerTests : TestBase
{
    public FunctionPointerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }


    [Fact]
    public void ExplicitReferencing()
    {
        string additionCode = File.ReadAllText("../../../CodeSamples/Pointers/functionpointers_explicit.sus");
        
        Crewmate output = GetOperators(additionCode).Item3 ?? throw new InvalidOperationException("Output color not defined.");
        
        ExecutionContext context = Compiler.CreateAst(additionCode);

        context.Continue();

        TestOutputHelper.WriteLine("Result: {0}", context.Crewmates[output]);

        Assert.StrictEqual(1, context.Crewmates[output]);
    }
}