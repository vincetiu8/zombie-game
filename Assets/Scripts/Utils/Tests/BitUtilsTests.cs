using NUnit.Framework;

namespace Utils.Tests
{
	public class BitUtilsTests
	{
		[Test]
		public void TestReadWriteBitUtils()
		{
			byte[] data = new byte[8];
			int offset = 0;

			// Test writing within 1 byte
			BitUtils.WriteBits(data, 0x9, 4, ref offset);
			Assert.AreEqual(4, offset);
			Assert.AreEqual(0x90, data[0]);

			// Test writing that overflows 1 byte
			BitUtils.WriteBits(data, 0xab, 8, ref offset);
			Assert.AreEqual(12, offset);
			Assert.AreEqual(0x9b, data[0]);
			Assert.AreEqual(0xa0, data[1]);

			// Test writing that overflows multiple bytes
			BitUtils.WriteBits(data, 0xcdef, 16, ref offset);
			Assert.AreEqual(28, offset);
			Assert.AreEqual(0xaf, data[1]);
			Assert.AreEqual(0xde, data[2]);
			Assert.AreEqual(0xc0, data[3]);

			// Test writing within 1 byte half full
			BitUtils.WriteBits(data, 7, 3, ref offset);
			Assert.AreEqual(31, offset);
			Assert.AreEqual(0xce, data[3]);

			offset = 0;

			// Test reading data within 1 byte
			int value = BitUtils.ReadBits(data, 4, ref offset);
			Assert.AreEqual(0x9, value);

			// Test reading that overflows 1 byte
			value = BitUtils.ReadBits(data, 8, ref offset);
			Assert.AreEqual(0xab, value);

			// Test reading that overflows multiple bytes
			value = BitUtils.ReadBits(data, 16, ref offset);
			Assert.AreEqual(0xcdef, value);

			// Test reading within 1 byte half full
			value = BitUtils.ReadBits(data, 3, ref offset);
			Assert.AreEqual(31, offset);
			Assert.AreEqual(7, value);
		}
	}
}