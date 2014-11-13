using System;
using System.Collections.Generic;
using System.Drawing;
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

		public static string ReversedSubstring(this string value, int start, int length)
		{
			return new string(value.Substring(start, length).Reverse().ToArray());
		}

		/// <summary>
		/// Gets the Bitmap image associated with the specified ring
		/// </summary>
		/// <param name="ring">The ring</param>
		/// <returns>The bitmap image for the ring</returns>
		/// <remarks>
		/// This method only works with a single value. If the ring flags
		/// contains more than one value, no bitmap will be returned.
		/// </remarks>
		public static Bitmap GetImage(this Rings ring)
		{
			switch (ring)
			{
				case Rings.ArmorRingL1:
					return Images.Rings.Armor_Ring_L_1;
				case Rings.ArmorRingL2:
					return Images.Rings.Armor_Ring_L_2;
				case Rings.ArmorRingL3:
					return Images.Rings.Armor_Ring_L_3;
				case Rings.BlastRing:
					return Images.Rings.Blast_Ring;
				case Rings.BlueHolyRing:
					return Images.Rings.Blue_Holy_Ring;
				case Rings.BlueJoyRing:
					return Images.Rings.Blue_Joy_Ring;
				case Rings.BlueLuckRing:
					return Images.Rings.Blue_Luck_Ring;
				case Rings.BlueRing:
					return Images.Rings.Blue_Ring;
				case Rings.BombersRing:
					return Images.Rings.Bomber_s_Ring;
				case Rings.BombproofRing:
					return Images.Rings.Bombproof_Ring;
				case Rings.ChargeRing:
					return Images.Rings.Charge_Ring;
				case Rings.CursedRing:
					return Images.Rings.Cursed_Ring;
				case Rings.DiscoveryRing:
					return Images.Rings.Discovery_Ring;
				case Rings.DoubleEdgeRing:
					return Images.Rings.Dbl__Edged_Ring;
				case Rings.EnergyRing:
					return Images.Rings.Energy_Ring;
				case Rings.ExpertsRing:
					return Images.Rings.Expert_s_Ring;
				case Rings.FirstGenRing:
					return Images.Rings.First_Gen_Ring;
				case Rings.FistRing:
					return Images.Rings.Fist_Ring;
				case Rings.FriendshipRing:
					return Images.Rings.Friendship_Ring;
				case Rings.GashaRing:
					return Images.Rings.Gasha_Ring;
				case Rings.GBANatureRing:
					return Images.Rings.GBA_Nature_Ring;
				case Rings.GBATimeRing:
					return Images.Rings.GBA_Time_Ring;
				case Rings.GoldJoyRing:
					return Images.Rings.Gold_Joy_Ring;
				case Rings.GoldLuckRing:
					return Images.Rings.Gold_Luck_Ring;
				case Rings.GreenHolyRing:
					return Images.Rings.Green_Holy_Ring;
				case Rings.GreenJoyRing:
					return Images.Rings.Green_Joy_Ring;
				case Rings.GreenLuckRing:
					return Images.Rings.Green_Luck_Ring;
				case Rings.GreenRing:
					return Images.Rings.Green_Ring;
				case Rings.HeartRingL1:
					return Images.Rings.Heart_Ring_L_1;
				case Rings.HeartRingL2:
					return Images.Rings.Heart_Ring_L_2;
				case Rings.HundredthRing:
					return Images.Rings._100th_Ring;
				case Rings.LightRingL1:
					return Images.Rings.Light_Ring_L_1;
				case Rings.LightRingL2:
					return Images.Rings.Light_Ring_L_2;
				case Rings.LikeLikeRing:
					return Images.Rings.Like_Like_Ring;
				case Rings.MaplesRing:
					return Images.Rings.Maple_s_Ring;
				case Rings.MoblinRing:
					return Images.Rings.Moblin_Ring;
				case Rings.OctoRing:
					return Images.Rings.Octo_Ring;
				case Rings.PeaceRing:
					return Images.Rings.Peace_Ring;
				case Rings.PegasusRing:
					return Images.Rings.Pegasus_Ring;
				case Rings.PowerRingL1:
					return Images.Rings.Power_Ring_L_1;
				case Rings.PowerRingL2:
					return Images.Rings.Power_Ring_L_2;
				case Rings.PowerRingL3:
					return Images.Rings.Power_Ring_L_3;
				case Rings.ProtectionRing:
					return Images.Rings.Protection_Ring;
				case Rings.QuicksandRing:
					return Images.Rings.Quicksand_Ring;
				case Rings.RangRingL1:
					return Images.Rings.Rang_Ring_L_1;
				case Rings.RangRingL2:
					return Images.Rings.Rang_Ring_L_2;
				case Rings.RedHolyRing:
					return Images.Rings.Red_Holy_Ring;
				case Rings.RedJoyRing:
					return Images.Rings.Red_Joy_Ring;
				case Rings.RedLuckRing:
					return Images.Rings.Red_Luck_Ring;
				case Rings.RedRing:
					return Images.Rings.Red_Ring;
				case Rings.RocsRing:
					return Images.Rings.Roc_s_Ring;
				case Rings.RupeeRing:
					return Images.Rings.Rupee_Ring;
				case Rings.SignRing:
					return Images.Rings.Sign_Ring;
				case Rings.SlayersRing:
					return Images.Rings.Slayer_s_Ring;
				case Rings.SnowshoeRing:
					return Images.Rings.Snowshoe_Ring;
				case Rings.SpinRing:
					return Images.Rings.Spin_Ring;
				case Rings.SteadfastRing:
					return Images.Rings.Steadfast_Ring;
				case Rings.SubrosianRing:
					return Images.Rings.Subrosian_Ring;
				case Rings.SwimmersRing:
					return Images.Rings.Swimmer_s_Ring;
				case Rings.TossRing:
					return Images.Rings.Toss_Ring;
				case Rings.VictoryRing:
					return Images.Rings.Victory_Ring;
				case Rings.WhimsicalRing:
					return Images.Rings.Whimsical_Ring;
				case Rings.WhispRing:
					return Images.Rings.Whisp_Ring;
				case Rings.ZoraRing:
					return Images.Rings.Zora_Ring;
				default:
					return null;
			}
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
