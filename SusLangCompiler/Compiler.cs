using System;
using System.Collections.Generic;
using System.IO;
using SusLang.Expressions;

namespace SusLang
{
    public static class Compiler
    {
        public static readonly Version CompilerVersion = new (0, 5);
        
        #region compilationNeededFields

        internal static Crewmate SussedColor;
        internal static Dictionary<Crewmate, byte> Crewmates = new();

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
            Crewmates.Clear();
            
            //Fill the crewmate dict with all the crewmates from the enum
            foreach (Crewmate crewmate in Enum.GetValues<Crewmate>())
                Crewmates.Add(crewmate, 0);

            ExecuteInternal(code);
        }

        /// <summary>
        /// Executes a piece of code without resetting any changes the script made
        /// </summary>
        /// <param name="code">The code to continue with</param>
        public static void ContinueExecute(string code)
        {
            if(Crewmates.Count > 0)
                ExecuteInternal(code);
            else
                Execute(code);
        }

        /// <summary>
        /// Executes a piece of code
        /// </summary>
        /// <param name="code">The input code to execute</param>
        /// <returns>Whether the code was executed successfully (and not errors were thrown)</returns>
        internal static bool ExecuteInternal(string code)
        {
            while (code.Length > 0)
            {
                executingLine++;
                
                Expression expression = Expression.Parse(ref code);
                if(expression != null)
                    expression.Execute();
                else
                    return false;
                
                code = code.TrimStart('\r');
                code = code.TrimStart('\n');
            }

            return true;
        }


        #region Quick and dirty logging

        private static int executingLine;
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
            
            internal static void LogError(string error)
            {
                LogRaw($"\nSabotage in line {executingLine}: {error}\n");
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