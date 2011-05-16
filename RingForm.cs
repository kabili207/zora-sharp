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
		public RingForm(bool[] selectedRings)
		{
			InitializeComponent();
			for (int i =0; i < selectedRings.Count() && i < checkedListBox1.Items.Count; i++)
			{
				checkedListBox1.SetItemChecked(i,selectedRings[i]);
			}
		}

		private void RingForm_Load(object sender, EventArgs e)
		{

		}
	}
}
