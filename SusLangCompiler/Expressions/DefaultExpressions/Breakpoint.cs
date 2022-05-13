using System;

namespace SusLang.Expressions.DefaultExpressions
{
    public class Breakpoint : Expression
    {

        private static bool shouldContinue = true;
        public static void Continue()
        {
            shouldContinue = true;
        }
        
        public static byte GetValue(Crewmate crewmate)
        {
            if (crewmate is Crewmate.Null)
                throw new ArgumentException("'Crewmate.Null' isn't supposed to be used like this!");
            return Compiler.Crewmates[crewmate];
        }

        public override bool Execute()
        {
            shouldContinue = false;
            while (!shouldContinue)
            {
                //Wait
            }
            
            //Continue
            return true;
        }
    }
}