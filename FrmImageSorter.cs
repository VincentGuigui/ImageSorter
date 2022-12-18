using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Drawing;

namespace ImageRenamer
{
    /// <summary>
    /// Description r�sum�e de Form1.
    /// </summary>
    public class FrmImageRenamer : System.Windows.Forms.Form
    {
        private enum ExtensionTransformation
        {
            NoChange = 0,
            Replace = 1,
            ToUpper = 2,
            ToLower = 3,
        }

        private const string DATE_FILENAMEFORMAT = "yyyyMMdd_HHmmss";
        private MyListView listView;
        private TextBox txtFolder;
        private FolderBrowserDialog folderBrowserDialog;
        private Button btnBrowse;
        private TrackBar tbThumbSize;
        private Button btnThumbSizeOk;
        private Button btnApplyChanges;
        private CheckBox bInsert;
        private NumericUpDown iInsert;
        private CheckBox bClearFilename;
        private CheckBox bFilename;
        private GroupBox gbBatchRename;
        private NumericUpDown iRemoveLength;
        private CheckBox bRemove;
        private NumericUpDown iRemovePosition;
        private GroupBox gbRemove;
        private Label label3;
        private TextBox txtNewFilenamePattern;
        private ProgressBar progressBar;
        private CheckBox bLoadMetaData;
        private CheckBox bLoadThumbnail;
        private GroupBox gbThumbnail;
        private ComboBox cbChangeExtension;
        private TextBox txtNewExtension;
        private Label label5;
        private TextBox txtDateFormatForFilename;
        private Label label6;
        private Button btnReset;
        private Button btnLockGoodFilenames;
        private Button btnPreviewOnSelection;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label7;
        private Label label8;
        private Button btnAutoFixExif;
        private Button btnSetExifFromDate;
        private Button btnSelectMissingExif;
        private Button btnSetDateFromFilename;
        private Button btnAutoFixDate;
        private Button btnSetDateFromExif;
        private Button btnExifFromName;
        private Button btnSelectUnmatchingDateFromName;
        private CheckBox bCounter;
        private GroupBox gbCounter;
        private NumericUpDown iStart;
        private Label label2;
        private NumericUpDown iStep;
        private Label label1;
        private NumericUpDown iDigits;
        private Label label4;
        private CheckBox bRelativeCounter;
        private CheckBox bDateCounter;
        private GroupBox gbDateCounter;
        private TextBox txtDateCounterStartDate;
        private Label lblDateOffset;
        private TextBox txtDateOffset;
        private Button btnAddToExif;
        private Button btnAddToDate;
        private Label label12;
        private Label label9;
        private TextBox txtDateSetterStep;
        private TextBox txtDateSetterStartDate;
        private Label label11;
        private Button btnSetExif;
        private Button btnSetDate;
        private Label label10;
        private TextBox txtDateCounterIncrement;
        private IContainer components;

        public FrmImageRenamer()
        {
            //
            // Requis pour la prise en charge du Concepteur Windows Forms
            //
            InitializeComponent();
            listView.ProgressBar = this.progressBar;
            cbChangeExtension.Items.Clear();
            cbChangeExtension.DataSource = System.Enum.GetValues(typeof(ExtensionTransformation));
            btnThumbSizeOk_Click(null, null);
        }

        /// <summary>
        /// Nettoyage des ressources utilis�es.
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

