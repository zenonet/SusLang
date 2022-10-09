using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SusLang;
using SusLang.Expressions.DefaultExpressions;
using SusLang.BuildEngine;

public static class Program
{
    private const string Help =
        @"Usage:
SusLang [option or path to source file]
You can use these options:
    -version    to print out the compiler version
    -info   to print out the link to the github repo
    -build  to create a .exe file of a script that can run without SusLang or Dotnet. Syntax: -build {sourcePath} {destinationPath}
    -addpath   adds the directory of this executable to path
    -removepath   removes the directory of this executable from path
    -translate  to generate a script that outputs a user defined string. Syntax: -translate {colorToUse} {parameters}
        You can use the following parameters:
            -comment or -cmt   to let the generator comment the script
            -tacticallyOvershoot or -to   might make scripts a little bit shorter
            -heSyntax or -hs    to make the generator select the color first and then access it using 'he'
        After that the generator will ask you to input your string";
    

    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] is "-api")
        {
            RunApi();
            return;
        }


        Breakpoint.OnBreakpointExecuted += context =>
        {
            Console.OutputEncoding = Encoding.ASCII;
            Console.WriteLine("\n----------\nBreakpoint activated:");
            foreach (KeyValuePair<Crewmate, byte> crewmate in context.Crewmates)
            {
                
                Console.Write($"{crewmate.Key}: {crewmate.Value}  or in ASCII:  " +
                              $"{Encoding.ASCII.GetString(new[] {crewmate.Value})}\n");
            }

            Console.WriteLine($"Currently selected color: {context.Crewmates.Selected}");
            Console.WriteLine("Press Enter to continue program execution");
            Console.WriteLine("----------");
            Console.ReadLine();
            context.Continue();
        };

        if (args.Length > 0)
        {
            if (File.Exists(args[0]))
                Compiler.ExecuteFromFile(args[0]);
            else
            {
                switch (args[0].ToLower())
                {
                    case "-version" or "-v":
                        Console.WriteLine("SusLang interpreter version: " + Compiler.CompilerVersion);
                        break;
                    case "-info" or "-i":
                        Console.WriteLine(
                            "SusLang is an among-us-themed esolang written in C#. Visit https://github.com/zenonet/SusLang for more information");
                        break;
                    case "-build" or "-b":
                        BuildEngine.BuildSusLangScriptFromFile(args[1], args[2]);
                        Console.WriteLine($"Successfully built {args[1]} to {args[2]}");
                        break;
                    case "-addpath" or "-ap":
                        Console.WriteLine(
                            "Do you really want to add the directory this executable is in to path?\n" +
                            "This means that you will be able to access every executable in this directory from anywhere on this machine with this user\n" +
                            "type 'yay' if you want to continue");
                        if (Console.ReadLine()?.ToLower() == "yay")
                        {
                            AddAssemblyToPath();
                        }

                        break;

                    case "-removepath" or "-rp":
                        Console.WriteLine(
                            "Do you really want to remove the directory this executable is in from path?\n" +
                            "This means that you will no longer be able to access every executable in this directory from anywhere on this machine with this user\n" +
                            "type 'I guess' if you want to continue");
                        if (Console.ReadLine()?.ToLower() == "i guess")
                        {
                            RemoveAssemblyFromPath();
                        }

                        break;
                    case "-translate" or "tr":
                        string color = args[1];
                        bool tacticallyOvershoot = args.Contains("-tacticallyOvershoot") || args.Contains("-to");
                        bool comment = args.Contains("-comment") || args.Contains("-cmt");
                        bool useHeSyntax = args.Contains("-heSyntax") || args.Contains("-hs");
                        
                        Console.WriteLine("Please enter your string here:");
                        string text = Console.ReadLine()!.Replace("\\n", "\n");
                        
                        Console.WriteLine(
                            SusLang.Tools.StringCreator.CreateSusLangScriptForString(
                                text,
                                color,
                                comment,
                                useHeSyntax,
                                tacticallyOvershoot)
                        );

                        break;

                    default:
                        Console.WriteLine($"File {args[0]} not found");
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine(Help);
        }
    }

    public static void RunApi()
    {
        Stream stdIn = Console.OpenStandardInput();
        Stream stdOut = Console.OpenStandardOutput();
        while (true)
        {
        }
    }

    private static void AddAssemblyToPath()
    {
        var scope = EnvironmentVariableTarget.User;
        string assemblyPath = AppContext.BaseDirectory;

        var oldValue = Environment.GetEnvironmentVariable("PATH", scope);
        if (oldValue == null)
        {
            Console.WriteLine("Unable to add this directory to Path");
            Environment.Exit(1);
        }

        if (oldValue.Split(";").Any(x => x == assemblyPath))
        {
            Console.WriteLine("This directory is already in Path");
            Environment.Exit(1);
        }

        string newValue = oldValue + ";" + assemblyPath;
        Environment.SetEnvironmentVariable("PATH", newValue, scope);
        Console.WriteLine("Done!");
    }

    private static void RemoveAssemblyFromPath()
    {
        var scope = EnvironmentVariableTarget.User;
        string assemblyPath = AppContext.BaseDirectory;
        var oldValue = Environment.GetEnvironmentVariable("PATH", scope);

        if (oldValue == null)
        {
            Console.WriteLine("Unable to remove this directory from Path");
            Environment.Exit(1);
        }

        if (oldValue.Split(";").All(x => x != assemblyPath))
        {
            Console.WriteLine("This directory is not in Path");
            Environment.Exit(1);
        }

        //Here, i replace twice so that it work even when there is no semicolon in front of the PATH entry
        string newValue = oldValue.Replace(";" + assemblyPath, "").Replace(assemblyPath, "");
        Environment.SetEnvironmentVariable("PATH", newValue, scope);
        Console.WriteLine("Done!");
    }
}