using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Zyrenth.OracleHack
{
	/// <summary>
	/// Provides quick access to details of an assembly
	/// </summary>
	public class AssemblyDetail
	{
		Assembly _asm;

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
				string result = string.Empty;
				Version version = _asm.GetName().Version;
				if (version != null)
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
			if (asm == null)
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
