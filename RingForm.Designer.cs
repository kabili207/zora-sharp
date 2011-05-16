namespace OracleHack
{
	partial class RingForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RingForm));
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.SuspendLayout();
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.FormattingEnabled = true;
			this.checkedListBox1.Items.AddRange(new object[] {
            "Friendship Ring",
            "Power Ring L-1",
            "Power Ring L-2",
            "  4  | Power Ring L-3",
            "  5  | Armor Ring L-1 ",
            "  6  | Armor Ring L-2",
            "  7  | Armor Ring L-3",
            "  8  | Red Ring",
            "  9  | Blue Ring",
            " 10  | Green Ring",
            " 11  | Cursed Ring",
            " 12  | Expert\'s Ring",
            " 13  | Blast Ring",
            " 14  | Rang Ring L-1",
            " 15  | GBA Time Ring            | Life Advanced!",
            " 16  | Maple\'s Ring             | Maple meetings >",
            " 17  | Steadfast Ring           | Get knocked back less",
            " 18  | Pegasus Ring             | Lengthen Pegasus Seed effect",
            " 19  | Toss Ring                | Throwing distance >",
            " 20  | Heart Ring L-1           | Slowly recover lost Hearts",
            " 21  | Heart Ring L-2           | Recover lost Hearts",
            " 22  | Swimmer\'s Ring           | Swimming speed >",
            " 23  | Charge Ring              | Spin Attack charges quickly",
            " 24  | Light Ring L-1           | Sword beams at -2 Hearts",
            " 25  | Light Ring L-2           | Sword beams at -3 Hearts",
            " 26  | Bomber\'s Ring            | Set two Bombs at once",
            " 27  | Green Luck Ring          | 1/2 damage from traps",
            " 28  | Blue Luck Ring           | 1/2 damage from beams",
            " 29  | Gold Luck Ring           | 1/2 damage from falls",
            " 30  | Red Luck Ring            | 1/2 damage from spiked floors",
            " 31  | Green Holy Ring          | No damage from electricity",
            " 32  | Blue Holy Ring           | No damage from Zora\'s fire",
            " 33  | Red Holy Ring            | No damage from small rocks",
            " 34  | Snowshoe Ring            | No sliding on ice",
            " 35  | Roc\'s Ring               | Cracked floors don\'t crumble",
            " 36  | Quicksand Ring           | No sinking in quicksand",
            " 37  | Red Joy Ring             | Beasts drop double Rupees",
            " 38  | Blue Joy Ring            | Beasts drop double Hearts",
            " 39  | Gold Joy Ring            | Find double items",
            " 40  | Green Joy Ring           | Find double Ore Chunks",
            " 41  | Discovery Ring           | Sense soft earth nearby",
            " 42  | Rang Ring L-2            | Boomerang damage >>",
            " 43  | Octo Ring                | Become an Octorok",
            " 44  | Moblin Ring              | Become a Moblin",
            " 45  | Like Like Ring           | Become a Like-Like",
            " 46  | Subrosian Ring           | Become a Subrosian",
            " 47  | First Gen Ring           | Become something",
            " 48  | Spin Ring                | Double Spin Attack",
            " 49  | Bombproof Ring           | No damage from your own Bombs",
            " 50  | Energy Ring              | Beam replaces Spin Attack",
            " 51  | Dbl. Edge Ring           | Sword damage > but you get hurt",
            " 52  | GBA Nature Ring          | Life Advanced!",
            " 53  | Slayer\'s Ring            | 1000 beasts slain",
            " 54  | Rupee Ring               | 10,000 Rupees collected",
            " 55  | Victory Ring             | The Evil King Ganon defeated",
            " 56  | Sign Ring                | 100 signs broken",
            " 57  | 100th Ring               | 100 rings appraised",
            " 58  | Whisp Ring               | No effect from jinxes",
            " 59  | Gasha Ring               | Grow great Gasha Trees",
            " 60  | Peace Ring               | No explosion if holding Bomb",
            " 61  | Zora Ring                | Dive without breathing",
            " 62  | Fist Ring                | Punch when not equipped",
            " 63  | Whimsical Ring           | Sword damage < Sometimes deadly",
            " 64  | Protection Ring          | Damage taken is always one Heart"});
			this.checkedListBox1.Location = new System.Drawing.Point(13, 13);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(259, 199);
			this.checkedListBox1.TabIndex = 0;
			// 
			// RingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.checkedListBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Rings";
			this.Load += new System.EventHandler(this.RingForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox checkedListBox1;
	}
}