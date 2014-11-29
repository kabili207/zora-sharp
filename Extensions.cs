using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Zyrenth.OracleHack
{
	public static class Extensions
	{
		
		public static IEnumerable<bool> GetBits(this byte b)
		{
			for (int i = 0; i < 8; i++)
			{
				yield return (b & 0x80) != 0;
				b *= 2;
			}
		}

		public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0) return min;
			else if (val.CompareTo(max) > 0) return max;
			else return val;
		}

		public static string Reverse(this string value)
		{
			return new string(Enumerable.Reverse(value).ToArray());
		}

		public static string ReversedSubstring(this string value, int start, int length)
		{
			return new string(Enumerable.Reverse(value.Substring(start, length)).ToArray());
		}
		

		#region Gets the build date and time (by reading the COFF header)

		// http://msdn.microsoft.com/en-us/library/ms680313

		struct _IMAGE_FILE_HEADER
		{
			public ushort Machine;
			public ushort NumberOfSections;
			public uint TimeDateStamp;
			public uint PointerToSymbolTable;
			public uint NumberOfSymbols;
			public ushort SizeOfOptionalHeader;
			public ushort Characteristics;
		};

		public static DateTime GetBuildDateTime(this Assembly assembly)
		{
			if (File.Exists(assembly.Location))
			{
				var buffer = new byte[Math.Max(Marshal.SizeOf(typeof(_IMAGE_FILE_HEADER)), 4)];
				using (var fileStream = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read))
				{
					fileStream.Position = 0x3C;
					fileStream.Read(buffer, 0, 4);
					fileStream.Position = BitConverter.ToUInt32(buffer, 0); // COFF header offset
					fileStream.Read(buffer, 0, 4); // "PE\0\0"
					fileStream.Read(buffer, 0, buffer.Length);
				}
				var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
				try
				{
					var coffHeader = (_IMAGE_FILE_HEADER)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(_IMAGE_FILE_HEADER));

					return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) + new TimeSpan(coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
				}
				finally
				{
					pinnedBuffer.Free();
				}
			}
			return new DateTime();
		}

		#endregion

	}
}
