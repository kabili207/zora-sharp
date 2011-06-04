using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

namespace OracleHack
{
	public partial class DecoderForm : Form
	{
		PictureBox[] pics;
		int currentPic = 0;

		string heroName = "";
		string kidName = "";
		int GameId = 0;
		bool[] ringBits = new bool[64];
		public DecoderForm()
		{
			InitializeComponent();
			pics = new PictureBox[] { pic00, pic01, pic02, pic03, pic04, pic05, pic06, pic07, pic08, pic09,
				pic10, pic11, pic12, pic13, pic14, pic15, pic16, pic17, pic18, pic19 };
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
		}

		IEnumerable<bool> GetBits(byte b)
		{
			for (int i = 0; i < 8; i++)
			{
				yield return (b & 0x80) != 0;
				b *= 2;
			}
		}

		private void SymbolButton_Click(object sender, EventArgs e)
		{
			Control ctl = sender as Control;
			if (ctl != null)
			{
				string num = Regex.Replace(ctl.Name,@"\D", "");
				int id = int.Parse(num);
				txtSymbolId.Text = num;
				txtSymbolHex.Text = new string(Convert.ToString(id, 2).PadLeft(6, '0').Reverse().ToArray());
				//txtSymbolHex.Text = Convert.ToString(id, 2).PadLeft(6, '0');
				pics[currentPic].Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("_" + num);
				if (currentPic < 19)
				{
					//txtBinary.Text += txtSymbolHex.Text;
					currentPic++;
				}
				else
				{
					//txtBinary.Text = txtBinary.Text.Remove(txtBinary.Text.Length - 7);
				}
				txtBinary.Text += txtSymbolHex.Text;
				lblBinaryCount.Text = txtBinary.Text.Length.ToString();
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			foreach (PictureBox pic in pics)
			{
				pic.Image = null;
			}
			txtBinary.Text = null;
			txtAscii.Text = null;
		}

		private void btnBack_Click(object sender, EventArgs e)
		{
			if (pics[currentPic].Image == null && currentPic > 0)
				currentPic--;

			pics[currentPic].Image = null;
			if (currentPic > 0)
				currentPic--;
		}

		private void btnRings_Click(object sender, EventArgs e)
		{
			RingForm form = new RingForm();
			form.SelectedRings = ringBits;
			form.ShowDialog();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			SecretForm form = new SecretForm();
			form.ShowDialog();
		}
	}
}
