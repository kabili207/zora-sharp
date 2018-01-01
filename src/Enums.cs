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
using System.Collections.Generic;
using System.Linq;
using System.Text;

// I Really don't feel like documenting all these enums values right now...
#pragma warning disable 1591 

namespace Zyrenth.Zora
{

	/// <summary>
	/// Specifies the game in the Zelda Oracle series.
	/// </summary>
	public enum Game
	{
		Ages = 0,
		Seasons = 1
	}

	/// <summary>
	/// Specifies the animal friend.
	/// </summary>
	public enum Animal : byte
	{
		Ricky = 0x0b,
		Dimitri = 0x0c,
		Moosh = 0x0d,
		None = 0
	}

	/// <summary>
	/// Specifies the child's behavior.
	/// </summary>
	public enum ChildBehavior : byte
	{
		Infant = 0,
		BouncyA,
		BouncyB,
		BouncyC,
		BouncyD,
		BouncyE,
		ShyA,
		ShyB,
		ShyC,
		ShyD,
		ShyE,
		HyperA,
		HyperB,
		HyperC,
		HyperD,
		HyperE
	}

	/// <summary>
	/// Specifies the type of memory.
	/// </summary>
	public enum Memory : byte
	{
		ClockShopKingZora = 0,
		GraveyardFairy = 1,
		SubrosianTroy = 2,
		DiverPlen = 3,
		SmithLibrary = 4,
		PirateTokay = 5,
		TempleMamamu = 6,
		DekuTingle = 7,
		BiggoronElder = 8,
		RuulSymmetry = 9
	}

