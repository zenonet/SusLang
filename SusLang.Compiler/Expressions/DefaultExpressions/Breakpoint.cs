using System;

namespace SusLang.Expressions.DefaultExpressions
{
    public class Breakpoint : Expression
    {
        public static event Action OnBreakpointExecuted;
        private static bool shouldContinue = true;
        public static void Continue()
        {
            shouldContinue = true;
        }
        
        public static byte GetValue(Crewmate crewmate)
        {
            if (crewmate is null)
                throw new ArgumentNullException(nameof(crewmate));
            return Compiler.Crewmates[crewmate];
        }

        public static Crewmate Selected => Compiler.SussedColor;

        public override bool Execute()
        {
            shouldContinue = false;
            OnBreakpointExecuted?.Invoke();
            
            while (!shouldContinue)
            {
                //Wait
            }
            
            //Continue
            return true;
        }
    }
}