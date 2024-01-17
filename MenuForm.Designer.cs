namespace AudioMonitor
{
    partial class MenuForm
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
            this.btnLaunchFFT = new System.Windows.Forms.Button();
            this.lbDevice = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLaunchFFT
            // 
            this.btnLaunchFFT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(212)))), ((int)(((byte)(92)))));
            this.btnLaunchFFT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLaunchFFT.Font = new System.Drawing.Font("Open Sans Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLaunchFFT.Location = new System.Drawing.Point(395, 325);
            this.btnLaunchFFT.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLaunchFFT.Name = "btnLaunchFFT";
            this.btnLaunchFFT.Size = new System.Drawing.Size(194, 46);
            this.btnLaunchFFT.TabIndex = 1;
            this.btnLaunchFFT.Text = "Start";
            this.btnLaunchFFT.UseVisualStyleBackColor = false;
            this.btnLaunchFFT.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbDevice
            // 
            this.lbDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.lbDevice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDevice.Font = new System.Drawing.Font("Open Sans Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbDevice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.lbDevice.FormattingEnabled = true;
            this.lbDevice.ItemHeight = 24;
            this.lbDevice.Location = new System.Drawing.Point(4, 37);
            this.lbDevice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbDevice.Name = "lbDevice";
            this.lbDevice.Size = new System.Drawing.Size(563, 254);
            this.lbDevice.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.groupBox1.Controls.Add(this.lbDevice);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(18, 19);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(571, 296);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device";
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(606, 386);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLaunchFFT);
            this.Font = new System.Drawing.Font("Open Sans", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "MenuForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Vis";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Button btnLaunchFFT;
        private ListBox lbDevice;
        private GroupBox groupBox1;
    }
}