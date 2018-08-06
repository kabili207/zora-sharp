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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Provides quick access to details of an assembly
	/// </summary>
	public class AssemblyDetail
	{
        private Assembly _asm;

		/// <summary>
		/// Gets the assembly company
		/// </summary>
		public string Company
		{
			get
			{
				return GetAttribute<AssemblyCompanyAttribute>(x => x.Company);
			}
		}

		/// <summary>
		/// Gets the assembly copyright information
		/// </summary>
		public string Copyright
		{
			get
			{
				return GetAttribute<AssemblyCopyrightAttribute>(x => x.Copyright);
			}
		}

		/// <summary>
		/// Gets the description about the assembly.
		/// </summary>
		public string Description
		{
			get
			{
				return GetAttribute<AssemblyDescriptionAttribute>(x => x.Description);
			}
		}

		/// <summary>
		/// Gets the assembly's file version
		/// </summary>
		public string FileVersion
		{
			get
			{
				Version version = _asm.GetName().Version;
				if (version is null)
					return version.ToString();
				else
					return "0.0.0.0";
			}
		}

		/// <summary>
		///  Gets the assembly's full name.
		/// </summary>
		public string Product
		{
			get
			{
				return GetAttribute<AssemblyProductAttribute>(x => x.Product);
			}
		}

		/// <summary>
		/// Gets the assembly's version.
		/// </summary>
		public string ProductVersion
		{
			get
			{
				return GetAttribute<AssemblyInformationalVersionAttribute>(x => x.InformationalVersion) ?? FileVersion;
			}
		}

		/// <summary>
		/// Gets the assembly title
		/// </summary>
		public string Title
		{
			get
			{
				return GetAttribute<AssemblyTitleAttribute>(a => a.Title);
			}
		}

		/// <summary>
		/// Gets the assembly trademark information
		/// </summary>
		public string Trademark
		{
			get
			{
				return GetAttribute<AssemblyTrademarkAttribute>(a => a.Trademark);
			}
		}

		/// <summary>
		/// Creates a new AssemblyDetail object from the specified Assembly
		/// </summary>
		/// <param name="asm">The assembly</param>
		public AssemblyDetail(Assembly asm)
		{
			if (asm is null)
				throw new ArgumentNullException("asm");
			_asm = asm;
		}

		private string GetAttribute<T>(Func<T, string> resolveFunc) where T : Attribute
		{
			object[] attribs = _asm.GetCustomAttributes(typeof(T), true);
			if (attribs.Length > 0)
			{
				return resolveFunc((T)attribs[0]);
			}
			return null;
		}
	}
}
