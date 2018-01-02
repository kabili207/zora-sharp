/*
 *  Copyright © 2013-2015, Andrew Nagle.
 *  All rights reserved.
 *
 *  This file is part of ZoraSharp.
 *
 *  ZoraSharp is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  ZoraSharp is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with ZoraSharp. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Text;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Encodes bytes to characters, or characters to bytes, for US and PAL text.
	/// Only used for Link and Child names; not for secret encoding.
	/// </summary>
	public class USEncoding : Encoding
	{
		char[] characters =
		{
			'●','♣','♦','♠','\0','↑','↓','←','→','\0','\0','「','」','·','\0','。',
			' ','!','"','#','$','%','&','\'','(',')','*','+',',','-','.','/',
			'0','1','2','3','4','5','6','7','8','9',':',';','<','=','>','?',
			'@','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O',
			'P','Q','R','S','T','U','V','W','X','Y','Z','[','~',']','^','\0',
			'\0','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o',
			'p','q','r','s','t','u','v','w','x','y','z','{','￥','}','▲','■',
			'À','Â','Ä','Æ','Ç','È','É','Ê','Ë','Î','Ï','Ñ','Ö','Œ','Ù','Û',
			'Ü','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0',
			'à','â','ä','æ','ç','è','é','ê','ë','î','ï','ñ','ö','œ','ù','û',
			'ü','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','♥','\0','\0'
		};

		public USEncoding()
		{
		}

		public override int GetByteCount(char[] chars, int index, int count)
		{
			return count;
		}

		/// <summary>
		/// Converts a char array to a byte array.
		/// </summary>
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			for (int i = charIndex; i < charCount; i++)
			{
				byte b = 0;
				char c = chars[i];

				if (c == 0)
					b = 0;
				else {
					for (int j = 0; j < characters.Length; j++)
					{
						if (characters[j] == c)
						{
							b = (byte)(j + 0x10);
							break;
						}
					}
				}

				bytes[i] = b;
			}

			return charCount;
		}

		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return count;
		}

		/// <summary>
		/// Converts a byte array to a char array.
		/// </summary>
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			for (int i = byteIndex; i < byteCount; i++)
			{
				int b = bytes[i] - 0x10;
				if (b < 0 || b >= characters.Length)
					chars[i] = '\0';
				else
					chars[i] = characters[b];
			}

			return byteCount;
		}

		public override int GetMaxByteCount(int charCount)
		{
			return charCount;
		}

		public override int GetMaxCharCount(int byteCount)
		{
			return byteCount;
		}
	}
}
