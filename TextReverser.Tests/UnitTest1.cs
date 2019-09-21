using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TextReverser.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var text = "chris";

            Assert.AreEqual("sirhc", TextReverser.ReverseText(text));
        }
    }
}
