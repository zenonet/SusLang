using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace SusLang.Tests;

public class MathTests
{
    private readonly ITestOutputHelper testOutputHelper;
    private readonly Random random = new();

    public MathTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }


    [Fact]
    public void Addition()
    {
        string additionCode = File.ReadAllText("../../../CodeSamples/addition.sus");

        (Crewmate addent1, Crewmate addent2, Crewmate output) = GetOperators(additionCode);

        byte val1 = GetRandomByte();
        byte val2 = GetRandomByte();

        // Note that the byte cast will do the overflow stuff meaning it would convert int 257 to byte 1
        byte outputVal = (byte) (val1 + val2);


        ExecutionContext context = Compiler.CreateAst(additionCode);

        context.Crewmates[addent1] = val1;
        context.Crewmates[addent2] = val2;

        testOutputHelper.WriteLine("Attempting to calculate {0} + {1}", val1, val2);
        testOutputHelper.WriteLine("Expected output: {0}", outputVal);

        context.Continue();

        testOutputHelper.WriteLine("Result: {0}", context.Crewmates[output]);

        Assert.Equal(outputVal, context.Crewmates[output]);
    }

    [Fact]
    public void Multiplication()
    {
        string additionCode = File.ReadAllText("../../../CodeSamples/multiplication.sus");

        (Crewmate addent1, Crewmate addent2, Crewmate output) = GetOperators(additionCode);

        byte val1 = GetRandomByte();
        byte val2 = GetRandomByte();

        // Note that the byte cast will do the overflow stuff meaning it would convert int 257 to byte 1
        byte outputVal = (byte) (val1 * val2);


        ExecutionContext context = Compiler.CreateAst(additionCode);

        context.Crewmates[addent1] = val1;
        context.Crewmates[addent2] = val2;

        testOutputHelper.WriteLine("Attempting to calculate {0} * {1}", val1, val2);
        testOutputHelper.WriteLine("Expected output: {0}", outputVal);

        context.Continue();

        testOutputHelper.WriteLine("Result: {0}", context.Crewmates[output]);

        Assert.Equal(outputVal, context.Crewmates[output]);
    }

    [Fact]
    public void Division()
    {
        string additionCode = File.ReadAllText("../../../CodeSamples/division.sus");

        (Crewmate dividend, Crewmate divisor, Crewmate output) = GetOperators(additionCode);

        byte val1 = GetRandomByte();
        byte val2 = GetRandomByte();

        // This is just to prevent division by zero
        if (val2 == 0)
            val2 = 21;

        // Note that the byte cast will do the overflow stuff meaning it would convert int 257 to byte 1
        // The Math.Ceiling is because this implementation of division ceils the result
        // So, technically I just fixed a bug by breaking the test 
        byte outputVal = (byte) Math.Ceiling(val1 / (float)val2);


        ExecutionContext context = Compiler.CreateAst(additionCode);

        context.Crewmates[dividend] = val1;
        context.Crewmates[divisor] = val2;

        testOutputHelper.WriteLine("Attempting to calculate {0} / {1}", val1, val2);
        testOutputHelper.WriteLine("Expected output: {0}", outputVal);

        context.Continue();

        testOutputHelper.WriteLine("Result: {0}", context.Crewmates[output]);

        Assert.Equal(outputVal, context.Crewmates[output]);
    }

    private (Crewmate, Crewmate, Crewmate) GetOperators(string code)
    {
        string op1 = code.Split("//-op1")[1].Split('\n')[0].Trim();
        string op2 = code.Split("//-op2")[1].Split('\n')[0].Trim();
        string output = code.Split("//-output")[1].Split('\n')[0].Trim();

        return (op1, op2, output);
    }

    private byte GetRandomByte()
    {
        byte[] bytes = new byte[1];
        random.NextBytes(bytes);
        return bytes[0];
    }
}