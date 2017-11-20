using mParticle.Sdk.Core;
using System.Diagnostics;

namespace mParticle.Sdk.UWP.ExampleApp
{
    internal class ExampleConsoleLogger : ILogger
    {
        public void Log(LogEntry entry)
        {
            //this just writes to debug logs, but you can use this
            //interface to use UWP logs via the Windows ETW APIs
            Debug.Write("mParticle: " + entry.Message + "\n");
        }
    }
}