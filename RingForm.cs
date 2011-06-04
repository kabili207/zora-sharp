using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OracleHack
{
	public partial class RingForm : Form
	{
		private bool[] _selectedRings;
		public bool[] SelectedRings
		{
			get { return _selectedRings; }
			set
			{
				_selectedRings = value;
				for (int i = 0; i < value.Count() && i < lstRings.Items.Count; i++)
				{
					lstRings.SetItemChecked(i, value[i]);
				}
			}
		}

		public RingForm()
		{
			InitializeComponent();
		}

		private void RingForm_Load(object sender, EventArgs e)
		{

		}

		private void lstRings_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			try
			{
				SelectedRings[e.Index] = e.NewValue == CheckState.Checked;
			}
			catch
			{

			}
		}
	}
}
