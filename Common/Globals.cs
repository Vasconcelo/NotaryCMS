using System;
using System.Diagnostics;

namespace Common
{
    public static class Globals
    {
        public const string AdminRole = "ProductManager";
        public const string ErrorCodeLabel = "nna-error-code";
        public const string MessageIdLabel = "nna-message-id";
        public const string DateLabel = "nna-date";


        public static T RunWithBenchmark<T>(Func<T> predicate, out TimeSpan benchmark)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // run custom function
            var output = predicate();

            stopwatch.Stop();
            benchmark = stopwatch.Elapsed;

            return output;
        }
    }
}
