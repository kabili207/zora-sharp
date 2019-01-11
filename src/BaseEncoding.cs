using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Encodes bytes to characters, or characters to bytes.
	/// Only used for Link and Child names; not for secret encoding.
	/// </summary>
	public abstract class BaseEncoding : Encoding
	{

		/// <summary>
		/// Gets the list of characters in this encoding scheme
		/// </summary>
		protected abstract char[] Characters { get; }

		/// <summary>
		/// Calculates the number of bytes produced by encoding a set of characters from the specified character array.
		/// </summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="chars">The character array containing the set of characters to encode.</param>
		/// <param name="index">The index of the first character to encode.</param>
		/// <param name="count">The number of characters to encode.</param>
		public override int GetByteCount(char[] chars, int index, int count)
		{
			return count;
		}

		/// <summary>
		/// Encodes a set of characters from the specified character array into the specified byte array.
		/// </summary>
		/// <returns>The actual number of bytes written into bytes.</returns>
		/// <param name="chars">The character array containing the set of characters to encode.</param>
		/// <param name="charIndex">The index of the first character to encode.</param>
		/// <param name="charCount">The number of characters to encode.</param>
		/// <param name="bytes">The byte array to contain the resulting sequence of bytes.</param>
		/// <param name="byteIndex">The index at which to start writing the resulting sequence of bytes.</param>
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			for (int i = charIndex; i < charCount; i++)
			{
				byte b = 0;
				char c = chars[i];

				if (c == 0)
				{
					b = 0;
				}
				else
				{
					c = NormalizeChar(c);
					for (int j = 0; j < Characters.Length; j++)
					{
						if (Characters[j] == c)
						{
							b = (byte)( j + 0x10 );
							break;
						}
					}
				}

				bytes[i] = b;
			}

			return charCount;
		}

		/// <summary>
		/// Normalizes the input character prior to encoding
		/// </summary>
		/// <param name="input">The input character</param>
		/// <returns>A normalized version of the char passed in</returns>
		protected virtual char NormalizeChar(char input) => input;

		/// <summary>
		/// Calculates the number of characters produced by decoding a sequence of bytes from the specified byte array.
		/// </summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <param name="index">The index of the first byte to decode.</param>
		/// <param name="count">The number of bytes to decode.</param>
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return count;
		}

		/// <summary>
		/// Decodes a sequence of bytes from the specified byte array into the specified character array.
		/// </summary>
		/// <returns>The actual number of characters written into chars.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <param name="byteIndex">The index of the first byte to decode.</param>
		/// <param name="byteCount">The number of bytes to decode.</param>
		/// <param name="chars">The character array to contain the resulting set of characters.</param>
		/// <param name="charIndex">The index at which to start writing the resulting set of characters.</param>
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			for (int i = byteIndex; i < byteCount; i++)
			{
				int b = bytes[i] - 0x10;
				if (b < 0 || b >= Characters.Length)
				{
					chars[i] = '\0';
				}
				else
				{
					chars[i] = Characters[b];
				}
			}

			return byteCount;
		}

		/// <summary>
		/// Calculates the maximum number of bytes produced by encoding the
		/// specified number of characters.
		/// </summary>
		/// <returns>The maximum number of bytes produced by encoding the specified number of characters.</returns>
		/// <param name="charCount">The number of characters to encode.</param>
		public override int GetMaxByteCount(int charCount)
		{
			return charCount;
		}

		/// <summary>
		/// Calculates the maximum number of characters produced by decoding the specified number of bytes.
		/// </summary>
		/// <returns>The number of bytes to decode.</returns>
		/// <param name="byteCount">The maximum number of characters produced by decoding the specified number of bytes.</param>
		public override int GetMaxCharCount(int byteCount)
		{
			return byteCount;
		}
	}
}
