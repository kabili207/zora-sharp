namespace OracleHack
{
	partial class SaveEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveEditor));
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.nudGameId = new System.Windows.Forms.NumericUpDown();
			this.txtKid = new System.Windows.Forms.TextBox();
			this.txtHero = new System.Windows.Forms.TextBox();
			this.cmbAnimal = new System.Windows.Forms.ComboBox();
			this.cmbBehavior = new System.Windows.Forms.ComboBox();
			this.picAnimal = new System.Windows.Forms.PictureBox();
			this.rdoAges = new System.Windows.Forms.RadioButton();
			this.rdoSeasons = new System.Windows.Forms.RadioButton();
			this.rdoHeroGame = new System.Windows.Forms.RadioButton();
			this.rdoLinkedGame = new System.Windows.Forms.RadioButton();
			this.picNayru = new System.Windows.Forms.PictureBox();
			this.picDin = new System.Windows.Forms.PictureBox();
			this.grpGame = new System.Windows.Forms.GroupBox();
			this.grpQuest = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.btnRings = new System.Windows.Forms.Button();
			this.btnEncode = new System.Windows.Forms.Button();
			this.btnDecode = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudGameId)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picAnimal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picNayru)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picDin)).BeginInit();
			this.grpGame.SuspendLayout();
			this.grpQuest.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(39, 12);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(163, 20);
			this.txtFileName.TabIndex = 0;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(208, 10);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(56, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(23, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "File";
			// 
			// nudGameId
			// 
			this.nudGameId.Location = new System.Drawing.Point(63, 114);
			this.nudGameId.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.nudGameId.Name = "nudGameId";
			this.nudGameId.Size = new System.Drawing.Size(66, 20);
			this.nudGameId.TabIndex = 164;
			// 
			// txtKid
			// 
			this.txtKid.Location = new System.Drawing.Point(190, 114);
			this.txtKid.MaxLength = 5;
			this.txtKid.Name = "txtKid";
			this.txtKid.Size = new System.Drawing.Size(66, 20);
			this.txtKid.TabIndex = 163;
			// 
			// txtHero
			// 
			this.txtHero.Location = new System.Drawing.Point(63, 140);
			this.txtHero.MaxLength = 5;
			this.txtHero.Name = "txtHero";
			this.txtHero.Size = new System.Drawing.Size(66, 20);
			this.txtHero.TabIndex = 162;
			// 
			// cmbAnimal
			// 
			this.cmbAnimal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAnimal.FormattingEnabled = true;
			this.cmbAnimal.Location = new System.Drawing.Point(63, 166);
			this.cmbAnimal.Name = "cmbAnimal";
			this.cmbAnimal.Size = new System.Drawing.Size(66, 21);
			this.cmbAnimal.TabIndex = 165;
			this.cmbAnimal.SelectedValueChanged += new System.EventHandler(this.cmbAnimal_SelectedValueChanged);
			// 
			// cmbBehavior
			// 
			this.cmbBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBehavior.FormattingEnabled = true;
			this.cmbBehavior.Location = new System.Drawing.Point(190, 140);
			this.cmbBehavior.Name = "cmbBehavior";
			this.cmbBehavior.Size = new System.Drawing.Size(66, 21);
			this.cmbBehavior.TabIndex = 166;
			// 
			// picAnimal
			// 
			this.picAnimal.Location = new System.Drawing.Point(134, 162);
			this.picAnimal.Name = "picAnimal";
			this.picAnimal.Size = new System.Drawing.Size(32, 28);
			this.picAnimal.TabIndex = 167;
			this.picAnimal.TabStop = false;
			// 
			// rdoAges
			// 
			this.rdoAges.AutoSize = true;
			this.rdoAges.Location = new System.Drawing.Point(32, 22);
			this.rdoAges.Name = "rdoAges";
			this.rdoAges.Size = new System.Drawing.Size(49, 17);
			this.rdoAges.TabIndex = 168;
			this.rdoAges.TabStop = true;
			this.rdoAges.Text = "Ages";
			this.rdoAges.UseVisualStyleBackColor = true;
			// 
			// rdoSeasons
			// 
			this.rdoSeasons.AutoSize = true;
			this.rdoSeasons.Location = new System.Drawing.Point(32, 46);
			this.rdoSeasons.Name = "rdoSeasons";
			this.rdoSeasons.Size = new System.Drawing.Size(66, 17);
			this.rdoSeasons.TabIndex = 169;
			this.rdoSeasons.TabStop = true;
			this.rdoSeasons.Text = "Seasons";
			this.rdoSeasons.UseVisualStyleBackColor = true;
			// 
			// rdoHeroGame
			// 
			this.rdoHeroGame.AutoSize = true;
			this.rdoHeroGame.Location = new System.Drawing.Point(6, 46);
			this.rdoHeroGame.Name = "rdoHeroGame";
			this.rdoHeroGame.Size = new System.Drawing.Size(86, 17);
			this.rdoHeroGame.TabIndex = 171;
			this.rdoHeroGame.TabStop = true;
			this.rdoHeroGame.Text = "Hero\'s Quest";
			this.rdoHeroGame.UseVisualStyleBackColor = true;
			// 
			// rdoLinkedGame
			// 
			this.rdoLinkedGame.AutoSize = true;
			this.rdoLinkedGame.Location = new System.Drawing.Point(6, 23);
			this.rdoLinkedGame.Name = "rdoLinkedGame";
			this.rdoLinkedGame.Size = new System.Drawing.Size(88, 17);
			this.rdoLinkedGame.TabIndex = 170;
			this.rdoLinkedGame.TabStop = true;
			this.rdoLinkedGame.Text = "Linked Quest";
			this.rdoLinkedGame.UseVisualStyleBackColor = true;
			// 
			// picNayru
			// 
			this.picNayru.Image = global::OracleHack.Properties.Resources.Nayru;
			this.picNayru.Location = new System.Drawing.Point(6, 19);
			this.picNayru.Name = "picNayru";
			this.picNayru.Size = new System.Drawing.Size(20, 20);
			this.picNayru.TabIndex = 172;
			this.picNayru.TabStop = false;
			// 
			// picDin
			// 
			this.picDin.Image = global::OracleHack.Properties.Resources.Din;
			this.picDin.Location = new System.Drawing.Point(6, 43);
			this.picDin.Name = "picDin";
			this.picDin.Size = new System.Drawing.Size(20, 20);
			this.picDin.TabIndex = 173;
			this.picDin.TabStop = false;
			// 
			// grpGame
			// 
			this.grpGame.Controls.Add(this.picNayru);
			this.grpGame.Controls.Add(this.picDin);
			this.grpGame.Controls.Add(this.rdoAges);
			this.grpGame.Controls.Add(this.rdoSeasons);
			this.grpGame.Location = new System.Drawing.Point(31, 38);
			this.grpGame.Name = "grpGame";
			this.grpGame.Size = new System.Drawing.Size(107, 70);
			this.grpGame.TabIndex = 174;
			this.grpGame.TabStop = false;
			this.grpGame.Text = "Game";
			// 
			// grpQuest
			// 
			this.grpQuest.Controls.Add(this.rdoHeroGame);
			this.grpQuest.Controls.Add(this.rdoLinkedGame);
			this.grpQuest.Location = new System.Drawing.Point(149, 38);
			this.grpQuest.Name = "grpQuest";
			this.grpQuest.Size = new System.Drawing.Size(100, 70);
			this.grpQuest.TabIndex = 175;
			this.grpQuest.TabStop = false;
			this.grpQuest.Text = "Quest Type";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(19, 143);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(30, 13);
			this.label2.TabIndex = 176;
			this.label2.Text = "Hero";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(19, 169);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 177;
			this.label3.Text = "Animal";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(19, 116);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(18, 13);
			this.label4.TabIndex = 178;
			this.label4.Text = "ID";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(135, 143);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(49, 13);
			this.label5.TabIndex = 179;
			this.label5.Text = "Behavior";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(135, 117);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(30, 13);
			this.label6.TabIndex = 180;
			this.label6.Text = "Child";
			// 
			// btnRings
			// 
			this.btnRings.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnRings.Location = new System.Drawing.Point(112, 203);
			this.btnRings.Name = "btnRings";
			this.btnRings.Size = new System.Drawing.Size(51, 23);
			this.btnRings.TabIndex = 181;
			this.btnRings.Text = "Rings";
			this.btnRings.UseVisualStyleBackColor = true;
			this.btnRings.Click += new System.EventHandler(this.btnRings_Click);
			// 
			// btnEncode
			// 
			this.btnEncode.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnEncode.Location = new System.Drawing.Point(12, 203);
			this.btnEncode.Name = "btnEncode";
			this.btnEncode.Size = new System.Drawing.Size(94, 23);
			this.btnEncode.TabIndex = 182;
			this.btnEncode.Text = "Make Secrets";
			this.btnEncode.UseVisualStyleBackColor = true;
			// 
			// btnDecode
			// 
			this.btnDecode.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnDecode.Location = new System.Drawing.Point(169, 203);
			this.btnDecode.Name = "btnDecode";
			this.btnDecode.Size = new System.Drawing.Size(94, 23);
			this.btnDecode.TabIndex = 183;
			this.btnDecode.Text = "Decode Secrets";
			this.btnDecode.UseVisualStyleBackColor = true;
			this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
			// 
			// SaveEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(275, 238);
			this.Controls.Add(this.btnDecode);
			this.Controls.Add(this.btnEncode);
			this.Controls.Add(this.btnRings);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.grpQuest);
			this.Controls.Add(this.grpGame);
			this.Controls.Add(this.picAnimal);
			this.Controls.Add(this.cmbBehavior);
			this.Controls.Add(this.cmbAnimal);
			this.Controls.Add(this.nudGameId);
			this.Controls.Add(this.txtKid);
			this.Controls.Add(this.txtHero);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtFileName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SaveEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Zelda Oracle of Secrets";
			((System.ComponentModel.ISupportInitialize)(this.nudGameId)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picAnimal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picNayru)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picDin)).EndInit();
			this.grpGame.ResumeLayout(false);
			this.grpGame.PerformLayout();
			this.grpQuest.ResumeLayout(false);
			this.grpQuest.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudGameId;
		private System.Windows.Forms.TextBox txtKid;
		private System.Windows.Forms.TextBox txtHero;
		private System.Windows.Forms.ComboBox cmbAnimal;
		private System.Windows.Forms.ComboBox cmbBehavior;
		private System.Windows.Forms.PictureBox picAnimal;
		private System.Windows.Forms.RadioButton rdoAges;
		private System.Windows.Forms.RadioButton rdoSeasons;
		private System.Windows.Forms.RadioButton rdoHeroGame;
		private System.Windows.Forms.RadioButton rdoLinkedGame;
		private System.Windows.Forms.PictureBox picNayru;
		private System.Windows.Forms.PictureBox picDin;
		private System.Windows.Forms.GroupBox grpGame;
		private System.Windows.Forms.GroupBox grpQuest;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnRings;
		private System.Windows.Forms.Button btnEncode;
		private System.Windows.Forms.Button btnDecode;
	}
}