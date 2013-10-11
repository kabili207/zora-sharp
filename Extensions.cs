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
					return RingImages.Armor_Ring_L_1;
				case Rings.ArmorRingL2:
					return RingImages.Armor_Ring_L_2;
				case Rings.ArmorRingL3:
					return RingImages.Armor_Ring_L_3;
				case Rings.BlastRing:
					return RingImages.Blast_Ring;
				case Rings.BlueHolyRing:
					return RingImages.Blue_Holy_Ring;
				case Rings.BlueJoyRing:
					return RingImages.Blue_Joy_Ring;
				case Rings.BlueLuckRing:
					return RingImages.Blue_Luck_Ring;
				case Rings.BlueRing:
					return RingImages.Blue_Ring;
				case Rings.BombersRing:
					return RingImages.Bomber_s_Ring;
				case Rings.BombproofRing:
					return RingImages.Bombproof_Ring;
				case Rings.ChargeRing:
					return RingImages.Charge_Ring;
				case Rings.CursedRing:
					return RingImages.Cursed_Ring;
				case Rings.DiscoveryRing:
					return RingImages.Discovery_Ring;
				case Rings.DoubleEdgeRing:
					return RingImages.Dbl__Edged_Ring;
				case Rings.EnergyRing:
					return RingImages.Energy_Ring;
				case Rings.ExpertsRing:
					return RingImages.Expert_s_Ring;
				case Rings.FirstGenRing:
					return RingImages.First_Gen_Ring;
				case Rings.FistRing:
					return RingImages.Fist_Ring;
				case Rings.FriendshipRing:
					return RingImages.Friendship_Ring;
				case Rings.GashaRing:
					return RingImages.Gasha_Ring;
				case Rings.GBANatureRing:
					return RingImages.GBA_Nature_Ring;
				case Rings.GBATimeRing:
					return RingImages.GBA_Time_Ring;
				case Rings.GoldJoyRing:
					return RingImages.Gold_Joy_Ring;
				case Rings.GoldLuckRing:
					return RingImages.Gold_Luck_Ring;
				case Rings.GreenHolyRing:
					return RingImages.Green_Holy_Ring;
				case Rings.GreenJoyRing:
					return RingImages.Green_Joy_Ring;
				case Rings.GreenLuckRing:
					return RingImages.Green_Luck_Ring;
				case Rings.GreenRing:
					return RingImages.Green_Ring;
				case Rings.HeartRingL1:
					return RingImages.Heart_Ring_L_1;
				case Rings.HeartRingL2:
					return RingImages.Heart_Ring_L_2;
				case Rings.HundredthRing:
					return RingImages._100th_Ring;
				case Rings.LightRingL1:
					return RingImages.Light_Ring_L_1;
				case Rings.LightRingL2:
					return RingImages.Light_Ring_L_2;
				case Rings.LikeLikeRing:
					return RingImages.Like_Like_Ring;
				case Rings.MaplesRing:
					return RingImages.Maple_s_Ring;
				case Rings.MoblinRing:
					return RingImages.Moblin_Ring;
				case Rings.OctoRing:
					return RingImages.Octo_Ring;
				case Rings.PeaceRing:
					return RingImages.Peace_Ring;
				case Rings.PegasusRing:
					return RingImages.Pegasus_Ring;
				case Rings.PowerRingL1:
					return RingImages.Power_Ring_L_1;
				case Rings.PowerRingL2:
					return RingImages.Power_Ring_L_2;
				case Rings.PowerRingL3:
					return RingImages.Power_Ring_L_3;
				case Rings.ProtectionRing:
					return RingImages.Protection_Ring;
				case Rings.QuicksandRing:
					return RingImages.Quicksand_Ring;
				case Rings.RangRingL1:
					return RingImages.Rang_Ring_L_1;
				case Rings.RangRingL2:
					return RingImages.Rang_Ring_L_2;
				case Rings.RedHolyRing:
					return RingImages.Red_Holy_Ring;
				case Rings.RedJoyRing:
					return RingImages.Red_Joy_Ring;
				case Rings.RedLuckRing:
					return RingImages.Red_Luck_Ring;
				case Rings.RedRing:
					return RingImages.Red_Ring;
				case Rings.RocsRing:
					return RingImages.Roc_s_Ring;
				case Rings.RupeeRing:
					return RingImages.Rupee_Ring;
				case Rings.SignRing:
					return RingImages.Sign_Ring;
				case Rings.SlayersRing:
					return RingImages.Slayer_s_Ring;
				case Rings.SnowshoeRing:
					return RingImages.Snowshoe_Ring;
				case Rings.SpinRing:
					return RingImages.Spin_Ring;
				case Rings.SteadfastRing:
					return RingImages.Steadfast_Ring;
				case Rings.SubrosianRing:
					return RingImages.Subrosian_Ring;
				case Rings.SwimmersRing:
					return RingImages.Swimmer_s_Ring;
				case Rings.TossRing:
					return RingImages.Toss_Ring;
				case Rings.VictoryRing:
					return RingImages.Victory_Ring;
				case Rings.WhimsicalRing:
					return RingImages.Whimsical_Ring;
				case Rings.WhispRing:
					return RingImages.Whisp_Ring;
				case Rings.ZoraRing:
					return RingImages.Zora_Ring;
				default:
					return null;
			}
		}

	}
}
