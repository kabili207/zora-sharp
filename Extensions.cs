using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zyrenth.OracleHack
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

		/// <summary>
		/// Splits the specified array into a multi-dimensional array with the
		/// specified number of rows and columns.
		/// </summary>
		/// <param name='arrayIn'>The input array</param>
		/// <param name='rows'>The number of rows in the resulting array</param>
		/// <param name='columns'>The number of columns in the resulting array</param>
		/// <typeparam name='T'>The type of the array</typeparam>
		/// <exception cref='ArgumentException'>
		/// Is thrown when an argument passed to a method is invalid.
		/// </exception>
		public static T[,] Split<T>(this T[] arrayIn, int rows, int columns)
		{
			if (arrayIn.Length != rows * columns)
				throw new ArgumentException("The array length does not match the specified dimensions");
			
			T[,] newArray = new T[rows, columns];
			
			int curIndex = 0;
			for (int i = 0; i < columns; i++)
			{
				for (int j = 0; j < rows; j++, curIndex++)
					newArray[i, j] = arrayIn[curIndex];
			}
			
			return newArray;
		}
		
		public static IEnumerable<bool> GetBits(this byte b)
		{
			for (int i = 0; i < 8; i++)
			{
				yield return (b & 0x80) != 0;
				b *= 2;
			}
		}
	}
}
