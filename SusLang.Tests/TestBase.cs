using System;
using Xunit.Abstractions;

namespace SusLang.Tests;

public abstract class TestBase
{
    protected readonly ITestOutputHelper TestOutputHelper;
    private readonly Random random = new();

    protected TestBase(ITestOutputHelper testOutputHelper)
    {
        this.TestOutputHelper = testOutputHelper;
    }

    protected static (Crewmate?, Crewmate?, Crewmate?) GetOperators(string code)
    {
        string? op1 = code.Contains("//-op1") ? code.Split("//-op1")[1].Split('\n')[0].Trim() : null;
        string? op2 = code.Contains("//-op2") ? code.Split("//-op2")[1].Split('\n')[0].Trim() : null;
        string? output = code.Contains("//-output") ? code.Split("//-output")[1].Split('\n')[0].Trim() : null;

        return (op1, op2, output);
    }

    protected byte GetRandomByte()
    {
        byte[] bytes = new byte[1];
        random.NextBytes(bytes);
        return bytes[0];
    }
}