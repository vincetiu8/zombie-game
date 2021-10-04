namespace Utils
{
	public static class BitUtils
	{
		public static void WriteBits(byte[] data, int value, int length, ref int offset)
		{
			int byteIndex = offset / 8;
			int space = offset % 8;
			offset += length;

			// Handle remaining space in other byte
			if (space > 0)
			{
				byte mask = (byte)((1 << space) - 1);
				int cutVal = mask & value;

				if (space > length) cutVal <<= space - length;

				data[byteIndex] += (byte)cutVal;

				// If all length used up, end early
				if (space >= length) return;

				value >>= space;
				length -= space;
				byteIndex++;
			}

			// Loop for amount of data in full bytes
			while (length >= 8)
			{
				data[byteIndex] = (byte)value;
				value >>= 8;
				byteIndex++;
				length -= 8;
			}

			if (length == 0) return;

			// Handle remaining value
			value <<= 8 - length;
			data[byteIndex] = (byte)value;
		}

		public static int ReadBits(byte[] data, int length, ref int offset)
		{
			int value = 0;
			int byteIndex = offset / 8;
			int space = offset % 8;
			offset += length;

			int usedLength = 0;

			// Handle remaining space in other byte
			if (space > 0)
			{
				byte mask = (byte)((1 << space) - 1);
				int cutVal = mask & data[byteIndex];
				value = cutVal;

				// If all length used up, end early
				if (space >= length)
				{
					// Make sure to delete extra bits from the end
					if (space > length) value >>= space - length;

					return value;
				}

				usedLength = space;
				byteIndex++;
			}

			// Loop for amount of data in full bytes
			while (length - usedLength >= 8)
			{
				value += data[byteIndex] << usedLength;
				byteIndex++;
				usedLength += 8;
			}

			if (length == 0) return value;

			// Handle remaining value
			value += (data[byteIndex] >> (8 - length + usedLength)) << usedLength;
			return value;
		}
	}
}