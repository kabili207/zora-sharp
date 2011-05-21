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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.nudGameId = new System.Windows.Forms.NumericUpDown();
			this.txtKid = new System.Windows.Forms.TextBox();
			this.txtHero = new System.Windows.Forms.TextBox();
			this.cmbAnimal = new System.Windows.Forms.ComboBox();
			this.cmbBehavior = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.nudGameId)).BeginInit();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(41, 12);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(210, 20);
			this.textBox1.TabIndex = 0;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(257, 10);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(23, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "File";
			// 
			// nudGameId
			// 
			this.nudGameId.Location = new System.Drawing.Point(24, 116);
			this.nudGameId.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.nudGameId.Name = "nudGameId";
			this.nudGameId.Size = new System.Drawing.Size(97, 20);
			this.nudGameId.TabIndex = 164;
			// 
			// txtKid
			// 
			this.txtKid.Location = new System.Drawing.Point(132, 63);
			this.txtKid.Name = "txtKid";
			this.txtKid.Size = new System.Drawing.Size(97, 20);
			this.txtKid.TabIndex = 163;
			// 
			// txtHero
			// 
			this.txtHero.Location = new System.Drawing.Point(24, 63);
			this.txtHero.Name = "txtHero";
			this.txtHero.Size = new System.Drawing.Size(97, 20);
			this.txtHero.TabIndex = 162;
			// 
			// cmbAnimal
			// 
			this.cmbAnimal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAnimal.FormattingEnabled = true;
			this.cmbAnimal.Location = new System.Drawing.Point(24, 89);
			this.cmbAnimal.Name = "cmbAnimal";
			this.cmbAnimal.Size = new System.Drawing.Size(97, 21);
			this.cmbAnimal.TabIndex = 165;
			// 
			// cmbBehavior
			// 
			this.cmbBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBehavior.FormattingEnabled = true;
			this.cmbBehavior.Location = new System.Drawing.Point(132, 89);
			this.cmbBehavior.Name = "cmbBehavior";
			this.cmbBehavior.Size = new System.Drawing.Size(97, 21);
			this.cmbBehavior.TabIndex = 166;
			// 
			// SaveEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(344, 263);
			this.Controls.Add(this.cmbBehavior);
			this.Controls.Add(this.cmbAnimal);
			this.Controls.Add(this.nudGameId);
			this.Controls.Add(this.txtKid);
			this.Controls.Add(this.txtHero);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.textBox1);
			this.Name = "SaveEditor";
			this.Text = "SaveEditor";
			((System.ComponentModel.ISupportInitialize)(this.nudGameId)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudGameId;
		private System.Windows.Forms.TextBox txtKid;
		private System.Windows.Forms.TextBox txtHero;
		private System.Windows.Forms.ComboBox cmbAnimal;
		private System.Windows.Forms.ComboBox cmbBehavior;
	}
}