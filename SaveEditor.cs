using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace OracleHack
{
	public enum GameType : byte { None = 0, Seasons = 49, Ages = 50 };
	public enum AnimalType : byte { None = 0, Ricky = 11, Dimitri = 12, Moosh = 13 };

	public partial class SaveEditor : Form
	{
		private short GameId;
		private string heroName;
		private string kidName;
		private GameType version = GameType.None;
		private AnimalType animal;
		

		public string SaveFile { get; set; }

		public SaveEditor()
		{
			InitializeComponent();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			if (DialogResult.OK == dialog.ShowDialog())
			{
				SaveFile = dialog.FileName;
				LoadFile();
			}
		}

		private void LoadFile(int offset = 0)
		{
			
			using (FileStream fsSource = new FileStream(SaveFile, FileMode.Open, FileAccess.Read))
			{
				fsSource.Seek(19, SeekOrigin.Begin);
				// Read the source file into a byte array.
				byte[] versionBytes = new byte[1];
				int numBytesToRead = 1;
				int numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					// Read may return anything from 0 to numBytesToRead.
					int n = fsSource.Read(versionBytes, numBytesRead, numBytesToRead);

					// Break when the end of the file is reached.
					if (n == 0)
						break;

					numBytesRead += n;
					numBytesToRead -= n;
				}

				version = (GameType)versionBytes[0];


				fsSource.Seek(96, SeekOrigin.Begin);
				// Read the source file into a byte array.
				byte[] gameIdBytes = new byte[2];
				numBytesToRead = 2;
				numBytesRead = 0;
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
				//fsSource.Seek(98, SeekOrigin.Begin);
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
				//fsSource.Seek(105, SeekOrigin.Begin);
				fsSource.Seek(2, SeekOrigin.Current);
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


				fsSource.Seek(2, SeekOrigin.Current);
				// Read the source file into a byte array.
				byte[] animalBytes = new byte[1];
				numBytesToRead = 1;
				numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					// Read may return anything from 0 to numBytesToRead.
					int n = fsSource.Read(animalBytes, numBytesRead, numBytesToRead);

					// Break when the end of the file is reached.
					if (n == 0)
						break;

					numBytesRead += n;
					numBytesToRead -= n;
				}

				animal = (AnimalType)animalBytes[0];

				fsSource.Seek(5, SeekOrigin.Current);
				//fsSource.Seek(118, SeekOrigin.Begin);
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
				
				bool[][] ringBitArray = ringBits.Split(8);
				for (int i = 0; i < ringBitArray.Count(); i++ )
				{
					ringBitArray[i] = ringBitArray[i].Reverse().ToArray();
				}
				ringBits = ringBitArray[0].Concat(ringBitArray[1]).Concat(ringBitArray[2]).Concat(ringBitArray[3])
						.Concat(ringBitArray[4]).Concat(ringBitArray[5]).Concat(ringBitArray[6]).Concat(ringBitArray[7]).ToArray();
				
			}
		}

		IEnumerable<bool> GetBits(byte b)
		{
			for (int i = 0; i < 8; i++)
			{
				yield return (b & 0x80) != 0;
				b *= 2;
			}
		}

		public bool[] ringBits { get; set; }
	}
}
