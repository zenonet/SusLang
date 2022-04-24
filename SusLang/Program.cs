
using System;
using System.Diagnostics;
using System.IO;

public static class Program
{
    private const string help = 
        @"Usage:
SusLang [option or path to source file]
You can use these options:
    -version    to print out the compiler version
    -info   to print out the link to the github repo";
    static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (File.Exists(args[0]))
                SusLang.Compiler.ExecuteFromFile(args[0]);
            else
            {
                switch (args[0])
                {
                    case "-version" or "-v":
                        Console.WriteLine("SusLang interpreter version: " + SusLang.Compiler.CompilerVersion);
                        break;
                    case "-info" or "-i":
                        Console.WriteLine(
                            "SusLang is an among-us-themed esolang written in C#. Visit https://github.com/zenonet/SusLang for more information");
                        break;
                    default:
                        Console.WriteLine($"File {args[0]} not found");
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine(help);
        }
    }
}