	/// <summary>
	/// <para>
	/// Specifies which rings a player has obtained.
	/// </para>
	/// <para>
	/// This enumeration has a <see cref="FlagsAttribute"/> attribute that
	/// allows a bitwise combination of its member values.
	/// </para>
	/// </summary>
	[Flags]
	public enum Rings : long
	{
		None = 0,
		All = unchecked((long)UInt64.MaxValue),
		[RingInfo("Friendship Ring", "Symbol of a meeting")]
		FriendshipRing = 0x1L,
		[RingInfo("Power Ring L-1", "Sword damage ▲\nDamage taken ▲")]
		PowerRingL1 = 0x2L,
		[RingInfo("Power Ring L-2", "Sword damage ▲▲\nDamage taken ▲▲")]
		PowerRingL2 = 0x4L,
		[RingInfo("Power Ring L-3", "Sword damage ▲▲▲\nDamage taken ▲▲▲")]
		PowerRingL3 = 0x8L,
		[RingInfo("Armor Ring L-1", "Sword Damage ▼\nDamage taken ▼")]
		ArmorRingL1 = 0x10L,
		[RingInfo("Armor Ring L-2", "Sword Damage ▼▼\nDamage taken ▼▼")]
		ArmorRingL2 = 0x20L,
		[RingInfo("Armor Ring L-3", "Sword Damage ▼▼▼\nDamage taken ▼▼▼")]
		ArmorRingL3 = 0x40L,
		[RingInfo("Red Ring", "Sword Damage x2")]
		RedRing = 0x80L,
		[RingInfo("Blue Ring", "Damage taken reduced by 1/2")]
		BlueRing = 0x100L,
		[RingInfo("Green Ring", "Damage taken down by 25%\nSword damage up by 50%")]
		GreenRing = 0x200L,
		[RingInfo("Cursed Ring", "1/2 sword damage\nx2 damage taken")]
		CursedRing = 0x400L,
		[RingInfo("Expert's Ring", "Punch when unequipped")]
		ExpertsRing = 0x800L,
		[RingInfo("Blast Ring", "Bomb damage ▲")]
		BlastRing = 0x1000L,
		[RingInfo("Rang Ring L-1", "Boomerang damage ▲")]
		RangRingL1 = 0x2000L,
		[RingInfo("GBA Time Ring", "Life Advanced!")]
		GBATimeRing = 0x4000L,
		[RingInfo("Maple's Ring", "Maple meetings ▲")]
		MaplesRing = 0x8000L,
		[RingInfo("Steadfast Ring", "Get knocked back less")]
		SteadfastRing = 0x10000L,
		[RingInfo("Pegasus Ring", "Lengthen Pegasus Seed effect")]
		PegasusRing = 0x20000L,
		[RingInfo("Toss Ring", "Throwing distance ▲")]
		TossRing = 0x40000L,
		[RingInfo("Heart Ring L-1", "Slowly recover lost Hearts")]
		HeartRingL1 = 0x80000L,
		[RingInfo("Heart Ring L-2", "Recover lost Hearts")]
		HeartRingL2 = 0x100000L,
		[RingInfo("Swimmer's Ring", "Swimming speed ▲")]
		SwimmersRing = 0x200000L,
		[RingInfo("Charge Ring", "Spin Attack charges quickly")]
		ChargeRing = 0x400000L,
		[RingInfo("Light Ring L-1", "Sword beams at -2 Hearts")]
		LightRingL1 = 0x800000L,
		[RingInfo("Light Ring L-2", "Sword beams at -3 Hearts")]
		LightRingL2 = 0x1000000L,
		[RingInfo("Bomber's Ring", "Set two Bombs at once")]
		BombersRing = 0x2000000L,
		[RingInfo("Green Luck Ring", "1/2 damage from traps")]
		GreenLuckRing = 0x4000000L,
		[RingInfo("Blue Luck Ring", "1/2 damage from beams")]
		BlueLuckRing = 0x8000000L,
		[RingInfo("Gold Luck Ring", "1/2 damage from falls")]
		GoldLuckRing = 0x10000000L,
		[RingInfo("Red Luck Ring", "1/2 damage from spiked floors")]
		RedLuckRing = 0x20000000L,
		[RingInfo("Green Holy Ring", "No damage from electricity")]
		GreenHolyRing = 0x40000000L,
		[RingInfo("Blue Holy Ring", "No damage from Zora's fire")]
		BlueHolyRing = 0x80000000L,
		[RingInfo("Red Holy Ring", "No damage from small rocks")]
		RedHolyRing = 0x100000000L,
		[RingInfo("Snowshoe Ring", "No sliding on ice")]
		SnowshoeRing = 0x200000000L,
		[RingInfo("Roc's Ring", "Cracked floors don't crumble")]
		RocsRing = 0x400000000L,
		[RingInfo("Quicksand Ring", "No sinking in quicksand")]
		QuicksandRing = 0x800000000L,
		[RingInfo("Red Joy Ring", "Beasts drop double Rupees")]
		RedJoyRing = 0x1000000000L,
		[RingInfo("Blue Joy Ring", "Beasts drop double Hearts")]
		BlueJoyRing = 0x2000000000L,
		[RingInfo("Gold Joy Ring", "Find double items")]
		GoldJoyRing = 0x4000000000L,
		[RingInfo("Green Joy Ring", "Find double Ore Chunks")]
		GreenJoyRing = 0x8000000000L,
		[RingInfo("Discovery Ring", "Sense soft earth nearby")]
		DiscoveryRing = 0x10000000000L,
		[RingInfo("Rang Ring L-2", "Boomerang damage ▲▲")]
		RangRingL2 = 0x20000000000L,
		[RingInfo("Octo Ring", "Become an Octorok")]
		OctoRing = 0x40000000000L,
		[RingInfo("Moblin Ring", "Become a Moblin")]
		MoblinRing = 0x80000000000L,
		[RingInfo("Like Like Ring", "Become a Like-Like")]
		LikeLikeRing = 0x100000000000L,
		[RingInfo("Subrosian Ring", "Become a Subrosian")]
		SubrosianRing = 0x200000000000L,
		[RingInfo("First Gen Ring", "Become something")]
		FirstGenRing = 0x400000000000L,
		[RingInfo("Spin Ring", "Double Spin Attack")]
		SpinRing = 0x800000000000L,
		[RingInfo("Bombproof Ring", "No damage from your own Bombs")]
		BombproofRing = 0x1000000000000L,
		[RingInfo("Energy Ring", "Beam replaces Spin Attack")]
		EnergyRing = 0x2000000000000L,
		[RingInfo("Dbl. Edge Ring", "Sword damage ▲ but you get hurt")]
		DoubleEdgeRing = 0x4000000000000L,
		[RingInfo("GBA Nature Ring", "Life Advanced!")]
		GBANatureRing = 0x8000000000000L,
		[RingInfo("Slayer's Ring", "1000 beasts slain")]
		SlayersRing = 0x10000000000000L,
		[RingInfo("Rupee Ring", "10,000 Rupees collected")]
		RupeeRing = 0x20000000000000L,
		[RingInfo("Victory Ring", "The Evil King Ganon defeated")]
		VictoryRing = 0x40000000000000L,
		[RingInfo("Sign Ring", "100 signs broken")]
		SignRing = 0x80000000000000L,
		[RingInfo("100th Ring", "100 rings appraised")]
		HundredthRing = 0x100000000000000L,
		[RingInfo("Whisp Ring", "No effect from jinxes")]
		WhispRing = 0x200000000000000L,
		[RingInfo("Gasha Ring", "Grow great Gasha Trees")]
		GashaRing = 0x400000000000000L,
		[RingInfo("Peace Ring", "No explosion if holding Bomb")]
		PeaceRing = 0x800000000000000L,
		[RingInfo("Zora Ring", "Dive without breathing")]
		ZoraRing = 0x1000000000000000L,
		[RingInfo("Fist Ring", "Punch when not equipped")]
		FistRing = 0x2000000000000000L,
		[RingInfo("Whimsical Ring", "Sword damage ▼ Sometimes deadly")]
		WhimsicalRing = 0x4000000000000000L,
		[RingInfo("Protection Ring", "Damage taken is always one Heart")]
		ProtectionRing = unchecked((long)0x8000000000000000L)
	}
}
