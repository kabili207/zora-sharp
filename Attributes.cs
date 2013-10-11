using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Zyrenth.OracleHack
{
	[AttributeUsage(AttributeTargets.Field)]
	public class RingInfoAttribute : Attribute
	{
		public string name;
		public string description;
		public Bitmap image;

		public RingInfoAttribute(string name, string description)
		{
			this.name = name;
			this.description = description;
		}
	}
}
