using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace ImageRenamer
{
    /// <summary>
    /// Description résumée de FrmInputDialog.
    /// </summary>
    public class FrmInputDialog : System.Windows.Forms.Form
    {
        public enum InputScopeEnum
        {
            CustomFormat,
            Filename,
            Date,
        }
        public InputScopeEnum InputScope = InputScopeEnum.CustomFormat;
        public string CustomFormat = "";
        public string Value = "";
        public string OriginalValue = "";
        private TextBox txtPrevValue;
        private TextBox txtValue;
        private TextBox txtNextValue;
        private Button btnOk;
        private Button btnCancel;
        private Button btnCancelAll;
        private Label lblError;
        private PictureBox btnAverage;
        private PictureBox btnReset;
        private ToolTip toolTip;
        private IContainer components;

        public FrmInputDialog()
        {
            InitializeComponent();
        }
        public FrmInputDialog(bool inBatch, string prevValue, string value, string originalValue, string nextValue, 
            InputScopeEnum inputScope = InputScopeEnum.CustomFormat,
            string customFormat = null)
        {
            InitializeComponent();
            txtPrevValue.Text = prevValue;
            txtPrevValue.Visible = prevValue != null;

            txtValue.Text = value;
            Value = value;
            OriginalValue = originalValue;

            txtNextValue.Text = nextValue;
            txtNextValue.Visible = nextValue != null;

             btnCancelAll.Visible = inBatch;
            InputScope = inputScope;
            CustomFormat = customFormat;
        }

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form
        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtPrevValue = new System.Windows.Forms.TextBox();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtNextValue = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCancelAll = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.PictureBox();
            this.btnAverage = new System.Windows.Forms.PictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.btnReset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAverage)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPrevValue
            // 
            this.txtPrevValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrevValue.Location = new System.Drawing.Point(6, 8);
            this.txtPrevValue.Name = "txtPrevValue";
            this.txtPrevValue.ReadOnly = true;
            this.txtPrevValue.Size = new System.Drawing.Size(292, 20);
            this.txtPrevValue.TabIndex = 1;
            // 
            // txtValue
            // 
            this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValue.Location = new System.Drawing.Point(6, 32);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(220, 20);
            this.txtValue.TabIndex = 1;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(27, 79);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(74, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "&Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtNextValue
            // 
            this.txtNextValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNextValue.Location = new System.Drawing.Point(6, 56);
            this.txtNextValue.Name = "txtNextValue";
            this.txtNextValue.ReadOnly = true;
            this.txtNextValue.Size = new System.Drawing.Size(292, 20);
            this.txtNextValue.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(115, 79);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCancelAll
            // 
            this.btnCancelAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancelAll.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnCancelAll.Location = new System.Drawing.Point(203, 79);
            this.btnCancelAll.Name = "btnCancelAll";
            this.btnCancelAll.Size = new System.Drawing.Size(74, 23);
            this.btnCancelAll.TabIndex = 3;
            this.btnCancelAll.Text = "Cancel &All";
            this.btnCancelAll.Click += new System.EventHandler(this.btnCancelAll_Click);
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(228, 32);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(17, 24);
            this.lblError.TabIndex = 4;
            this.lblError.Text = "*";
            this.lblError.Visible = false;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnReset.Image = global::ImageSorter.Properties.Resources.Back;
            this.btnReset.Location = new System.Drawing.Point(249, 30);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(20, 22);
            this.btnReset.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnReset.TabIndex = 5;
            this.btnReset.TabStop = false;
            this.toolTip.SetToolTip(this.btnReset, "Reset to original value");
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnAverage
            // 
            this.btnAverage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAverage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnAverage.Image = global::ImageSorter.Properties.Resources.AutoFix;
            this.btnAverage.Location = new System.Drawing.Point(277, 30);
            this.btnAverage.Name = "btnAverage";
            this.btnAverage.Size = new System.Drawing.Size(20, 22);
            this.btnAverage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnAverage.TabIndex = 6;
            this.btnAverage.TabStop = false;
            this.toolTip.SetToolTip(this.btnAverage, "AutoFix from previous and next dates");
            this.btnAverage.Click += new System.EventHandler(this.btnAverage_Click);
            // 
            // FrmInputDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(304, 111);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnAverage);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtNextValue);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.txtPrevValue);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCancelAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(320, 150);
            this.Name = "FrmInputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Name of the file";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmInputDialog_FormClosing);
            this.Load += new System.EventHandler(this.FrmInputDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnReset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAverage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void FrmInputDialog_Load(object sender, System.EventArgs e)
        {
            this.txtValue.Focus();
            this.txtValue.SelectAll();
        }
        
        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (IsValid())
                this.Value = txtValue.Text;
            
            //this.DialogResult = btnOk.DialogResult;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            //this.DialogResult = btnCancel.DialogResult;

        }

        private void btnCancelAll_Click(object sender, System.EventArgs e)
        {
            //this.DialogResult = btnCancelAll.DialogResult;
        }

        private void FrmInputDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DialogResult == DialogResult.OK)
                e.Cancel = !IsValid();
        }

        private bool IsValid()
        {
            switch (InputScope)
            {
                case InputScopeEnum.CustomFormat:
                    return true;
                case InputScopeEnum.Filename:
                    return !string.IsNullOrEmpty(txtValue.Text) && this.Value.IndexOfAny(Path.GetInvalidFileNameChars()) < 0; 
                case InputScopeEnum.Date:
                    return DateTime.TryParse(txtValue.Text, out _);
                default:
                    return true;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtValue.Text = OriginalValue;
        }

        private void btnAverage_Click(object sender, EventArgs e)
        {
            switch (InputScope)
            {
                case InputScopeEnum.CustomFormat:
                    break;
                case InputScopeEnum.Filename:
                    break;
                case InputScopeEnum.Date:
                    var prevDate = DateTime.Parse(txtPrevValue.Text);
                    var nextDate = DateTime.Parse(txtNextValue.Text);
                    txtValue.Text = prevDate.AddSeconds((nextDate - prevDate).TotalSeconds/2d).ToString();
                    break;
                default:
                    break;
            }
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            bool isValid = IsValid();
            lblError.Visible = !isValid;
        }
    }
}
