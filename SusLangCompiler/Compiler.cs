using System;
using System.Collections.Generic;
using System.IO;
using SusLang.Expressions;

namespace SusLang
{
    public static class Compiler
    {
        public static readonly Version CompilerVersion = new (0, 2);
        
        #region compilationNeededFields

        internal static Crewmate SussedColor;
        internal static Dictionary<Crewmate, byte> Crewmates = new();

        #endregion


        public static void ExecuteFromFile(string path) => Execute(File.ReadAllText(path));

        public static void Execute(string code)
        {
            //Fill the crewmate dict with all the crewmates from the enum
            foreach (Crewmate crewmate in Enum.GetValues<Crewmate>())
                Crewmates.Add(crewmate, 0);

            while (code.Length > 0)
            {
                executingLine++;
                Expression.Parse(code, out code).Execute();
                code = code.TrimStart('\r');
                code = code.TrimStart('\n');
            }
        }


        #region Quick and dirty logging

        private static int executingLine;
        public static class Logging
        {
            public static TextWriter Stream;


            internal static void LogError(string error)
            {
                LogRaw($"\nSabotage in line {executingLine}: {error}\n");
                Environment.Exit(1);
            }

            internal static void LogProgramOutput(string msg)
            {
                LogRaw(msg);
            }

            internal static void LogRaw(string msg)
            {
                if (Stream == null)
                    Stream = Console.Out;

                Stream.Write(msg);
                Stream.Flush();
            }
        }

        #endregion
    }
}