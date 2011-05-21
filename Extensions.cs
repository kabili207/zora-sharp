using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleHack
{
	public static class Extensions
	{

		public static T[][] Split<T>(this T[] arrayIn, int length)
		{
			bool even = arrayIn.Length % length == 0;
			int totalLength = arrayIn.Length / length;
			if (!even)
				totalLength++;

			T[][] newArray = new T[totalLength][];
			for (int i = 0; i < totalLength; ++i)
			{
				int allocLength = length;
				if (!even && i == totalLength - 1)
					allocLength = arrayIn.Length % length;

				newArray[i] = new T[allocLength];
				Array.Copy(arrayIn, i * length, newArray[i], 0, allocLength);
			}
			return newArray;
		}
	}
}
