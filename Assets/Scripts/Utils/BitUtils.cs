namespace Utils
{
	public static class BitUtils
	{
		/// <summary>
		///     Converts a degree to a byte.
		/// </summary>
		public const float Deg2Byte = 256f / 360;

		/// <summary>
		///     Writes bits to a byte array.
		/// </summary>
		/// <param name="data">The byte array to write to</param>
		/// <param name="value">The value to write to the array</param>
		/// <param name="length">The length of bits to write</param>
		/// <param name="offset">The index (in bits) to start writing from</param>
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

		/// <summary>
		///     Reads bits from a byte array.
		/// </summary>
		/// <param name="data">The byte array to read from</param>
		/// <param name="length">The length of bits to read</param>
		/// <param name="offset">The index (in bits) to start reading from</param>
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

		/// <summary>
		///     Cuts a float to the necessary precision before writing it to a byte array.
		/// </summary>
		/// <param name="data">The byte array to write to</param>
		/// <param name="value">The float value</param>
		/// <param name="precision">Multiplier to number before cutting</param>
		/// <param name="length">Number of bits that should be sent</param>
		/// <param name="offset">The index (in bits) to start writing from</param>
		public static void WriteFloat(byte[] data, float value, float precision, int length, ref int offset)
		{
			int intVal = (int)(value * precision);
			WriteBits(data, intVal, length, ref offset);
		}

		/// <summary>
		///     Reads a float with given precision and length from a byte array.
		/// </summary>
		/// <param name="data">The byte array to write to</param>
		/// <param name="precision">Multiplier to number before cutting</param>
		/// <param name="length">Number of bits that should be sent</param>
		/// <param name="offset">The index (in bits) to start writing from</param>
		public static float ReadFloat(byte[] data, float precision, int length, ref int offset)
		{
			int intVal = ReadBits(data, length, ref offset);
			return intVal / precision;
		}
	}
}