using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using SusLang.CodeAnalysis;
using SusLang.Expressions;

namespace SusLang
{
    public static class Compiler
    {
        public static readonly Version CompilerVersion = new(0, 5);

        #region compilationNeededFields

        public static readonly ImmutableDictionary<Crewmate, byte> StandardCrewmates = new Dictionary<Crewmate, byte>()
        {
            {"red", 0},
            {"blue", 0},
            {"green", 0},
            {"pink", 0},
            {"orange", 0},
            {"yellow", 0},
            {"black", 0},
            {"white", 0},
            {"purple", 0},
            {"brown", 0},
            {"cyan", 0},
            {"lime", 0},
            {"maroon", 0},
            {"rose", 0},
            {"banana", 0},
            {"gray", 0},
            {"tan", 0},
            {"coral", 0},
        }.ToImmutableDictionary();


        public static ExecutionContext ExecutionContext;

        #endregion


        /// <summary>
        /// Executes a piece of code from a file path
        /// </summary>
        /// <param name="path">The path to load the script from</param>
        public static void ExecuteFromFile(string path) => Execute(File.ReadAllText(path));

        /// <summary>
        /// Executes a piece of code
        /// </summary>
        /// <param name="code">The input code to execute</param>
        public static void Execute(string code)
        {
            //Set the Crewmate values to the standard crewmates
            ExecuteInternal(code);
        }


        /// <summary>
        /// Executes a piece of code
        /// </summary>
        /// <param name="code">The input code to execute</param>
        /// <returns>Whether the code was executed successfully (and not errors were thrown)</returns>
        internal static bool ExecuteInternal(string code)
        {
            ExecutionContext = CreateAst(code);

            ExecutionContext.Continue();

            return true;
        }

        public static ExecutionContext CreateAst(string code)
        {
            ExecutionContext context = new(new List<Expression>());

            while (code.Length > 0)
            {
                Expression expression = Expression.Parse(ref code, context);
                if (expression != null)
                    (context.Expressions as List<Expression>)!.Add(expression);
                else
                    return context;

                while (code.StartsWith(Environment.NewLine))
                {
                    code = code[Environment.NewLine.Length..];
                    context.LineNumber++;
                }
            }

            return context;
        }

        #region Quick and dirty logging

        internal static int ExecutingLine;

        public static class Logging
        {
            public static TextWriter Stream;

            public static event Func<string> OnInputExpected;
            public static event Action<string> OnOutput;

            internal static string WaitForInput()
            {
                //Fallback:
                if (OnInputExpected is null || OnInputExpected.GetInvocationList().Length < 1)
                    return Console.ReadLine();

                return OnInputExpected?.Invoke();
            }

            internal static void LogError(Diagnosis diagnosis)
            {
                LogRaw($"Sabotage in line {diagnosis.LineNumber}: {diagnosis.Message}");
            }

            internal static void LogError(string error)
            {
                LogRaw($"\nSabotage in line {ExecutingLine}: {error}\n");
            }

            internal static void LogProgramOutput(string msg)
            {
                LogRaw(msg);
            }

            internal static void LogRaw(string msg)
            {
                Stream ??= Console.Out;

                Stream.Write(msg);
                Stream.Flush();

                OnOutput?.Invoke(msg);
            }
        }

        #endregion
    }
}