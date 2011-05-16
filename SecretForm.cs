using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace OracleHack
{
	public partial class SecretForm : Form
	{
		public SecretForm()
		{
			InitializeComponent();
		}

		private void btn63_Click(object sender, EventArgs e)
		{
			Control ctl = sender as Control;
			if (ctl != null)
			{
				string num = Regex.Replace(ctl.Name, @"\D", "");
				int id = int.Parse(num);
				txtSymbolId.Text = num;
				txtSymbolHex.Text = new string(Convert.ToString(id, 2).PadLeft(6, '0').Reverse().ToArray());
				//txtSymbolHex.Text = Convert.ToString(id, 2).PadLeft(6, '0');
				/*pics[currentPic].Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("_" + num);
				if (currentPic < 19)
				{
					//txtBinary.Text += txtSymbolHex.Text;
					currentPic++;
				}
				else
				{
					//txtBinary.Text = txtBinary.Text.Remove(txtBinary.Text.Length - 7);
				}*/
				txtBinary.Text += txtSymbolHex.Text;
				lblBinaryCount.Text = txtBinary.Text.Length.ToString();
			}
		}
	}
}
