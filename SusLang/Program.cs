
using System;
using System.IO;
using System.Linq;
using System.Reflection;

public static class Program
{
    private const string help = 
        @"Usage:
SusLang [option or path to source file]
You can use these options:
    -version    to print out the compiler version
    -info   to print out the link to the github repo
    -addpath   adds the directory of this executable to path
    -removepath   removes the directory of this executable from path";
    
    static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (File.Exists(args[0]))
                SusLang.Compiler.ExecuteFromFile(args[0]);
            else
            {
                switch (args[0].ToLower())
                {
                    case "-version" or "-v":
                        Console.WriteLine("SusLang interpreter version: " + SusLang.Compiler.CompilerVersion);
                        break;
                    case "-info" or "-i":
                        Console.WriteLine(
                            "SusLang is an among-us-themed esolang written in C#. Visit https://github.com/zenonet/SusLang for more information");
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

    private static void AddAssemblyToPath()
    {
        var scope = EnvironmentVariableTarget.User;
        string assemblyPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program))?.Location);
        
        var oldValue = Environment.GetEnvironmentVariable("PATH", scope);
        if (oldValue == null || assemblyPath == null)
        {
            Console.WriteLine("Unable to add this directory to Path");
            Environment.Exit(1);
        }

        if (oldValue.Split(";").Any(x => x == assemblyPath))
        {
            Console.WriteLine("This directory is already in Path");
            Environment.Exit(1);
        }
        
        string newValue  = oldValue + ";" + assemblyPath;
        Environment.SetEnvironmentVariable("PATH", newValue, scope);
        Console.WriteLine("Done!");
    }

    private static void RemoveAssemblyFromPath()
    {
        var scope = EnvironmentVariableTarget.User;
        string assemblyPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program))?.Location);
        
        var oldValue = Environment.GetEnvironmentVariable("PATH", scope);
        
        if (oldValue == null || assemblyPath == null)
        {
            Console.WriteLine("Unable to add this directory to Path");
            Environment.Exit(1);
        }

        if (oldValue.Split(";").All(x => x != assemblyPath))
        {
            Console.WriteLine("This directory is not in Path");
            Environment.Exit(1);
        }
        
        //Here, i replace twice so that it work even when there is no semicolon in front of the PATH entry
        string newValue  = oldValue.Replace(";" + assemblyPath, "").Replace(assemblyPath, "");
        Environment.SetEnvironmentVariable("PATH", newValue, scope);
        Console.WriteLine("Done!");
    }
}