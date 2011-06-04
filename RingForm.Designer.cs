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
			this.lstRings = new System.Windows.Forms.CheckedListBox();
			this.SuspendLayout();
			// 
			// lstRings
			// 
			this.lstRings.FormattingEnabled = true;
			this.lstRings.Items.AddRange(new object[] {
            "Friendship Ring",
            "Power Ring L-1",
            "Power Ring L-2",
            "Power Ring L-3",
            "Armor Ring L-1 ",
            "Armor Ring L-2",
            "Armor Ring L-3",
            "Red Ring",
            "Blue Ring",
            "Green Ring",
            "Cursed Ring",
            "Expert\'s Ring",
            "Blast Ring",
            "Rang Ring L-1",
            "GBA Time Ring            | Life Advanced!",
            "Maple\'s Ring             | Maple meetings >",
            "Steadfast Ring           | Get knocked back less",
            "Pegasus Ring             | Lengthen Pegasus Seed effect",
            "Toss Ring                | Throwing distance >",
            "Heart Ring L-1           | Slowly recover lost Hearts",
            "Heart Ring L-2           | Recover lost Hearts",
            "Swimmer\'s Ring           | Swimming speed >",
            "Charge Ring              | Spin Attack charges quickly",
            "Light Ring L-1           | Sword beams at -2 Hearts",
            "Light Ring L-2           | Sword beams at -3 Hearts",
            "Bomber\'s Ring            | Set two Bombs at once",
            "Green Luck Ring          | 1/2 damage from traps",
            "Blue Luck Ring           | 1/2 damage from beams",
            "Gold Luck Ring           | 1/2 damage from falls",
            "Red Luck Ring            | 1/2 damage from spiked floors",
            "Green Holy Ring          | No damage from electricity",
            "Blue Holy Ring           | No damage from Zora\'s fire",
            "Red Holy Ring            | No damage from small rocks",
            "Snowshoe Ring            | No sliding on ice",
            "Roc\'s Ring               | Cracked floors don\'t crumble",
            "Quicksand Ring           | No sinking in quicksand",
            "Red Joy Ring             | Beasts drop double Rupees",
            "Blue Joy Ring            | Beasts drop double Hearts",
            "Gold Joy Ring            | Find double items",
            "Green Joy Ring           | Find double Ore Chunks",
            "Discovery Ring           | Sense soft earth nearby",
            "Rang Ring L-2            | Boomerang damage >>",
            "Octo Ring                | Become an Octorok",
            "Moblin Ring              | Become a Moblin",
            "Like Like Ring           | Become a Like-Like",
            "Subrosian Ring           | Become a Subrosian",
            "First Gen Ring           | Become something",
            " Spin Ring                | Double Spin Attack",
            "Bombproof Ring           | No damage from your own Bombs",
            "Energy Ring              | Beam replaces Spin Attack",
            "Dbl. Edge Ring           | Sword damage > but you get hurt",
            "GBA Nature Ring          | Life Advanced!",
            "Slayer\'s Ring            | 1000 beasts slain",
            "Rupee Ring               | 10,000 Rupees collected",
            "Victory Ring             | The Evil King Ganon defeated",
            "Sign Ring                | 100 signs broken",
            "100th Ring               | 100 rings appraised",
            "Whisp Ring               | No effect from jinxes",
            "Gasha Ring               | Grow great Gasha Trees",
            "Peace Ring               | No explosion if holding Bomb",
            "Zora Ring                | Dive without breathing",
            "Fist Ring                | Punch when not equipped",
            "Whimsical Ring           | Sword damage < Sometimes deadly",
            "Protection Ring          | Damage taken is always one Heart"});
			this.lstRings.Location = new System.Drawing.Point(13, 13);
			this.lstRings.Name = "lstRings";
			this.lstRings.Size = new System.Drawing.Size(259, 199);
			this.lstRings.TabIndex = 0;
			this.lstRings.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstRings_ItemCheck);
			// 
			// RingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.lstRings);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Rings";
			this.Load += new System.EventHandler(this.RingForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox lstRings;
	}
}