using NUnit.Framework;

namespace FDR.Tools.Library.Test
{
    /// <summary>
    /// Base class of every test
    /// Provides overridable dummy implementation for all steps
    /// in the test flow.
    /// </summary>
    [TestFixture]
    public abstract class TestBase
    {
        /// <summary>
        /// This method is called first, before any test gets executed.
        /// Can be extended in derived classes via method override.
        /// </summary>
        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {

        }

        /// <summary>
        /// Called before each test.
        /// Override to add test initialization logic.
        /// </summary>
        [SetUp]
        public virtual void SetUp()
        {

        }

        /// <summary>
        /// Called after each test.
        /// Override to extend with additional test cleanup logic.
        /// </summary>
        [TearDown]
        public virtual void Destroy()
        {

        }

        /// <summary>
        /// Called after every test has run.
        /// Override to extend with additional fixture cleanup logic.
        /// </summary>
        [OneTimeTearDown]
        public virtual void FixtureDestroy()
        {
        }
    }
}
