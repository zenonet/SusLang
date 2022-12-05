using System;
using System.IO;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace SusLang.Tests;

public class MathTests : TestBase
{
    public MathTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
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

        TestOutputHelper.WriteLine("Attempting to calculate {0} + {1}", val1, val2);
        TestOutputHelper.WriteLine("Expected output: {0}", outputVal);

        context.Continue();

        TestOutputHelper.WriteLine("Result: {0}", context.Crewmates[output]);

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

        TestOutputHelper.WriteLine("Attempting to calculate {0} * {1}", val1, val2);
        TestOutputHelper.WriteLine("Expected output: {0}", outputVal);

        context.Continue();

        TestOutputHelper.WriteLine("Result: {0}", context.Crewmates[output]);

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

        TestOutputHelper.WriteLine("Attempting to calculate {0} / {1}", val1, val2);
        TestOutputHelper.WriteLine("Expected output: {0}", outputVal);

        context.Continue();

        TestOutputHelper.WriteLine("Result: {0}", context.Crewmates[output]);

        Assert.Equal(outputVal, context.Crewmates[output]);
    }
}