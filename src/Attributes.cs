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

namespace Zyrenth.Zora
{
	/// <summary>
	/// Provides additional information about a <see cref="Rings"/> value
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class RingInfoAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the ring name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the ring description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="RingInfoAttribute"/> class.
		/// </summary>
		/// <param name="name">The ring name.</param>
		/// <param name="description">The ring description.</param>
		public RingInfoAttribute(string name, string description)
		{
			Name = name;
			Description = description;
		}
	}
}
