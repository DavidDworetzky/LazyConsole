using LazyConsole.Tests.Fixtures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LazyConsole.Tests
{
    [TestClass]
    public class OptionsTests
    {
        [TestMethod]
        public void PublicStaticConsoleWorks()
        {
            var actions = LazyConsole.GenerateActions(typeof(PublicStaticConsole));
            Assert.IsTrue(actions.Count == 1);
        }
        [TestMethod]
        public void MixedSignatureStaticConsoleWorks()
        {
            var actions = LazyConsole.GenerateActions(typeof(MixedSignatureStaticConsole));
            Assert.IsTrue(actions.Count == 2);
        }
    }
}
