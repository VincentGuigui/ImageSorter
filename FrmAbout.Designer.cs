
namespace ImageSorter
{
    partial class FrmAbout
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
            this.lblAppname = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblWebsite = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAppname
            // 
            this.lblAppname.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAppname.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppname.Location = new System.Drawing.Point(0, 0);
            this.lblAppname.Name = "lblAppname";
            this.lblAppname.Size = new System.Drawing.Size(784, 40);
            this.lblAppname.TabIndex = 0;
            this.lblAppname.Text = "ImageSorter";
            this.lblAppname.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            this.lblVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblVersion.Location = new System.Drawing.Point(0, 40);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(784, 30);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "version ";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWebsite
            // 
            this.lblWebsite.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWebsite.Location = new System.Drawing.Point(0, 70);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(784, 30);
            this.lblWebsite.TabIndex = 2;
            this.lblWebsite.TabStop = true;
            this.lblWebsite.Text = "Website (github)";
            this.lblWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblWebsite_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::ImageSorter.Properties.Resources.tooltips;
            this.pictureBox1.Location = new System.Drawing.Point(0, 100);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(784, 517);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 617);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblWebsite);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblAppname);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAppname;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.LinkLabel lblWebsite;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}