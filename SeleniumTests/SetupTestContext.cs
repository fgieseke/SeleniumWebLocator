#region Using

#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeleniumWebLocator.Base
{
    [TestClass]
    public class SetupTestContext
        : BaseTest
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            InitTests(context);
        }

        [AssemblyCleanup]
        public static void TeardownAssembly()
        {
            CleanupTests();
        }

    }
}