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
            var pointer = PointerTable.CalculatePointer(0x06F187);
            Assert.AreEqual(new byte[] { 0x87, 0xF1, 0x00 }, pointer);
        }

        //[Test]
        //public void CalculatePointer_ForNames()
        //{
        //    var pointer = PointerTable.CalculatePointer(0x00);
        //    Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00 }, pointer); //TODO: Look in the ROM for the correct values
        //}

        [Test]
        public void ConvertTPTEntry()
        {
            Assert.AreEqual(0x06F187, PointerTable.BytesToOffset(0x87, 0xF1, 0x06));
        }
    }
}
