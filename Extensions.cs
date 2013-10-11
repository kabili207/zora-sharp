using System;
using System.Collections.Generic;
using System.Drawing;
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

	}
}
