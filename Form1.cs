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
	public partial class Form1 : Form
	{
		PictureBox[] pics;
		int currentPic = 0;

		string heroName = "";
		string kidName = "";
		int GameId = 0;
		bool[] ringBits = new bool[64];
		public Form1()
		{
			InitializeComponent();
			pics = new PictureBox[] { pic00, pic01, pic02, pic03, pic04, pic05, pic06, pic07, pic08, pic09,
				pic10, pic11, pic12, pic13, pic14, pic15, pic16, pic17, pic18, pic19 };
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			using (FileStream fsSource = new FileStream(@"C:\Users\kabili\Documents\Projects\C-Sharp\OracleHack\OracleHack\ages_old.sav",
			FileMode.Open, FileAccess.Read))
			{
				fsSource.Seek(96, SeekOrigin.Begin);
				// Read the source file into a byte array.
				byte[] gameIdBytes = new byte[2];
				int numBytesToRead = 2;
				int numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					// Read may return anything from 0 to numBytesToRead.
					int n = fsSource.Read(gameIdBytes, numBytesRead, numBytesToRead);

					// Break when the end of the file is reached.
					if (n == 0)
						break;

					numBytesRead += n;
					numBytesToRead -= n;
				}

				GameId = BitConverter.ToInt16(gameIdBytes, 0);
				nudGameId.Value = GameId;
				fsSource.Seek(98, SeekOrigin.Begin);
				// Read the source file into a byte array.
				byte[] heroBytes = new byte[5];
				numBytesToRead = 5;
				numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					// Read may return anything from 0 to numBytesToRead.
					int n = fsSource.Read(heroBytes, numBytesRead, numBytesToRead);

					// Break when the end of the file is reached.
					if (n == 0)
						break;

					numBytesRead += n;
					numBytesToRead -= n;
				}

				heroName = System.Text.Encoding.ASCII.GetString(heroBytes);
				txtHero.Text = heroName;
				fsSource.Seek(105, SeekOrigin.Begin);
				// Read the source file into a byte array.
				byte[] kidBytes = new byte[5];
				numBytesToRead = 5;
				numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					// Read may return anything from 0 to numBytesToRead.
					int n = fsSource.Read(kidBytes, numBytesRead, numBytesToRead);

					// Break when the end of the file is reached.
					if (n == 0)
						break;

					numBytesRead += n;
					numBytesToRead -= n;
				}

				kidName = System.Text.Encoding.ASCII.GetString(kidBytes);
				txtKid.Text = kidName;

				fsSource.Seek(118, SeekOrigin.Begin);
				// Read the source file into a byte array.
				byte[] ringBytes = new byte[8];
				numBytesToRead = 8;
				numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					// Read may return anything from 0 to numBytesToRead.
					int n = fsSource.Read(ringBytes, numBytesRead, numBytesToRead);

					// Break when the end of the file is reached.
					if (n == 0)
						break;

					numBytesRead += n;
					numBytesToRead -= n;
				}
				// Write the byte array to the other FileStream.
				ringBits = ringBytes.SelectMany(GetBits).ToArray();
				bool[][] ringBitArray = Split<bool>(ringBits, 8);
				for (int i = 0; i < ringBitArray.Count(); i++ )
				{
					ringBitArray[i] = ringBitArray[i].Reverse().ToArray();
				}
				ringBits = ringBitArray[0].Concat(ringBitArray[1]).Concat(ringBitArray[2]).Concat(ringBitArray[3])
						.Concat(ringBitArray[4]).Concat(ringBitArray[5]).Concat(ringBitArray[6]).Concat(ringBitArray[7]).ToArray();
			}
		}

		public static T[][] Split<T>(T[] arrayIn, int length)
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
			RingForm form = new RingForm(ringBits);
			form.ShowDialog();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			SecretForm form = new SecretForm();
			form.ShowDialog();
		}
	}
}
