using System;

namespace SusLang.Expressions.DefaultExpressions
{
    public class Breakpoint : Expression
    {
        public static event Action<ExecutionContext> OnBreakpointExecuted;
        private static bool shouldContinue = true;
        
        
        public override bool Execute()
        {
            Context.IsRunning = false;
            
            OnBreakpointExecuted?.Invoke(Context);

            return true;
        }
    }
}