        #region Code g�n�r� par le Concepteur Windows Form
        /// <summary>
        /// M�thode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette m�thode avec l'�diteur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImageRenamer));
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbThumbSize = new System.Windows.Forms.TrackBar();
            this.btnThumbSizeOk = new System.Windows.Forms.Button();
            this.btnApplyChanges = new System.Windows.Forms.Button();
            this.bClearFilename = new System.Windows.Forms.CheckBox();
            this.bInsert = new System.Windows.Forms.CheckBox();
            this.iInsert = new System.Windows.Forms.NumericUpDown();
            this.txtNewFilenamePattern = new System.Windows.Forms.TextBox();
            this.bFilename = new System.Windows.Forms.CheckBox();
            this.gbBatchRename = new System.Windows.Forms.GroupBox();
            this.bDateCounter = new System.Windows.Forms.CheckBox();
            this.gbDateCounter = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDateCounterIncrement = new System.Windows.Forms.TextBox();
            this.txtDateCounterStartDate = new System.Windows.Forms.TextBox();
            this.bCounter = new System.Windows.Forms.CheckBox();
            this.gbCounter = new System.Windows.Forms.GroupBox();
            this.bRelativeCounter = new System.Windows.Forms.CheckBox();
            this.iStart = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.iStep = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.iDigits = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPreviewOnSelection = new System.Windows.Forms.Button();
            this.txtDateFormatForFilename = new System.Windows.Forms.TextBox();
            this.txtNewExtension = new System.Windows.Forms.TextBox();
            this.cbChangeExtension = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.bRemove = new System.Windows.Forms.CheckBox();
            this.gbRemove = new System.Windows.Forms.GroupBox();
            this.iRemoveLength = new System.Windows.Forms.NumericUpDown();
            this.iRemovePosition = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.bLoadMetaData = new System.Windows.Forms.CheckBox();
            this.bLoadThumbnail = new System.Windows.Forms.CheckBox();
            this.gbThumbnail = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnLockGoodFilenames = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSelectUnmatchingDateFromName = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDateSetterStep = new System.Windows.Forms.TextBox();
            this.txtDateSetterStartDate = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lblDateOffset = new System.Windows.Forms.Label();
            this.txtDateOffset = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSetExif = new System.Windows.Forms.Button();
            this.btnSetDate = new System.Windows.Forms.Button();
            this.btnAddToExif = new System.Windows.Forms.Button();
            this.btnAddToDate = new System.Windows.Forms.Button();
            this.btnAutoFixExif = new System.Windows.Forms.Button();
            this.btnSetExifFromDate = new System.Windows.Forms.Button();
            this.btnSelectMissingExif = new System.Windows.Forms.Button();
            this.btnSetDateFromFilename = new System.Windows.Forms.Button();
            this.btnAutoFixDate = new System.Windows.Forms.Button();
            this.btnSetDateFromExif = new System.Windows.Forms.Button();
            this.btnExifFromName = new System.Windows.Forms.Button();
            this.listView = new ImageRenamer.MyListView();
            ((System.ComponentModel.ISupportInitialize)(this.tbThumbSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iInsert)).BeginInit();
            this.gbBatchRename.SuspendLayout();
            this.gbDateCounter.SuspendLayout();
            this.gbCounter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iDigits)).BeginInit();
            this.gbRemove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iRemoveLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iRemovePosition)).BeginInit();
            this.gbThumbnail.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFolder
            // 
            this.txtFolder.AllowDrop = true;
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.Location = new System.Drawing.Point(6, 11);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(912, 20);
            this.txtFolder.TabIndex = 1;
            this.txtFolder.TextChanged += new System.EventHandler(this.txtFolder_TextChanged);
            this.txtFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFolder_DragDrop);
            this.txtFolder.DragOver += new System.Windows.Forms.DragEventHandler(this.txtFolder_DragOver);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(928, 8);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(72, 23);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbThumbSize
            // 
            this.tbThumbSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbThumbSize.LargeChange = 3;
            this.tbThumbSize.Location = new System.Drawing.Point(8, 16);
            this.tbThumbSize.Maximum = 20;
            this.tbThumbSize.Name = "tbThumbSize";
            this.tbThumbSize.Size = new System.Drawing.Size(120, 45);
            this.tbThumbSize.TabIndex = 2;
            this.tbThumbSize.Value = 5;
            // 
            // btnThumbSizeOk
            // 
            this.btnThumbSizeOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThumbSizeOk.Location = new System.Drawing.Point(128, 16);
            this.btnThumbSizeOk.Name = "btnThumbSizeOk";
            this.btnThumbSizeOk.Size = new System.Drawing.Size(31, 23);
            this.btnThumbSizeOk.TabIndex = 3;
            this.btnThumbSizeOk.Text = "Ok";
            this.btnThumbSizeOk.Click += new System.EventHandler(this.btnThumbSizeOk_Click);
            // 
            // btnApplyChanges
            // 
            this.btnApplyChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyChanges.Location = new System.Drawing.Point(900, 63);
            this.btnApplyChanges.Name = "btnApplyChanges";
            this.btnApplyChanges.Size = new System.Drawing.Size(100, 45);
            this.btnApplyChanges.TabIndex = 4;
            this.btnApplyChanges.Text = "Apply";
            this.btnApplyChanges.Click += new System.EventHandler(this.btnApplyChanges_Click);
            // 
            // bClearFilename
            // 
            this.bClearFilename.Location = new System.Drawing.Point(8, 22);
            this.bClearFilename.Name = "bClearFilename";
            this.bClearFilename.Size = new System.Drawing.Size(104, 23);
            this.bClearFilename.TabIndex = 0;
            this.bClearFilename.Text = "Clear Filename";
            this.bClearFilename.CheckedChanged += new System.EventHandler(this.bClearFilename_CheckedChanged);
            // 
            // bInsert
            // 
            this.bInsert.Location = new System.Drawing.Point(8, 44);
            this.bInsert.Name = "bInsert";
            this.bInsert.Size = new System.Drawing.Size(112, 24);
            this.bInsert.TabIndex = 1;
            this.bInsert.Text = "Insert at position";
            this.bInsert.CheckedChanged += new System.EventHandler(this.bInsert_CheckedChanged);
            // 
            // iInsert
            // 
            this.iInsert.Location = new System.Drawing.Point(120, 48);
            this.iInsert.Name = "iInsert";
            this.iInsert.Size = new System.Drawing.Size(48, 20);
            this.iInsert.TabIndex = 2;
            this.iInsert.ValueChanged += new System.EventHandler(this.iInsert_ValueChanged);
            // 
            // txtNewFilenamePattern
            // 
            this.txtNewFilenamePattern.Location = new System.Drawing.Point(8, 72);
            this.txtNewFilenamePattern.Name = "txtNewFilenamePattern";
            this.txtNewFilenamePattern.Size = new System.Drawing.Size(188, 20);
            this.txtNewFilenamePattern.TabIndex = 5;
            this.txtNewFilenamePattern.Text = "%COUNTER_";
            this.txtNewFilenamePattern.TextChanged += new System.EventHandler(this.txtNewFilenamePattern_TextChanged);
            // 
            // bFilename
            // 
            this.bFilename.AutoSize = true;
            this.bFilename.Location = new System.Drawing.Point(10, 4);
            this.bFilename.Name = "bFilename";
            this.bFilename.Size = new System.Drawing.Size(85, 17);
            this.bFilename.TabIndex = 0;
            this.bFilename.Text = "File Rename";
            this.bFilename.CheckedChanged += new System.EventHandler(this.bBatchRename_CheckedChanged);
            // 
            // gbBatchRename
            // 
            this.gbBatchRename.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBatchRename.Controls.Add(this.bDateCounter);
            this.gbBatchRename.Controls.Add(this.gbDateCounter);
            this.gbBatchRename.Controls.Add(this.bCounter);
            this.gbBatchRename.Controls.Add(this.gbCounter);
            this.gbBatchRename.Controls.Add(this.btnPreviewOnSelection);
            this.gbBatchRename.Controls.Add(this.txtDateFormatForFilename);
            this.gbBatchRename.Controls.Add(this.txtNewExtension);
            this.gbBatchRename.Controls.Add(this.cbChangeExtension);
            this.gbBatchRename.Controls.Add(this.label6);
            this.gbBatchRename.Controls.Add(this.bClearFilename);
            this.gbBatchRename.Controls.Add(this.txtNewFilenamePattern);
            this.gbBatchRename.Controls.Add(this.bInsert);
            this.gbBatchRename.Controls.Add(this.iInsert);
            this.gbBatchRename.Controls.Add(this.bRemove);
            this.gbBatchRename.Controls.Add(this.gbRemove);
            this.gbBatchRename.Controls.Add(this.label5);
            this.gbBatchRename.Location = new System.Drawing.Point(6, 6);
            this.gbBatchRename.Name = "gbBatchRename";
            this.gbBatchRename.Size = new System.Drawing.Size(206, 477);
            this.gbBatchRename.TabIndex = 11;
            this.gbBatchRename.TabStop = false;
            // 
            // bDateCounter
            // 
            this.bDateCounter.AutoSize = true;
            this.bDateCounter.Location = new System.Drawing.Point(13, 235);
            this.bDateCounter.Name = "bDateCounter";
            this.bDateCounter.Size = new System.Drawing.Size(89, 17);
            this.bDateCounter.TabIndex = 15;
            this.bDateCounter.Text = "Date Counter";
            this.bDateCounter.CheckedChanged += new System.EventHandler(this.bDateCounter_CheckedChanged);
            // 
            // gbDateCounter
            // 
            this.gbDateCounter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDateCounter.BackColor = System.Drawing.Color.Transparent;
            this.gbDateCounter.Controls.Add(this.label10);
            this.gbDateCounter.Controls.Add(this.txtDateCounterIncrement);
            this.gbDateCounter.Controls.Add(this.txtDateCounterStartDate);
            this.gbDateCounter.Location = new System.Drawing.Point(8, 238);
            this.gbDateCounter.Name = "gbDateCounter";
            this.gbDateCounter.Size = new System.Drawing.Size(193, 73);
            this.gbDateCounter.TabIndex = 16;
            this.gbDateCounter.TabStop = false;
            this.gbDateCounter.Text = "Counter";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 34;
            this.label10.Text = "Step";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDateCounterIncrement
            // 
            this.txtDateCounterIncrement.Location = new System.Drawing.Point(38, 42);
            this.txtDateCounterIncrement.Name = "txtDateCounterIncrement";
            this.txtDateCounterIncrement.Size = new System.Drawing.Size(150, 20);
            this.txtDateCounterIncrement.TabIndex = 33;
            this.txtDateCounterIncrement.Text = "d or d.hh:mm[:ss]";
            // 
            // txtDateCounterStartDate
            // 
            this.txtDateCounterStartDate.Location = new System.Drawing.Point(8, 20);
            this.txtDateCounterStartDate.Name = "txtDateCounterStartDate";
            this.txtDateCounterStartDate.Size = new System.Drawing.Size(180, 20);
            this.txtDateCounterStartDate.TabIndex = 17;
            // 
            // bCounter
            // 
            this.bCounter.AutoSize = true;
            this.bCounter.Checked = true;
            this.bCounter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bCounter.Location = new System.Drawing.Point(12, 153);
            this.bCounter.Name = "bCounter";
            this.bCounter.Size = new System.Drawing.Size(63, 17);
            this.bCounter.TabIndex = 15;
            this.bCounter.Text = "Counter";
            this.bCounter.CheckedChanged += new System.EventHandler(this.bCounter_CheckedChanged);
            // 
            // gbCounter
            // 
            this.gbCounter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCounter.BackColor = System.Drawing.Color.Transparent;
            this.gbCounter.Controls.Add(this.bRelativeCounter);
            this.gbCounter.Controls.Add(this.iStart);
            this.gbCounter.Controls.Add(this.label2);
            this.gbCounter.Controls.Add(this.iStep);
            this.gbCounter.Controls.Add(this.label1);
            this.gbCounter.Controls.Add(this.iDigits);
            this.gbCounter.Controls.Add(this.label4);
            this.gbCounter.Location = new System.Drawing.Point(8, 157);
            this.gbCounter.Name = "gbCounter";
            this.gbCounter.Size = new System.Drawing.Size(193, 73);
            this.gbCounter.TabIndex = 14;
            this.gbCounter.TabStop = false;
            this.gbCounter.Text = "Counter";
            // 
            // bRelativeCounter
            // 
            this.bRelativeCounter.AutoSize = true;
            this.bRelativeCounter.Checked = true;
            this.bRelativeCounter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bRelativeCounter.Location = new System.Drawing.Point(100, 42);
            this.bRelativeCounter.Name = "bRelativeCounter";
            this.bRelativeCounter.Size = new System.Drawing.Size(65, 17);
            this.bRelativeCounter.TabIndex = 15;
            this.bRelativeCounter.Text = "Relative";
            // 
            // iStart
            // 
            this.iStart.Location = new System.Drawing.Point(41, 16);
            this.iStart.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.iStart.Name = "iStart";
            this.iStart.Size = new System.Drawing.Size(48, 20);
            this.iStart.TabIndex = 10;
            this.iStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.iStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Start";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iStep
            // 
            this.iStep.Location = new System.Drawing.Point(134, 16);
            this.iStep.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.iStep.Name = "iStep";
            this.iStep.Size = new System.Drawing.Size(49, 20);
            this.iStep.TabIndex = 11;
            this.iStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.iStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Digits";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iDigits
            // 
            this.iDigits.Location = new System.Drawing.Point(41, 39);
            this.iDigits.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.iDigits.Name = "iDigits";
            this.iDigits.Size = new System.Drawing.Size(48, 20);
            this.iDigits.TabIndex = 12;
            this.iDigits.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.iDigits.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(103, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Step";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPreviewOnSelection
            // 
            this.btnPreviewOnSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviewOnSelection.Location = new System.Drawing.Point(6, 421);
            this.btnPreviewOnSelection.Name = "btnPreviewOnSelection";
            this.btnPreviewOnSelection.Size = new System.Drawing.Size(195, 28);
            this.btnPreviewOnSelection.TabIndex = 14;
            this.btnPreviewOnSelection.Text = "Preview on selection";
            this.btnPreviewOnSelection.Click += new System.EventHandler(this.btnPreviewOnSelection_Click);
            // 
            // txtDateFormatForFilename
            // 
            this.txtDateFormatForFilename.Location = new System.Drawing.Point(8, 395);
            this.txtDateFormatForFilename.Name = "txtDateFormatForFilename";
            this.txtDateFormatForFilename.Size = new System.Drawing.Size(193, 20);
            this.txtDateFormatForFilename.TabIndex = 13;
            this.txtDateFormatForFilename.Text = "yyyyMMdd_HHmmss";
            this.txtDateFormatForFilename.TextChanged += new System.EventHandler(this.txtDateFormatForFilename_TextChanged);
            // 
            // txtNewExtension
            // 
            this.txtNewExtension.Location = new System.Drawing.Point(108, 339);
            this.txtNewExtension.Name = "txtNewExtension";
            this.txtNewExtension.Size = new System.Drawing.Size(93, 20);
            this.txtNewExtension.TabIndex = 4;
            this.txtNewExtension.Text = ".";
            this.txtNewExtension.TextChanged += new System.EventHandler(this.txtNewExtension_TextChanged);
            // 
            // cbChangeExtension
            // 
            this.cbChangeExtension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChangeExtension.Items.AddRange(new object[] {
            "0;No Change",
            "1;Replace",
            "2;Upcase",
            "3;Lowcase"});
            this.cbChangeExtension.Location = new System.Drawing.Point(8, 337);
            this.cbChangeExtension.Name = "cbChangeExtension";
            this.cbChangeExtension.Size = new System.Drawing.Size(88, 21);
            this.cbChangeExtension.TabIndex = 3;
            this.cbChangeExtension.SelectedIndexChanged += new System.EventHandler(this.cbChangeExtension_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(7, 366);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(189, 26);
            this.label6.TabIndex = 9;
            this.label6.Text = "Date Format for Filename\r\n%DATE or %EXIF";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bRemove
            // 
            this.bRemove.AutoSize = true;
            this.bRemove.Location = new System.Drawing.Point(13, 98);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new System.Drawing.Size(66, 17);
            this.bRemove.TabIndex = 6;
            this.bRemove.Text = "Remove";
            this.bRemove.CheckedChanged += new System.EventHandler(this.bRemove_CheckedChanged);
            // 
            // gbRemove
            // 
            this.gbRemove.Controls.Add(this.iRemoveLength);
            this.gbRemove.Controls.Add(this.iRemovePosition);
            this.gbRemove.Controls.Add(this.label3);
            this.gbRemove.Location = new System.Drawing.Point(9, 103);
            this.gbRemove.Name = "gbRemove";
            this.gbRemove.Size = new System.Drawing.Size(192, 48);
            this.gbRemove.TabIndex = 11;
            this.gbRemove.TabStop = false;
            // 
            // iRemoveLength
            // 
            this.iRemoveLength.Location = new System.Drawing.Point(7, 19);
            this.iRemoveLength.Name = "iRemoveLength";
            this.iRemoveLength.Size = new System.Drawing.Size(48, 20);
            this.iRemoveLength.TabIndex = 7;
            this.iRemoveLength.ValueChanged += new System.EventHandler(this.iRemoveLength_ValueChanged);
            // 
            // iRemovePosition
            // 
            this.iRemovePosition.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.iRemovePosition.Location = new System.Drawing.Point(107, 19);
            this.iRemovePosition.Name = "iRemovePosition";
            this.iRemovePosition.Size = new System.Drawing.Size(48, 20);
            this.iRemovePosition.TabIndex = 8;
            this.iRemovePosition.ValueChanged += new System.EventHandler(this.iRemovePosition_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.Location = new System.Drawing.Point(50, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "chars at";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 313);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 22);
            this.label5.TabIndex = 8;
            this.label5.Text = "Extension";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(0, 647);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1008, 17);
            this.progressBar.TabIndex = 12;
            // 
            // bLoadMetaData
            // 
            this.bLoadMetaData.Location = new System.Drawing.Point(246, 34);
            this.bLoadMetaData.Name = "bLoadMetaData";
            this.bLoadMetaData.Size = new System.Drawing.Size(102, 24);
            this.bLoadMetaData.TabIndex = 12;
            this.bLoadMetaData.Text = "Load MetaData";
            this.bLoadMetaData.CheckedChanged += new System.EventHandler(this.bLoadMetaData_CheckedChanged);
            // 
            // bLoadThumbnail
            // 
            this.bLoadThumbnail.AutoSize = true;
            this.bLoadThumbnail.Location = new System.Drawing.Point(246, 56);
            this.bLoadThumbnail.Name = "bLoadThumbnail";
            this.bLoadThumbnail.Size = new System.Drawing.Size(75, 17);
            this.bLoadThumbnail.TabIndex = 13;
            this.bLoadThumbnail.Text = "Thumbnail";
            this.bLoadThumbnail.CheckedChanged += new System.EventHandler(this.bLoadThumbnail_CheckedChanged);
            // 
            // gbThumbnail
            // 
            this.gbThumbnail.Controls.Add(this.tbThumbSize);
            this.gbThumbnail.Controls.Add(this.btnThumbSizeOk);
            this.gbThumbnail.Location = new System.Drawing.Point(242, 61);
            this.gbThumbnail.Name = "gbThumbnail";
            this.gbThumbnail.Size = new System.Drawing.Size(168, 47);
            this.gbThumbnail.TabIndex = 14;
            this.gbThumbnail.TabStop = false;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(797, 63);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 45);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnLockGoodFilenames
            // 
            this.btnLockGoodFilenames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLockGoodFilenames.Location = new System.Drawing.Point(6, 493);
            this.btnLockGoodFilenames.Name = "btnLockGoodFilenames";
            this.btnLockGoodFilenames.Size = new System.Drawing.Size(206, 29);
            this.btnLockGoodFilenames.TabIndex = 17;
            this.btnLockGoodFilenames.Text = "Lock good filenames";
            this.btnLockGoodFilenames.Click += new System.EventHandler(this.btnLockGoodFilenames_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(8, 34);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(228, 607);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSelectUnmatchingDateFromName);
            this.tabPage1.Controls.Add(this.btnLockGoodFilenames);
            this.tabPage1.Controls.Add(this.bFilename);
            this.tabPage1.Controls.Add(this.gbBatchRename);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(220, 581);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Filename";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSelectUnmatchingDateFromName
            // 
            this.btnSelectUnmatchingDateFromName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectUnmatchingDateFromName.Location = new System.Drawing.Point(6, 525);
            this.btnSelectUnmatchingDateFromName.Name = "btnSelectUnmatchingDateFromName";
            this.btnSelectUnmatchingDateFromName.Size = new System.Drawing.Size(206, 28);
            this.btnSelectUnmatchingDateFromName.TabIndex = 20;
            this.btnSelectUnmatchingDateFromName.Text = "Select Wrong Dates in filename";
            this.btnSelectUnmatchingDateFromName.UseVisualStyleBackColor = true;
            this.btnSelectUnmatchingDateFromName.Click += new System.EventHandler(this.btnSelectUnmatchingDateFromName_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.txtDateSetterStep);
            this.tabPage2.Controls.Add(this.txtDateSetterStartDate);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.lblDateOffset);
            this.tabPage2.Controls.Add(this.txtDateOffset);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.btnSetExif);
            this.tabPage2.Controls.Add(this.btnSetDate);
            this.tabPage2.Controls.Add(this.btnAddToExif);
            this.tabPage2.Controls.Add(this.btnAddToDate);
            this.tabPage2.Controls.Add(this.btnAutoFixExif);
            this.tabPage2.Controls.Add(this.btnSetExifFromDate);
            this.tabPage2.Controls.Add(this.btnSelectMissingExif);
            this.tabPage2.Controls.Add(this.btnSetDateFromFilename);
            this.tabPage2.Controls.Add(this.btnAutoFixDate);
            this.tabPage2.Controls.Add(this.btnSetDateFromExif);
            this.tabPage2.Controls.Add(this.btnExifFromName);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(220, 581);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Date";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 331);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 13);
            this.label12.TabIndex = 33;
            this.label12.Text = "Step";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 436);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Step";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDateSetterStep
            // 
            this.txtDateSetterStep.Location = new System.Drawing.Point(37, 446);
            this.txtDateSetterStep.Name = "txtDateSetterStep";
            this.txtDateSetterStep.Size = new System.Drawing.Size(173, 20);
            this.txtDateSetterStep.TabIndex = 31;
            this.txtDateSetterStep.Text = "d or d.hh:mm[:ss]";
            // 
            // txtDateSetterStartDate
            // 
            this.txtDateSetterStartDate.Location = new System.Drawing.Point(5, 420);
            this.txtDateSetterStartDate.Name = "txtDateSetterStartDate";
            this.txtDateSetterStartDate.Size = new System.Drawing.Size(204, 20);
            this.txtDateSetterStartDate.TabIndex = 30;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.Location = new System.Drawing.Point(6, 395);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(204, 22);
            this.label11.TabIndex = 27;
            this.label11.Text = "Date Setter";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDateOffset
            // 
            this.lblDateOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateOffset.Location = new System.Drawing.Point(6, 304);
            this.lblDateOffset.Name = "lblDateOffset";
            this.lblDateOffset.Size = new System.Drawing.Size(204, 22);
            this.lblDateOffset.TabIndex = 24;
            this.lblDateOffset.Text = "Date Offset";
            this.lblDateOffset.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtDateOffset
            // 
            this.txtDateOffset.Location = new System.Drawing.Point(37, 329);
            this.txtDateOffset.Name = "txtDateOffset";
            this.txtDateOffset.Size = new System.Drawing.Size(173, 20);
            this.txtDateOffset.TabIndex = 23;
            this.txtDateOffset.Text = "d or d.hh:mm[:ss]";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(6, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(204, 22);
            this.label7.TabIndex = 22;
            this.label7.Text = "Exif Date";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(6, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(204, 22);
            this.label8.TabIndex = 21;
            this.label8.Text = "System Date";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnSetExif
            // 
            this.btnSetExif.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetExif.Location = new System.Drawing.Point(111, 472);
            this.btnSetExif.Name = "btnSetExif";
            this.btnSetExif.Size = new System.Drawing.Size(99, 23);
            this.btnSetExif.TabIndex = 8;
            this.btnSetExif.Text = "Set Exif";
            this.btnSetExif.UseVisualStyleBackColor = true;
            this.btnSetExif.Click += new System.EventHandler(this.btnSetExif_Click);
            // 
            // btnSetDate
            // 
            this.btnSetDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDate.Location = new System.Drawing.Point(5, 472);
            this.btnSetDate.Name = "btnSetDate";
            this.btnSetDate.Size = new System.Drawing.Size(99, 23);
            this.btnSetDate.TabIndex = 8;
            this.btnSetDate.Text = "Set Date";
            this.btnSetDate.UseVisualStyleBackColor = true;
            this.btnSetDate.Click += new System.EventHandler(this.btnSetDate_Click);
            // 
            // btnAddToExif
            // 
            this.btnAddToExif.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddToExif.Location = new System.Drawing.Point(111, 355);
            this.btnAddToExif.Name = "btnAddToExif";
            this.btnAddToExif.Size = new System.Drawing.Size(99, 23);
            this.btnAddToExif.TabIndex = 8;
            this.btnAddToExif.Text = "Add to Exif";
            this.btnAddToExif.UseVisualStyleBackColor = true;
            this.btnAddToExif.Click += new System.EventHandler(this.btnAddToExif_Click);
            // 
            // btnAddToDate
            // 
            this.btnAddToDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddToDate.Location = new System.Drawing.Point(5, 355);
            this.btnAddToDate.Name = "btnAddToDate";
            this.btnAddToDate.Size = new System.Drawing.Size(99, 23);
            this.btnAddToDate.TabIndex = 8;
            this.btnAddToDate.Text = "Add to Date";
            this.btnAddToDate.UseVisualStyleBackColor = true;
            this.btnAddToDate.Click += new System.EventHandler(this.btnAddToDate_Click);
            // 
            // btnAutoFixExif
            // 
            this.btnAutoFixExif.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoFixExif.Location = new System.Drawing.Point(6, 252);
            this.btnAutoFixExif.Name = "btnAutoFixExif";
            this.btnAutoFixExif.Size = new System.Drawing.Size(204, 22);
            this.btnAutoFixExif.TabIndex = 8;
            this.btnAutoFixExif.Text = "AutoFix Exif";
            this.btnAutoFixExif.UseVisualStyleBackColor = true;
            this.btnAutoFixExif.Click += new System.EventHandler(this.btnAutoFixExif_Click);
            // 
            // btnSetExifFromDate
            // 
            this.btnSetExifFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetExifFromDate.Location = new System.Drawing.Point(6, 227);
            this.btnSetExifFromDate.Name = "btnSetExifFromDate";
            this.btnSetExifFromDate.Size = new System.Drawing.Size(204, 22);
            this.btnSetExifFromDate.TabIndex = 7;
            this.btnSetExifFromDate.Text = "Set Exif From Date";
            this.btnSetExifFromDate.UseVisualStyleBackColor = true;
            this.btnSetExifFromDate.Click += new System.EventHandler(this.btnSetExifFromDate_Click);
            // 
            // btnSelectMissingExif
            // 
            this.btnSelectMissingExif.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectMissingExif.Location = new System.Drawing.Point(6, 176);
            this.btnSelectMissingExif.Name = "btnSelectMissingExif";
            this.btnSelectMissingExif.Size = new System.Drawing.Size(204, 22);
            this.btnSelectMissingExif.TabIndex = 5;
            this.btnSelectMissingExif.Text = "Select Missing Exif";
            this.btnSelectMissingExif.UseVisualStyleBackColor = true;
            this.btnSelectMissingExif.Click += new System.EventHandler(this.btnSelectMissingExif_Click);
            // 
            // btnSetDateFromFilename
            // 
            this.btnSetDateFromFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDateFromFilename.Location = new System.Drawing.Point(5, 38);
            this.btnSetDateFromFilename.Name = "btnSetDateFromFilename";
            this.btnSetDateFromFilename.Size = new System.Drawing.Size(205, 22);
            this.btnSetDateFromFilename.TabIndex = 2;
            this.btnSetDateFromFilename.Text = "Set Date From Name";
            this.btnSetDateFromFilename.UseVisualStyleBackColor = true;
            this.btnSetDateFromFilename.Click += new System.EventHandler(this.btnSetDateFromFilename_Click);
            // 
            // btnAutoFixDate
            // 
            this.btnAutoFixDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoFixDate.Location = new System.Drawing.Point(5, 89);
            this.btnAutoFixDate.Name = "btnAutoFixDate";
            this.btnAutoFixDate.Size = new System.Drawing.Size(205, 22);
            this.btnAutoFixDate.TabIndex = 4;
            this.btnAutoFixDate.Text = "AutoFix Date";
            this.btnAutoFixDate.UseVisualStyleBackColor = true;
            this.btnAutoFixDate.Click += new System.EventHandler(this.btnAutoFixDate_Click);
            // 
            // btnSetDateFromExif
            // 
            this.btnSetDateFromExif.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDateFromExif.Location = new System.Drawing.Point(5, 64);
            this.btnSetDateFromExif.Name = "btnSetDateFromExif";
            this.btnSetDateFromExif.Size = new System.Drawing.Size(205, 22);
            this.btnSetDateFromExif.TabIndex = 3;
            this.btnSetDateFromExif.Text = "Set Date From Exif";
            this.btnSetDateFromExif.UseVisualStyleBackColor = true;
            this.btnSetDateFromExif.Click += new System.EventHandler(this.btnSetDateFromExif_Click);
            // 
            // btnExifFromName
            // 
            this.btnExifFromName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExifFromName.Location = new System.Drawing.Point(6, 201);
            this.btnExifFromName.Name = "btnExifFromName";
            this.btnExifFromName.Size = new System.Drawing.Size(204, 22);
            this.btnExifFromName.TabIndex = 6;
            this.btnExifFromName.Text = "Set Exif From Name";
            this.btnExifFromName.UseVisualStyleBackColor = true;
            this.btnExifFromName.Click += new System.EventHandler(this.btnExifFromName_Click);
            // 
            // listView
            // 
            this.listView.AllowColumnReorder = true;
            this.listView.AllowDrop = true;
            this.listView.AllowRowReorder = true;
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(242, 114);
            this.listView.Name = "listView";
            this.listView.Path = "";
            this.listView.ProgressBar = this.progressBar;
            this.listView.Size = new System.Drawing.Size(758, 527);
            this.listView.TabIndex = 0;
            this.listView.ThumbSize = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listView_AfterLabelEdit);
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_DragDrop);
            // 
            // FrmImageRenamer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnApplyChanges);
            this.Controls.Add(this.bLoadThumbnail);
            this.Controls.Add(this.gbThumbnail);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.bLoadMetaData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 700);
            this.Name = "FrmImageRenamer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Sorter";
            this.Load += new System.EventHandler(this.FrmImageRenamer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbThumbSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iInsert)).EndInit();
            this.gbBatchRename.ResumeLayout(false);
            this.gbBatchRename.PerformLayout();
            this.gbDateCounter.ResumeLayout(false);
            this.gbDateCounter.PerformLayout();
            this.gbCounter.ResumeLayout(false);
            this.gbCounter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iDigits)).EndInit();
            this.gbRemove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iRemoveLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iRemovePosition)).EndInit();
            this.gbThumbnail.ResumeLayout(false);
            this.gbThumbnail.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// Point d'entr�e principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new FrmImageRenamer());
        }

        private void FrmImageRenamer_Load(object sender, System.EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtFolder.Text;
            txtDateCounterStartDate.Text = DateTime.Now.ToString();
            txtDateSetterStartDate.Text = DateTime.Now.ToString();
            txtFolder_TextChanged(null, null);
            bLoadThumbnail_CheckedChanged(null, null);
            bBatchRename_CheckedChanged(null, null);
            bClearFilename_CheckedChanged(null, null);
        }

        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void txtFolder_TextChanged(object sender, System.EventArgs e)
        {
            if (Directory.Exists(txtFolder.Text))
            {
                folderBrowserDialog.SelectedPath = txtFolder.Text;
                listView.LoadFolder(txtFolder.Text);
            }
        }

        private void txtFolder_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            txtFolder.Text = ((String[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private void txtFolder_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (Directory.Exists(((String[])e.Data.GetData(DataFormats.FileDrop))[0]))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void bLoadThumbnail_CheckedChanged(object sender, System.EventArgs e)
        {
            gbThumbnail.Enabled = bLoadThumbnail.Checked;
            btnThumbSizeOk_Click(sender, e);
        }

        private void bLoadMetaData_CheckedChanged(object sender, System.EventArgs e)
        {
            listView.MetaDataRequired = bLoadMetaData.Checked;
            btnThumbSizeOk_Click(sender, e);
        }

        private void btnThumbSizeOk_Click(object sender, System.EventArgs e)
        {
            listView.ThumbSize = bLoadThumbnail.Checked ? tbThumbSize.Value * 16 : 0;
            listView.RefreshListViewItems();
        }

        private void listView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void bBatchRename_CheckedChanged(object sender, System.EventArgs e)
        {
            gbBatchRename.Enabled = bFilename.Checked;
            txtNewFilenamePattern_TextChanged(null, null);
        }
        private void bCounter_CheckedChanged(object sender, System.EventArgs e)
        {
            if (bCounter.Checked)
            {
                this.gbCounter.Enabled = true;
                this.gbCounter.Visible = true;
                this.bDateCounter.Checked = false;
            }
            else
                this.gbCounter.Enabled = false;

            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void bDateCounter_CheckedChanged(object sender, EventArgs e)
        {
            if (bDateCounter.Checked)
            {
                this.gbDateCounter.Enabled = true;
                this.gbDateCounter.Visible = true;
                this.bCounter.Checked = false;
            }
            else
                this.gbDateCounter.Enabled = false;

            txtNewFilenamePattern_TextChanged(null, null);

        }
        private void bClearFilename_CheckedChanged(object sender, System.EventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void bInsert_CheckedChanged(object sender, System.EventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void bRemove_CheckedChanged(object sender, System.EventArgs e)
        {
            gbRemove.Enabled = bRemove.Checked;
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void iStart_ValueChanged(object sender, System.EventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void iStep_ValueChanged(object sender, System.EventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }
        private void iDigits_ValueChanged(object sender, System.EventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void iInsert_ValueChanged(object sender, System.EventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void iRemoveLength_ValueChanged(object sender, System.EventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void iRemovePosition_ValueChanged(object sender, System.EventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void bClearExtension_CheckedChanged(object sender, System.EventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void cbChangeExtension_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((ExtensionTransformation)cbChangeExtension.SelectedValue == ExtensionTransformation.NoChange
                || (ExtensionTransformation)cbChangeExtension.SelectedValue == ExtensionTransformation.ToLower
                || (ExtensionTransformation)cbChangeExtension.SelectedValue == ExtensionTransformation.ToUpper)
                txtNewExtension.Visible = false;
            else
                txtNewExtension.Visible = true;
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void txtNewExtension_TextChanged(object sender, System.EventArgs e)
        {
            txtNewFilenamePattern_TextChanged(null, null);
        }

        private void txtNewFilenamePattern_TextChanged(object sender, System.EventArgs e)
        {
        }
        private void btnPreviewOnSelection_Click(object sender, EventArgs e)
        {
            int relativeBatchIndex = 0;
            if (listView.SelectedItems.Count > 0)
                foreach (ListViewItem listViewItem in listView.SelectedItems)
                {
                    listView.SetNewFilename(listViewItem, GenerateNewFilename(listViewItem, relativeBatchIndex));
                    relativeBatchIndex++;
                }
            else
                foreach (ListViewItem listViewItem in listView.Items)
                {
                    listView.SetNewFilename(listViewItem, GenerateNewFilename(listViewItem, relativeBatchIndex));
                    relativeBatchIndex++;
                }
        }


        private void btnApplyChanges_Click(object sender, System.EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                foreach (ListViewItem listViewItem in listView.SelectedItems)
                {
                    listView.ApplyChanges(listViewItem);
                }
            else
                foreach (ListViewItem listViewItem in listView.Items)
                {
                    listView.ApplyChanges(listViewItem);
                }
            this.Refresh();
        }

        private void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            listView.SetNewFilename(listView.Items[e.Item], GenerateNewFilename(listView.Items[e.Item]));
        }

        private void txtDateFormatForFilename_TextChanged(object sender, EventArgs e)
        {
            if (txtDateFormatForFilename.Text == "")
                txtDateFormatForFilename.Text = DATE_FILENAMEFORMAT;
        }

        private string GenerateNewFilename(ListViewItem listViewItem, int relativeBatchIndex = 0)
        {
            ImageInfo imageInfo = (ImageInfo)listViewItem.Tag;
            if (!bFilename.Checked)
            {
                if (imageInfo.NewFilenameLocked)
                    return imageInfo.NewFilename;
                else
                    return imageInfo.FileInfo.Name;
            }
            int absoluteItemIndex = listViewItem.Index;
            string filename = imageInfo.FileInfo.Name;
            string path = imageInfo.FileInfo.DirectoryName;
            string NewFilename = imageInfo.FileInfo.Name.Substring(0, imageInfo.FileInfo.Name.Length - imageInfo.FileInfo.Extension.Length);
            string NewExtension = imageInfo.FileInfo.Extension;
            switch ((ExtensionTransformation)cbChangeExtension.SelectedValue)
            {
                case ExtensionTransformation.Replace:
                    NewExtension = txtNewExtension.Text;
                    break;
                case ExtensionTransformation.ToUpper:
                    NewExtension = NewExtension.ToUpper();
                    break;
                case ExtensionTransformation.ToLower:
                    NewExtension = NewExtension.ToLower();
                    break;
                default:
                    break;
            }
            string strCounter = "";
            if (bCounter.Checked)
            {
                String counterFormat = new String('0', (int)iDigits.Value);
                int counter;
                if (bRelativeCounter.Checked)
                    counter = (int)iStart.Value + (int)iStep.Value * relativeBatchIndex;
                else
                    counter = (int)iStart.Value + (int)iStep.Value * absoluteItemIndex;
                strCounter = (counter % (Math.Pow(10, (int)iDigits.Value))).ToString(counterFormat);
            }
            if (bDateCounter.Checked)
            {
                DateTime computedDate = ComputeDateIncrement(txtDateCounterStartDate, txtDateCounterIncrement, relativeBatchIndex); ;
                strCounter = computedDate.ToString(txtDateFormatForFilename.Text);
            }
            string NewFilePattern = txtNewFilenamePattern.Text
            .Replace("%COUNTER", strCounter)
            .Replace("%EXIF", imageInfo.NewExifDate.ToString(txtDateFormatForFilename.Text))
            .Replace("%DATE", imageInfo.NewWriteDate.ToString(txtDateFormatForFilename.Text));
            ;
            if (bClearFilename.Checked)
                NewFilename = "";
            if (bInsert.Checked)
                NewFilename = NewFilename.Insert(Math.Min((int)iInsert.Value, NewFilename.Length), NewFilePattern);
            else
            {
                NewFilename =
                    NewFilename.Substring(0,
                    Math.Max(0, Math.Min(
                    (int)iInsert.Value,
                    NewFilename.Length - NewFilePattern.Length)))
                    + NewFilePattern
                    + NewFilename.Substring(
                    Math.Max(0, Math.Min(
                    (int)iInsert.Value + NewFilePattern.Length,
                    NewFilename.Length)));
            }
            if (bRemove.Checked)
            {
                int startIndex = Math.Min((int)iRemovePosition.Value, NewFilename.Length - 1);
                int count = Math.Min((int)iRemoveLength.Value, NewFilename.Length - startIndex);
                NewFilename = NewFilename.Remove(startIndex, count);
            }
            NewFilename = NewFilename + NewExtension;
            bool bWillExist = false;
            ArrayList ExcludedFilenames = new ArrayList();
            for (int i = 0; i < absoluteItemIndex; i++)
            {
                string name = ((ImageInfo)listView.Items[i].Tag).NewFilename.ToLower();
                ExcludedFilenames.Add(name);
                if (name.ToLower() == NewFilename.ToLower())
                    bWillExist = true;
            }
            if (bWillExist || (NewFilename.ToLower() != filename.ToLower() && File.Exists(path + "\\" + NewFilename)))
            {
                NewFilename = Utils.DeclineFilename(NewFilename, path, ExcludedFilenames);
            }
            return NewFilename;
        }

        private void btnSetDateFromFilename_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                try
                {
                    DateTime filenameDate = DateTime.ParseExact(
                         imageInfo.NewFilename.Substring(0, txtDateFormatForFilename.Text.Length),
                         txtDateFormatForFilename.Text, CultureInfo.InvariantCulture);
                    if (filenameDate.ToString() != imageInfo.NewWriteDate.ToString())
                    {
                        imageInfo.NewWriteDate = filenameDate;
                        imageInfo.NewWriteDateLocked = true;
                        listView.RefreshListViewItem(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }

        }

        private void btnSetDateFromExif_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                if (imageInfo.NewExifDate.ToString() != DateTime.MinValue.ToString()
                    && imageInfo.NewWriteDate.ToString() != imageInfo.NewExifDate.ToString())
                {
                    imageInfo.NewWriteDate = imageInfo.NewExifDate;
                    imageInfo.NewWriteDateLocked = true;
                    listView.RefreshListViewItem(item);
                }
            }
        }

        private void btnExifFromName_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                try
                {
                    DateTime filenameDate = DateTime.ParseExact(
                        imageInfo.NewFilename.Substring(0, txtDateFormatForFilename.Text.Length),
                        txtDateFormatForFilename.Text, CultureInfo.InvariantCulture);
                    if (filenameDate.ToString() != imageInfo.NewExifDate.ToString())
                    {
                        imageInfo.NewExifDate = filenameDate;
                        imageInfo.NewExifDateLocked = true;
                        listView.RefreshListViewItem(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private void btnSetExifFromDate_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                if (imageInfo.NewWriteDate.ToString() != imageInfo.NewExifDate.ToString())
                {
                    imageInfo.NewExifDate = imageInfo.NewWriteDate;
                    imageInfo.NewExifDateLocked = true;
                    listView.RefreshListViewItem(item);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.Items)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                imageInfo.Reset();
                listView.RefreshListViewItem(item);
            }
        }

        private void btnAutoFixExif_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                if (imageInfo.NewExifDateLocked) return;
                if (imageInfo.NewExifDate.ToString() != DateTime.MinValue.ToString()) return;
                ImageInfo prevImageInfo = null;
                ImageInfo nextImageInfo = null;
                if (item.Index > 0)
                    prevImageInfo = (ImageInfo)listView.Items[item.Index - 1].Tag;
                if (item.Index < listView.Items.Count - 1)
                    nextImageInfo = (ImageInfo)listView.Items[item.Index + 1].Tag;
                if (prevImageInfo == null && nextImageInfo == null) return;
                if (prevImageInfo == null)
                    prevImageInfo = nextImageInfo;
                if (nextImageInfo == null)
                    nextImageInfo = prevImageInfo;
                imageInfo.NewExifDate = prevImageInfo.NewExifDate.AddSeconds((nextImageInfo.NewExifDate - prevImageInfo.NewExifDate).TotalSeconds / 2d);
                listView.RefreshListViewItem(item);
            }
        }

        private void btnAutoFixDate_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                if (imageInfo.NewWriteDateLocked) return;
                ImageInfo prevImageInfo = null;
                ImageInfo nextImageInfo = null;
                if (item.Index > 0)
                    prevImageInfo = (ImageInfo)listView.Items[item.Index - 1].Tag;
                if (item.Index < listView.Items.Count - 1)
                    nextImageInfo = (ImageInfo)listView.Items[item.Index + 1].Tag;
                if (prevImageInfo == null && nextImageInfo == null) return;
                if (prevImageInfo == null)
                    prevImageInfo = nextImageInfo;
                if (nextImageInfo == null)
                    nextImageInfo = prevImageInfo;
                imageInfo.NewWriteDate = prevImageInfo.NewWriteDate.AddSeconds((nextImageInfo.NewWriteDate - prevImageInfo.NewWriteDate).TotalSeconds / 2d);
                listView.RefreshListViewItem(item);
            }
        }

        private void btnSelectMissingExif_Click(object sender, EventArgs e)
        {
            listView.SelectedItems.Clear();
            foreach (ListViewItem item in listView.Items)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                if (imageInfo.HasMissingExifDate())
                    item.Selected = true;
            }
        }

        private void btnSelectUnmatchingDateFromName_Click(object sender, EventArgs e)
        {
            listView.SelectedItems.Clear();
            foreach (ListViewItem item in listView.Items)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                try
                {
                    DateTime filenameDate = DateTime.ParseExact(
                        imageInfo.NewFilename.Substring(0, txtDateFormatForFilename.Text.Length),
                        txtDateFormatForFilename.Text, CultureInfo.InvariantCulture);
                    if (Math.Abs((filenameDate - imageInfo.NewWriteDate).TotalSeconds) > 2d)
                        item.Selected = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    item.Selected = true;
                }
            }
        }

        private void btnLockGoodFilenames_Click(object sender, EventArgs e)
        {
            // unlock
            if (btnLockGoodFilenames.Tag != null)
            {
                foreach (ListViewItem item in listView.Items)
                {
                    ImageInfo imageInfo = (ImageInfo)item.Tag;
                    imageInfo.NewFilenameLocked = false;
                    listView.RefreshListViewItem(item);
                }
                btnLockGoodFilenames.Text = btnLockGoodFilenames.Tag.ToString();
                btnLockGoodFilenames.Tag = null;
            }
            else //lock
            {
                string offset =
                           bClearFilename.Checked
                           ? ""
                           : ".{" + iInsert.Value + "}";
                string counterRegex = "[0-9]{" + (int)iDigits.Value + "}";
                string dateRegex = txtDateFormatForFilename.Text
                    .Replace("yyyy", "[1-2][0-9]{3}")
                    .Replace("MM", "((0[1-9])|(1[0-2]))")
                    .Replace("dd", "(([0-2][0-9])|(3[0-1]))")
                    .Replace("HH", "(([0-1][0-9])|(2)[0-3])")
                    .Replace("mm", "[0-5][0-9]")
                    .Replace("ss", "[0-5][0-9]")
                    ;
                string pattern =
                    offset
                    + txtNewFilenamePattern.Text
                    .Replace("%COUNTER", counterRegex)
                    .Replace("%DATE", dateRegex)
                    .Replace("%EXIF", dateRegex);
                Regex regex = new Regex(pattern);
                foreach (ListViewItem item in listView.Items)
                {
                    ImageInfo imageInfo = (ImageInfo)item.Tag;
                    try
                    {

                        if (regex.IsMatch(imageInfo.NewFilename))
                            imageInfo.NewFilenameLocked = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                    listView.RefreshListViewItem(item);
                }
                btnLockGoodFilenames.Tag = btnLockGoodFilenames.Text;
                btnLockGoodFilenames.Text = "Unlock all";
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                btnPreviewOnSelection.Text = "Preview on Selection";
                btnApplyChanges.Text = "Apply to Selection";
            }
            else
            {
                btnPreviewOnSelection.Text = "Preview on all";
                btnApplyChanges.Text = "Apply to all";
            }
        }

        private void btnAddToDate_Click(object sender, EventArgs e)
        {
            TimeSpan offset = TimeSpan.Parse(txtDateOffset.Text);
            foreach (ListViewItem item in listView.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                if (!imageInfo.NewWriteDateLocked)
                    imageInfo.NewWriteDate += offset;
                listView.RefreshListViewItem(item);
            }
        }

        private void btnAddToExif_Click(object sender, EventArgs e)
        {
            TimeSpan offset = TimeSpan.Parse(txtDateOffset.Text);
            foreach (ListViewItem item in listView.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                if (!imageInfo.NewExifDateLocked)
                    imageInfo.NewExifDate += offset;
                listView.RefreshListViewItem(item);
            }
        }

        private DateTime ComputeDateIncrement(TextBox txtStartDate, TextBox txtIncrement, int relativeBatchIndex)
        {
            DateTime startDate;
            if (DateTime.TryParse(txtDateSetterStartDate.Text, out startDate)
                ||
                DateTime.TryParseExact(txtDateSetterStartDate.Text, txtDateFormatForFilename.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                startDate += TimeSpan.FromSeconds(TimeSpan.Parse(txtIncrement.Text).TotalSeconds * relativeBatchIndex);
            }
            return startDate;
        }

        private void btnSetDate_Click(object sender, EventArgs e)
        {
            int itemIndex = 0;
            foreach (ListViewItem item in listView.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                if (!imageInfo.NewWriteDateLocked)
                    imageInfo.NewWriteDate = ComputeDateIncrement(txtDateSetterStartDate, txtDateSetterStep, itemIndex);
                listView.RefreshListViewItem(item);
                itemIndex++;
            }
        }

        private void btnSetExif_Click(object sender, EventArgs e)
        {
            int itemIndex = 0;
            foreach (ListViewItem item in listView.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                if (!imageInfo.NewExifDateLocked)
                    imageInfo.NewExifDate = ComputeDateIncrement(txtDateSetterStartDate, txtDateSetterStep, itemIndex);
                listView.RefreshListViewItem(item);
                itemIndex++;
            }
        }

        private void listView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            ImageInfo imageInfo = (ImageInfo)(e.Item.Tag);
            e.DrawDefault = true;
            switch ((MyListView.ColumnIndexes)e.ColumnIndex)
            {
                case MyListView.ColumnIndexes.THUMBNAIL_INDEX:

                    if (imageInfo.ThumbImage != null && listView.ThumbSize != 0)
                        e.Graphics.DrawImage(imageInfo.ThumbImage,
                            new RectangleF(e.Bounds.X + e.Bounds.Height / 2 - imageInfo.ThumbImage.Width / 2,
                                e.Bounds.Y + e.Bounds.Height / 2 - imageInfo.ThumbImage.Height / 2,
                                imageInfo.ThumbImage.Width,
                                imageInfo.ThumbImage.Height));
                    e.DrawDefault = false;
                    break;
                case MyListView.ColumnIndexes.NAME_INDEX:
                    e.DrawText(TextFormatFlags.EndEllipsis);
                    e.DrawDefault = false;
                    break;
                case MyListView.ColumnIndexes.NEW_NAME_INDEX:
                    string text = e.SubItem.Text;
                    while ((int)e.Graphics.MeasureString(text, e.SubItem.Font).Width > e.Item.ListView.Columns[e.ColumnIndex].Width && text.Length > 6)
                    {
                        text = text.Substring(0, text.Length - 6) + "...";
                    }
                    e.Graphics.DrawString(text, e.SubItem.Font, new SolidBrush(e.SubItem.ForeColor),
                        e.Bounds.X, e.Bounds.Y + e.Bounds.Height / 2 - e.SubItem.Font.GetHeight() / 2);
                    e.DrawDefault = false;
                    break;
                case MyListView.ColumnIndexes.FILESIZE_INDEX:
                    break;
                case MyListView.ColumnIndexes.WRITEDATE_INDEX:
                    break;
                case MyListView.ColumnIndexes.EXIFDATE_INDEX:
                    break;
                default:
                    break;
            }

        }

        private void listView_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.State == DrawItemState.Selected)//.(State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            }
            if (listView.DropIndex == e.Index)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds.X, e.Bounds.Y - 1, e.Bounds.Width, 3);
            }
            else if (listView.DropIndex == e.Index + 1)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds.X, e.Bounds.Y + e.Bounds.Height - 1, e.Bounds.Width, 3);
            }
        }

        private void listView_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = listView.ThumbSize;
        }
    }
}
