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
        private Label lblError;
        private PictureBox btnAverage;
        private PictureBox btnReset;
        private ToolTip toolTip;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnCancelAll;
        private Button btnOk;
        private Button btnCancel;
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
            if (!inBatch)
                this.Height -= btnCancelAll.Height;
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
            this.txtNextValue = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.PictureBox();
            this.btnAverage = new System.Windows.Forms.PictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancelAll = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.btnReset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAverage)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPrevValue
            // 
            this.txtPrevValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrevValue.Location = new System.Drawing.Point(73, 8);
            this.txtPrevValue.Name = "txtPrevValue";
            this.txtPrevValue.ReadOnly = true;
            this.txtPrevValue.Size = new System.Drawing.Size(240, 20);
            this.txtPrevValue.TabIndex = 1;
            // 
            // txtValue
            // 
            this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValue.Location = new System.Drawing.Point(73, 32);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(240, 20);
            this.txtValue.TabIndex = 1;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            this.txtValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValue_KeyDown);
            // 
            // txtNextValue
            // 
            this.txtNextValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNextValue.Location = new System.Drawing.Point(73, 56);
            this.txtNextValue.Name = "txtNextValue";
            this.txtNextValue.ReadOnly = true;
            this.txtNextValue.Size = new System.Drawing.Size(240, 20);
            this.txtNextValue.TabIndex = 2;
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(310, 32);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(12, 24);
            this.lblError.TabIndex = 4;
            this.lblError.Text = "*";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblError.Visible = false;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnReset.Image = global::ImageSorter.Properties.Resources.Back;
            this.btnReset.Location = new System.Drawing.Point(326, 32);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(24, 24);
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
            this.btnAverage.Location = new System.Drawing.Point(356, 32);
            this.btnAverage.Name = "btnAverage";
            this.btnAverage.Size = new System.Drawing.Size(24, 24);
            this.btnAverage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnAverage.TabIndex = 6;
            this.btnAverage.TabStop = false;
            this.toolTip.SetToolTip(this.btnAverage, "AutoFix from previous and next dates");
            this.btnAverage.Click += new System.EventHandler(this.btnAverage_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Previous";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Current";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Next";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnCancelAll
            // 
            this.btnCancelAll.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancelAll.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnCancelAll.Location = new System.Drawing.Point(155, 115);
            this.btnCancelAll.Name = "btnCancelAll";
            this.btnCancelAll.Size = new System.Drawing.Size(74, 23);
            this.btnCancelAll.TabIndex = 12;
            this.btnCancelAll.Text = "Cancel &All";
            this.btnCancelAll.Click += new System.EventHandler(this.btnCancelAll_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(112, 86);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(74, 23);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "&Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(199, 86);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmInputDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(384, 141);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnAverage);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.btnCancelAll);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.txtNextValue);
            this.Controls.Add(this.txtPrevValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 180);
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
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {

        }

        private void btnCancelAll_Click(object sender, System.EventArgs e)
        {
        }

        private void FrmInputDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
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
                    txtValue.Text = prevDate.AddSeconds((nextDate - prevDate).TotalSeconds / 2d).ToString();
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

        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Cancel:
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    break;
                case Keys.Return:
                    btnOk_Click(sender, null);
                    this.DialogResult = DialogResult.OK;
                    break;
                default:
                    break;
            }
        }
    }
}
