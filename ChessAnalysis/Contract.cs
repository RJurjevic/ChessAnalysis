namespace ExtensionMethods
{
    public static class MyExtensionMethods
    {
        public static bool Implies(this bool a, bool b)
        {
            return !a || b;
        }
    }
}

namespace System.Diagnostics.Meyer.Contracts
{
    public static class Contract
    {
        private static void Initialize()
        {
            System.Diagnostics.Trace.Listeners.Clear();
            DefaultTraceListener defaultListener;
            defaultListener = new DefaultTraceListener();
            Trace.Listeners.Add(defaultListener);
            defaultListener.LogFileName = @".\Logs\contract.log";
        }

        static Contract()
        {
            Initialize();
        }

        public static void Assert(bool condition, string message = "")
        {
            System.Diagnostics.Trace.Assert(condition, "Assertion violation:", message);
        }

        public static void Require(bool condition, string message = "")
        {
            System.Diagnostics.Trace.Assert(condition, "Precondition violation:", message);
        }

        public static void Ensure(bool condition, string message = "")
        {
            System.Diagnostics.Trace.Assert(condition, "Postcondition violation:", message);
        }
    }
}
