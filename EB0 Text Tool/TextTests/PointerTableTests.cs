using NUnit.Framework;
using TextPacker;

namespace TextTests
{
    public class PointerTableTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var foo = PointerTable.CalculatePointer(0x06F197);
            Assert.AreEqual(new byte[] { 0x96, 0x1B }, foo);
        }
    }
}
