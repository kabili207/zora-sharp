/*
 *  Copyright © 2013-2018, Amy Nagle.
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
	/// Encodes bytes to characters, or characters to bytes, for japanese text.
	/// Only used for Link and Child names; not for secret encoding.
	/// </summary>
	public class JapaneseEncoding : Encoding
	{
		char[] characters =
		{
			'\0','\0','\0','\0','♥','↑','↓','←','→','\0','\0','「','」','\0','\0','。',
			' ','!','"','#','$','%','&','\'','(',')','*','+',',','-','.','/',
			'0','1','2','3','4','5','6','7','8','9',':',';','<','=','>','?',
			'@','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O',
			'P','Q','R','S','T','U','V','W','X','Y','Z','[','~',']','^','\0',
			'あ','い','う','え','お','か','き','く','け','こ','さ','し','す','せ','そ','た',
			'ち','つ','て','と','な','に','ぬ','ね','の','は','ひ','ふ','へ','ほ','ま','み',
			'む','め','も','や','ゆ','よ','ら','り','る','れ','ろ','を','わ','ん','ぁ','ぃ',
			'ぅ','ぇ','ぉ','っ','ゃ','ゅ','ょ','が','ぎ','ぐ','げ','ご','ざ','じ','ず','ぜ',
			'ぞ','だ','ぢ','づ','で','ど','ば','び','ぶ','べ','ぼ','ぱ','ぴ','ぷ','ぺ','ぽ',
			'ア','イ','ウ','エ','オ','カ','キ','ク','ケ','コ','サ','シ','ス','セ','ソ','タ',
			'チ','ツ','テ','ト','ナ','ニ','ヌ','ネ','ノ','ハ','ヒ','フ','ヘ','ホ','マ','ミ',
			'ム','メ','モ','ヤ','ユ','ヨ','ラ','リ','ル','レ','ロ','ワ','ヲ','ン','ァ','ィ',
			'ゥ','ェ','ォ','ッ','ャ','ュ','ョ','ガ','ギ','グ','ゲ','ゴ','ザ','ジ','ズ','ゼ',
			'ゾ','ダ','ヂ','ヅ','デ','ド','バ','ビ','ブ','ベ','ボ','パ','ピ','プ','ペ','ポ',
		};

		public JapaneseEncoding ()
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
				if (c >= 'a' && c <= 'z') {
					c -= 'a';
					c += 'A';
				}

				if (c == 0)
					b = 0;
				else {
					for (int j=0; j<characters.Length; j++)
					{
						if (characters[j] == c) {
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
				int b = bytes[i]-0x10;
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
