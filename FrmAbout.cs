using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageSorter
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.lblVersion.Text += " " + version.Major + "." + version.Minor;
        }

        private void lblWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/VincentGuigui/ImageSorter#readme");
        }
    }
}
