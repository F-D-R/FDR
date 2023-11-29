using NUnit.Framework;
using System.Diagnostics;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class TestBase
    {
        private ConsoleTraceListener consoleTracer;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            consoleTracer = new ConsoleTraceListener();
            Trace.Listeners.Add(consoleTracer);
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            Trace.Flush();
            Trace.Listeners.Remove(consoleTracer);
            consoleTracer?.Close();
            consoleTracer?.Dispose();
        }

        [SetUp]
        public virtual void SetUp()
        {
        }

        [TearDown]
        public virtual void TearDown()
        {
            Common.FileComparer = null;
        }
    }
}
