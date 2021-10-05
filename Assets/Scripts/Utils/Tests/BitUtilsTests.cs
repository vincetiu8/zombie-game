using NUnit.Framework;

namespace Utils.Tests
{
	public class BitUtilsTests
	{
		[Test]
		public void TestReadWrite()
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

			// Test truncating long data
			BitUtils.WriteBits(data, 0x1234, 8, ref offset);
			Assert.AreEqual(36, offset);
			Assert.AreEqual(0xc4, data[3]);
			Assert.AreEqual(0x30, data[4]);

			// Test writing within 1 byte half full
			BitUtils.WriteBits(data, 7, 3, ref offset);
			Assert.AreEqual(39, offset);
			Assert.AreEqual(0x3e, data[4]);

			offset = 0;

			// Test reading data within 1 byte
			int value = BitUtils.ReadBits(data, 4, ref offset);
			Assert.AreEqual(4, offset);
			Assert.AreEqual(0x9, value);

			// Test reading that overflows 1 byte
			value = BitUtils.ReadBits(data, 8, ref offset);
			Assert.AreEqual(12, offset);
			Assert.AreEqual(0xab, value);

			// Test reading that overflows multiple bytes
			value = BitUtils.ReadBits(data, 16, ref offset);
			Assert.AreEqual(28, offset);
			Assert.AreEqual(0xcdef, value);

			// Test reading long data
			value = BitUtils.ReadBits(data, 8, ref offset);
			Assert.AreEqual(36, offset);
			Assert.AreEqual(0x34, value);

			// Test reading within 1 byte half full
			value = BitUtils.ReadBits(data, 3, ref offset);
			Assert.AreEqual(39, offset);
			Assert.AreEqual(7, value);
		}

		[Test]
		public void TestReadWriteFloat()
		{
			byte[] data = new byte[8];
			int offset = 0;

			// Test writing generic float
			BitUtils.WriteFloat(data, 0x12, 1, 8, ref offset);
			Assert.AreEqual(8, offset);
			Assert.AreEqual(0x12, data[0]);

			// Test writing degree
			BitUtils.WriteFloat(data, 180, BitUtils.Deg2Byte, 8, ref offset);
			Assert.AreEqual(16, offset);
			Assert.AreEqual(0x80, data[1]);

			offset = 0;

			// Test reading generic float
			float value = BitUtils.ReadFloat(data, 1, 8, ref offset);
			Assert.AreEqual(8, offset);
			Assert.AreEqual(0x12, value);

			// Test reading degree
			value = BitUtils.ReadFloat(data, BitUtils.Deg2Byte, 8, ref offset);
			Assert.AreEqual(16, offset);
			Assert.AreEqual(180, value);
		}
	}
}