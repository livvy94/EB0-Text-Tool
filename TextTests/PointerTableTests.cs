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
        public void CalculatePointer_ForDialogTextBank()
        {
            var foo = PointerTable.CalculatePointer(0x06F197);
            Assert.AreEqual(new byte[] { 0x87, 0xF1, 0x00 }, foo);
        }

        [Test]
        public void CalculatePointer_ForNames()
        {
            var foo = PointerTable.CalculatePointer(0x00);
            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00 }, foo); //TODO: Look in the ROM for the correct values
        }
    }
}
