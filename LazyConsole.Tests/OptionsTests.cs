using LazyConsole.Tests.Fixtures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LazyConsole.Tests
{
    [TestClass]
    public class OptionsTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var actions = LazyConsole.GenerateActions(typeof(PublicStaticConsole));
            Assert.IsTrue(actions.Count == 1);
        }
    }
}
