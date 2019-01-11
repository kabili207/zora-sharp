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
	/// Encodes bytes to characters, or characters to bytes, for US and PAL text.
	/// Only used for Link and Child names; not for secret encoding.
	/// </summary>
	public class USEncoding : BaseEncoding
	{
		private readonly char[] characters =
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

		/// <summary>
		/// Gets the list of characters in this encoding scheme
		/// </summary>
		protected override char[] Characters => characters;
	}
}
