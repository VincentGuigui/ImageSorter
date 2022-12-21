using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;

namespace ImageRenamer
{
    /// <summary>
    /// Description résumée de MyListView.
    /// </summary>
    public class MyListView : System.Windows.Forms.ListView
    {
        public enum ColumnIndexes
        {
            THUMBNAIL_INDEX = 0,
            NAME_INDEX,
            NEW_NAME_INDEX,
            FILESIZE_INDEX,
            WRITEDATE_INDEX,
            EXIFDATE_INDEX
        }
        private const int THUMBNAIL_INDEX = (int)ColumnIndexes.THUMBNAIL_INDEX;
        private const int NAME_INDEX = (int)ColumnIndexes.NAME_INDEX;
        private const int NEW_NAME_INDEX = (int)ColumnIndexes.NEW_NAME_INDEX;
        private const int FILESIZE_INDEX = (int)ColumnIndexes.FILESIZE_INDEX;
        private const int WRITEDATE_INDEX = (int)ColumnIndexes.WRITEDATE_INDEX;
        private const int EXIFDATE_INDEX = (int)ColumnIndexes.EXIFDATE_INDEX;
        private bool manualReorder = false;
        private int[] MINIMALIST_VIEW = { THUMBNAIL_INDEX, NEW_NAME_INDEX, WRITEDATE_INDEX, EXIFDATE_INDEX };
        private bool minimalistView;
        public bool MinimalistView
        {
            get { return minimalistView; }
            set
            {
                if (chThumb == null) return;
                if (minimalistView != value)
                {
                    minimalistView = value;
                    if (minimalistView)
                    {
                        chThumb.Tag = chThumb.Width;
                        chFilename.Tag = chFilename.Width;
                        chNewFilename.Tag = chNewFilename.Width;
                        chSize.Tag = chSize.Width;
                        chDate.Tag = chDate.Width;
                        chExifDate.Tag = chExifDate.Width;

                        chFilename.Width = 0;
                        chSize.Width = 0;
                    }
                    else
                    {
                        chFilename.Width = (int)chFilename.Tag;
                        chSize.Width = (int)chSize.Tag;
                    }
                    this.Invalidate();
                }
            }
        }
        private string[] headers = { "Thumbnail", "Filename", "New filename", "Filesize", "Date", "Exif Date" };
        private String path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private ListViewItem previousHoverItem = null;
        public ProgressBar ProgressBar
        {
            get; set;
        }
        public int DropIndex = -1;
        private int thumbSize = 0;
        public int ThumbSize
        {
            get
            {
                return thumbSize;
            }
            set
            {
                thumbSize = value;
                chThumb.Width = value;
                this.Redraw();
            }
        }
        public bool MetaDataRequired = false;
        private const String REORDER = "Reorder";
        private MyListViewItemSorter lvSorter;
        private System.Windows.Forms.ColumnHeader chThumb;
        private System.Windows.Forms.ColumnHeader chFilename;
        private System.Windows.Forms.ColumnHeader chNewFilename;
        private System.Windows.Forms.ColumnHeader chSize;
        private System.Windows.Forms.ColumnHeader chDate;
        private System.Windows.Forms.ColumnHeader chExifDate;
        BufferedGraphics bufferedGraphics;
        private bool allowRowReorder = true;
        public bool AllowRowReorder
        {
            get
            {
                return this.allowRowReorder;
            }
            set
            {
                this.allowRowReorder = value;
                base.AllowDrop = value;
            }
        }
        public new SortOrder Sorting
        {
            get
            {
                return SortOrder.None;
            }
            set
            {
                base.Sorting = SortOrder.None;
            }
        }
        private ListViewItem SelectedItem;
        private ListViewItem.ListViewSubItem SelectedSubItem;
        private int SelectedSubItemIndex;

        public MyListView() : base()
        {
            this.allowRowReorder = true;
            InitializeComponent();

            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.DoubleBuffered = true;
            bufferedGraphics = BufferedGraphicsManager.Current.Allocate(this.CreateGraphics(), this.Bounds);
        }

        #region Code généré par le Concepteur Windows Form
        private void InitializeComponent()
        {
            this.lvSorter = new MyListViewItemSorter();
            this.chThumb = new System.Windows.Forms.ColumnHeader();
            this.chFilename = new System.Windows.Forms.ColumnHeader();
            this.chNewFilename = new System.Windows.Forms.ColumnHeader();
            this.chSize = new System.Windows.Forms.ColumnHeader();
            this.chDate = new System.Windows.Forms.ColumnHeader();
            this.chExifDate = new System.Windows.Forms.ColumnHeader();
            this.lvSorter.SortColumn = NEW_NAME_INDEX;
            this.lvSorter.Order = SortOrder.Ascending;

            // 
            // chThumb
            // 
            this.chThumb.Text = headers[THUMBNAIL_INDEX];
            this.chThumb.Width = 64;
            // 
            // chFilename
            // 
            this.chFilename.Text = headers[NAME_INDEX];
            this.chFilename.Width = 200;
            // 
            // chNewFilename
            // 
            this.chNewFilename.Text = headers[NEW_NAME_INDEX];
            this.chNewFilename.Width = 200;
            // 
            // chSize
            // 
            this.chSize.Text = headers[FILESIZE_INDEX];
            this.chSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.chSize.Width = 100;
            // 
            // chDate
            // 
            this.chDate.Text = headers[WRITEDATE_INDEX];
            this.chDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.chDate.Width = 120;
            // 
            // chExifDate
            // 
            this.chExifDate.Text = headers[EXIFDATE_INDEX];
            this.chExifDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.chExifDate.Width = 120;
            // 
            // MyListView
            // 
            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                              this.chThumb,
                                                                              this.chFilename,
                                                                              this.chNewFilename,
                                                                              this.chSize,
                                                                              this.chDate,
                                                                              this.chExifDate});
            this.ListViewItemSorter = lvSorter;
            this.FullRowSelect = true;
            this.HideSelection = false;
            this.MinimalistView = true;
            this.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MyListView_KeyDown);
            this.MouseDown += MyListView_MouseDown;
            this.MouseClick += MyListView_MouseClick;
            this.MouseDoubleClick += MyListView_MouseDoubleClick;
        }

        #endregion

        #region ListViewItemSorter class
        class MyListViewItemSorter : IComparer
        {
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            private int ColumnToSort;

            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort;

            /// <summary>
            /// Case insensitive comparer object
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            /// <summary>
            /// Class constructor. Initializes various elements
            /// </summary>
            public MyListViewItemSorter()
            {
                // Initialize the column to '0'
                ColumnToSort = 0;

                // Initialize the sort order to 'none'
                OrderOfSort = SortOrder.None;

                // Initialize the CaseInsensitiveComparer object
                ObjectCompare = new CaseInsensitiveComparer();
            }

            /// <summary>
            /// This method is inherited from the IComparer interface. It compares the two objects passed using a case insensitive comparison.
            /// </summary>
            /// <param name="x">First object to be compared</param>
            /// <param name="y">Second object to be compared</param>
            /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // Cast the objects to be compared to ListViewItem objects
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                // Compare the two items
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }

            /// <summary>
            /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
            /// </summary>
            public int SortColumn
            {
                set
                {
                    ColumnToSort = value;
                }
                get
                {
                    return ColumnToSort;
                }
            }

            /// <summary>
            /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
            /// </summary>
            public SortOrder Order
            {
                set
                {
                    OrderOfSort = value;
                }
                get
                {
                    return OrderOfSort;
                }
            }

        }
        #endregion

        #region Drag Drop
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            if (!this.AllowRowReorder)
            {
                return;
            }
            base.DoDragDrop(REORDER, DragDropEffects.Move);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            bool bReorder = (this.AllowRowReorder && e.Data.GetDataPresent(DataFormats.Text) && e.Data.GetData(DataFormats.Text).ToString() == REORDER);
            this.SuspendLayout();
            if (bReorder || e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (bReorder)
                {
                    e.Effect = DragDropEffects.Move;
                }
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                Point cp = base.PointToClient(new Point(e.X, e.Y));
                ListViewItem dragToItem = base.GetItemAt(cp.X, cp.Y);
                if (dragToItem != null)
                {
                    if (cp.Y < (dragToItem.Bounds.Y + dragToItem.Bounds.Height / 2))
                        DropIndex = dragToItem.Index;
                    else
                        DropIndex = (dragToItem.Index + 1);
                    if (DropIndex < this.Items.Count)
                    {
                        dragToItem = this.Items[DropIndex];
                        dragToItem.EnsureVisible();
                        if (DropIndex > 0)
                            this.Items[DropIndex - 1].EnsureVisible();
                    }
                    else
                        dragToItem = null;
                }
                else //if (base.Items.Count >= base.SelectedItems.Count)
                {
                    DropIndex = base.Items.Count;
                }

                if (bReorder)
                {
                    foreach (ListViewItem moveItem in base.SelectedItems)
                    {
                        if (moveItem.Index == DropIndex)
                        {
                            e.Effect = DragDropEffects.None;
                            moveItem.BackColor = SystemColors.Window;
                        }
                    }
                }
                if (this.previousHoverItem != dragToItem)
                {
                    if (this.previousHoverItem != null)
                    {
                        //this.previousHoverItem.BackColor = SystemColors.Window;
                        this.Invalidate(new Rectangle(previousHoverItem.Bounds.X, previousHoverItem.Bounds.Y - 1, previousHoverItem.Bounds.Width, 3));
                    }
                    else if (this.Items.Count > 0)
                        this.Invalidate(new Rectangle(this.Items[this.Items.Count - 1].Bounds.X, this.Items[this.Items.Count - 1].Bounds.Y + this.Items[this.Items.Count - 1].Bounds.Height - 1, this.Items[this.Items.Count - 1].Bounds.Width, 3));
                    else if (this.Items.Count == 0)
                        this.Invalidate();
                    //					if (dragToItem != null)
                    //					{
                    //						//dragToItem.BackColor = SystemColors.HotTrack;
                    //						DropIndex = DropIndex;
                    //					}
                    //					else
                    //					{
                    //						DropIndex = this.Items.Count;
                    //					}
                    this.previousHoverItem = dragToItem;
                }
                if (dragToItem != null)
                {
                    this.Invalidate(new Rectangle(dragToItem.Bounds.X, dragToItem.Bounds.Y - 1, dragToItem.Bounds.Width, 3));
                }
                else if (this.Items.Count > 0)
                    this.Invalidate(new Rectangle(this.Items[this.Items.Count - 1].Bounds.X, this.Items[this.Items.Count - 1].Bounds.Y + this.Items[this.Items.Count - 1].Bounds.Height - 1, this.Items[this.Items.Count - 1].Bounds.Width, 3));
                else if (this.Items.Count == 0)
                    this.Invalidate();
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
            this.ResumeLayout();
            base.OnDragOver(e);
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            bool bReorder = (this.AllowRowReorder && e.Data.GetDataPresent(DataFormats.Text) && e.Data.GetData(DataFormats.Text).ToString() == REORDER);
            this.SuspendLayout();
            ListViewItem dragToItem = null;
            if (bReorder && base.SelectedItems.Count > 0)
            {
                Point cp = base.PointToClient(new Point(e.X, e.Y));
                dragToItem = base.GetItemAt(cp.X, cp.Y);
                if (dragToItem != null)
                {
                    if (cp.Y < (dragToItem.Bounds.Y + dragToItem.Bounds.Height / 2))
                        DropIndex = dragToItem.Index;
                    else
                        DropIndex = (dragToItem.Index + 1);
                    if (DropIndex < this.Items.Count)
                        dragToItem = this.Items[DropIndex];
                    else
                        dragToItem = null;
                }
                else// if (base.Items.Count - base.SelectedItems.Count > 0)
                {
                    DropIndex = base.Items.Count;
                }
                ArrayList insertItems = new ArrayList(base.SelectedItems.Count);
                foreach (ListViewItem item in base.SelectedItems)
                {
                    insertItems.Add(item.Clone());
                }
                for (int i = insertItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem insertItem = (ListViewItem)insertItems[i];
                    this.Items.Insert(DropIndex, insertItem);
                }
                foreach (ListViewItem removeItem in base.SelectedItems)
                {
                    base.Items.Remove(removeItem);
                }
                for (int i = insertItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem insertItem = (ListViewItem)insertItems[i];
                    insertItem.Focused = true;
                    insertItem.Selected = true;
                }
                this.manualReorder = true;
                this.lvSorter.Order = SortOrder.None;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (Directory.Exists(((String[])e.Data.GetData(DataFormats.FileDrop))[0]))
                {
                    this.LoadFolder(((String[])e.Data.GetData(DataFormats.FileDrop))[0]);
                }
                else
                {
                    String[] files = ((String[])e.Data.GetData(DataFormats.FileDrop));
                    Point cp = base.PointToClient(new Point(e.X, e.Y));
                    dragToItem = base.GetItemAt(cp.X, cp.Y);
                    if (dragToItem != null)
                    {
                        if (cp.Y < (dragToItem.Bounds.Y + dragToItem.Bounds.Height / 2))
                            DropIndex = dragToItem.Index;
                        else
                            DropIndex = (dragToItem.Index + 1);
                        this.manualReorder = true;
                        this.lvSorter.Order = SortOrder.None;
                    }
                    else if (base.Items.Count - base.SelectedItems.Count >= 0)
                    {
                        DropIndex = base.Items.Count;
                        if (base.Items.Count > 0)
                        {
                            this.manualReorder = true;
                            this.lvSorter.Order = SortOrder.None;
                        }
                    }
                    this.InsertItems(files, DropIndex);
                }
            }
            if (dragToItem != null)
            {
                this.Invalidate(new Rectangle(dragToItem.Bounds.X, dragToItem.Bounds.Y - 1, dragToItem.Bounds.Width, 3));
            }
            else if (this.Items.Count > 0)
                this.Invalidate(new Rectangle(this.Items[this.Items.Count - 1].Bounds.X, this.Items[this.Items.Count - 1].Bounds.Y + this.Items[this.Items.Count - 1].Bounds.Height - 1, this.Items[this.Items.Count - 1].Bounds.Width, 3));
            else if (this.Items.Count == 0)
                this.Invalidate();
            DropIndex = -1;
            base.OnDragDrop(e);
            this.ResumeLayout();
        }
        #endregion

        #region Custom Draw
        #region Structures

        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int code;
        }
        private struct MEASUREITEMSTRUCT
        {
            public int CtlType;
            public int CtlID;
            public int itemID;
            public int itemWidth;
            public int itemHeight;
            public IntPtr itemData;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private struct DrawItemStruct
        {
            public int ctlType;
            public int ctlID;
            public int itemID;
            public int itemAction;
            public int itemState;
            public IntPtr hWndItem;
            public IntPtr hDC;
            public RECT rcItem;
            public IntPtr itemData;
        }
        struct NMCUSTOMDRAW
        {
            public NMHDR nmcd;
            public int dwDrawStage;
            public IntPtr hdc;
            public RECT rc;
            public int dwItemSpec;
            public int uItemState;
            public IntPtr lItemlParam;
        }

        struct NMLVCUSTOMDRAW
        {
            public NMCUSTOMDRAW nmcd;
            public int clrText;
            public int clrTextBk;
            public int iSubItem;
            public int dwItemType;
            public int clrFace;
            public int iIconEffect;
            public int iIconPhase;
            public int iPartId;
            public int iStateId;
            public RECT rcText;
            public uint uAlign;
        }
        #endregion
        #region Enumération
        private enum ReflectedMessages
        {
            OCM__BASE = (0x0400 + 0x1c00),
            OCM_DRAWITEM = (OCM__BASE + 0x002B),
            OCM_MESUREITEM = (OCM__BASE + 0x002C),
            OCM_CTLCOLOREDIT = (OCM__BASE + 0x001D),
        }
        #endregion

        public const int LVS_OWNERDRAWFIXED = 0x0400;
        private bool isInWmPaintMsg = false;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);

            Console.WriteLine(m.Msg);
            switch (m.Msg)
            {
                case 0x14: // WM_ERASEBKGND
                    //bufferedGraphics.Graphics.Clear(this.BackColor);
                    break;
                case 0x0F: // WM_PAINT
                    this.isInWmPaintMsg = true;
                    //base.WndProc(ref m);
                    //bufferedGraphic.Graphics.Clear(this.BackColor);
                    this.isInWmPaintMsg = false;
                    break;
                case 0x4E: // WM_NOTIFY:
                    //NMHDR nmhdr = (NMHDR)m.GetLParam(typeof(NMHDR));
                    break;
                case 0x204E: // WM_REFLECT_NOTIFY
                    NMHDR nmhdr = (NMHDR)m.GetLParam(typeof(NMHDR));
                    if (nmhdr.code == -12) // NM_CUSTOMDRAW 
                    {
                        //if (this.isInWmPaintMsg)
                        //    base.WndProc(ref m);
                        NMLVCUSTOMDRAW nmcustomdraw = (NMLVCUSTOMDRAW)m.GetLParam(typeof(NMLVCUSTOMDRAW));

                        if (nmcustomdraw.nmcd.dwDrawStage == 0) // CDDS_POSTPAINT
                        {
                            //bufferedGraphics.Graphics.Clear(this.BackColor);
                        }
                    }
                    //else
                    //    base.WndProc(ref m);
                    break;
                case (int)ReflectedMessages.OCM_CTLCOLOREDIT:
                case (int)ReflectedMessages.OCM_DRAWITEM:
                    {
                        DrawItemStruct dis = (DrawItemStruct)m.GetLParam(typeof(DrawItemStruct));

                        Graphics graph = Graphics.FromHdc(dis.hDC);
                        Rectangle rect = new
                            Rectangle(dis.rcItem.left, dis.rcItem.top, dis.rcItem.right -
                            dis.rcItem.left, dis.rcItem.bottom - dis.rcItem.top);
                        int index = dis.itemID;
                        ListViewItemStates state;
                        if (dis.itemState == 1)
                        {
                            state = ListViewItemStates.Selected;
                        }
                        else
                            if (dis.itemState == 17)
                        {
                            state = ListViewItemStates.Focused;
                        }
                        else
                        {
                            state = ListViewItemStates.Default;
                        }

                        //System.Windows.Forms.DrawItemEventArgs e = new System.Windows.Forms.DrawItemEventArgs(graph, Font, rect, index, state, this.Items[index].ForeColor, this.Items[index].BackColor);
                        DrawListViewItemEventArgs e = new DrawListViewItemEventArgs(graph, this.Items[index], rect, index, state);
                        try
                        {
                            this.MyOnDrawItem(e);
                        }
                        catch { }
                        graph.Dispose();
                        break;
                    }

                case (int)ReflectedMessages.OCM_MESUREITEM:
                    //base.WndProc(ref m);
                    this.WmReflectMeasureItem(ref m);
                    break;
                default:
                    //Console.WriteLine(m.Msg);
                    break;

            }
        }

        private void WmReflectMeasureItem(ref Message m)
        {
            Graphics graph;
            MeasureItemEventArgs e;
            MEASUREITEMSTRUCT mis = (MEASUREITEMSTRUCT)
                m.GetLParam(typeof(MEASUREITEMSTRUCT));
            //if ((this.drawMode == DrawMode.OwnerDrawVariable) &&
            //    (mis.itemID >= 0))
            {
                graph = Graphics.FromHwnd(this.Handle);
                e = new MeasureItemEventArgs(graph,
                    mis.itemID, 20);
                try
                {
                    this.OnMeasureItem(e);
                    mis.itemHeight = e.ItemHeight;
                    mis.itemWidth = e.ItemWidth;
                }
                finally
                {
                    graph.Dispose();
                }
            }

            Marshal.StructureToPtr(mis, m.LParam, false);
            m.Result = ((IntPtr)1);
        }

        //public new event DrawItemEventHandler DrawItem;
        public new event LabelEditEventHandler AfterLabelEdit;
        public event MeasureItemEventHandler MeasureItem;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= LVS_OWNERDRAWFIXED;
                return cp;
            }
        }
        protected virtual void MyOnDrawItem(System.Windows.Forms.DrawListViewItemEventArgs e)
        {
            Graphics graphics = bufferedGraphics.Graphics;
            this.SuspendLayout();
            //this.BeginUpdate();
            ListViewItem listViewItem = this.Items[e.Item.Index];
            // Select the appropriate brush depending on if the item is selected.
            graphics.FillRectangle(new SolidBrush(listViewItem.BackColor), new Rectangle(e.Bounds.X, e.Bounds.Y, this.Bounds.Width, e.Bounds.Height));

            if (listViewItem.Selected)//.(State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
            }
            else
            {
                graphics.FillRectangle(new SolidBrush(e.Item.BackColor), e.Bounds);
            }
            if (DropIndex == e.Item.Index)
            {
                graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds.X, e.Bounds.Y - 1, e.Bounds.Width, 3);
            }
            else if (DropIndex == e.Item.Index + 1)
            {
                graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds.X, e.Bounds.Y + e.Bounds.Height - 1, e.Bounds.Width, 3);
            }
            ImageInfo imageInfo = (ImageInfo)listViewItem.Tag;
            if (imageInfo.ThumbImage != null && this.thumbSize != 0)
                graphics.DrawImage(imageInfo.ThumbImage,
                     new RectangleF(e.Bounds.X + e.Bounds.Height / 2 - imageInfo.ThumbImage.Width / 2,
                         e.Bounds.Y + e.Bounds.Height / 2 - imageInfo.ThumbImage.Height / 2,
                         imageInfo.ThumbImage.Width,
                         imageInfo.ThumbImage.Height));
            else
            {
                Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(imageInfo.FileInfo.FullName);
                using (Bitmap bmp = icon.ToBitmap())
                {
                    graphics.DrawImage(bmp,
                         e.Bounds.X,
                         e.Bounds.Y,
                         e.Bounds.Height,
                         e.Bounds.Height
                         );
                }
            }
            int colWidth = 0;
            int i = 0;

            foreach (ListViewItem.ListViewSubItem subItem in listViewItem.SubItems)
            {
                if (i == THUMBNAIL_INDEX)
                {
                    this.Columns[THUMBNAIL_INDEX].Width = ThumbSize == 0 ? e.Bounds.Height : ThumbSize;
                }
                if (this.Columns[i].Width > 0)
                {
                    if (listViewItem.Selected && subItem.ForeColor == SystemColors.WindowText)
                        subItem.ForeColor = SystemColors.HighlightText;
                    if (!listViewItem.Selected && subItem.ForeColor == SystemColors.HighlightText)
                        subItem.ForeColor = SystemColors.WindowText;
                    String text = subItem.Text;
                    while ((int)e.Graphics.MeasureString(text, subItem.Font).Width > this.Columns[i].Width && text.Length > 6)
                    {
                        text = text.Substring(0, text.Length - 6) + "...";
                    }
                    graphics.DrawString(text, subItem.Font, new SolidBrush(subItem.ForeColor), 
                        e.Bounds.X + colWidth, (int)(e.Bounds.Y + e.Bounds.Height / 2 - subItem.Font.GetHeight() / 2));
                    colWidth += this.Columns[i].Width;
                }
                i++;
            }
            if (this.Items[e.Item.Index].Focused)
            {
                System.Drawing.Drawing2D.HatchBrush b = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.DottedGrid, SystemColors.Highlight, Color.FromArgb((byte)~SystemColors.Highlight.R, (byte)~SystemColors.Highlight.G, (byte)~SystemColors.Highlight.B));
                graphics.DrawRectangle(new Pen(b), e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            }
            bufferedGraphics.Render(e.Graphics);
            this.ResumeLayout();
        }

        public virtual void OnMeasureItem(MeasureItemEventArgs e)
        {
            if (this.MeasureItem != null)
            {
                this.MeasureItem(this, e);
            }
            else
            {
                e.ItemHeight = Math.Max(this.thumbSize, 16);
                e.ItemWidth = Math.Max(this.thumbSize, 16);
            }
        }
        #endregion

        #region FillListViewItem
        public void RefreshListViewItems()
        {
            if (this.ProgressBar != null)
            {
                this.ProgressBar.Value = 0;
                this.ProgressBar.Maximum = Items.Count;
            }
            this.SuspendLayout();
            foreach (ListViewItem item in Items)
            {
                RefreshListViewItem(item);
                if (this.ProgressBar != null)
                {
                    this.ProgressBar.Value++;
                    this.ProgressBar.Update();
                }
            }
            this.ResumeLayout();
            this.ProgressBar.Value = 0;
        }

        private ListViewItem FillListViewItem(FileInfo fileInfo)
        {
            return FillListViewItem(null, new ImageInfo(fileInfo)); ;
        }

        public ListViewItem RefreshListViewItem(ListViewItem listViewItem)
        {
            return FillListViewItem(listViewItem, (ImageInfo)listViewItem.Tag);

        }

        private ListViewItem FillListViewItem(ListViewItem listViewItem, ImageInfo imageInfo)
        {
            if (listViewItem == null)
                listViewItem = new ListViewItem(new String[] { null, null, null, null, null, null });
            Cursor.Current = Cursors.WaitCursor;

            imageInfo.CreateThumbnailImage(this.thumbSize, MetaDataRequired);
            listViewItem.SubItems[NAME_INDEX].Text = imageInfo.FileInfo.Name;
            listViewItem.SubItems[NEW_NAME_INDEX].Text = imageInfo.NewFilename;
            listViewItem.SubItems[FILESIZE_INDEX].Text = Utils.FormatFileSize(imageInfo.FileInfo.Length);
            listViewItem.SubItems[WRITEDATE_INDEX].Text = imageInfo.NewWriteDate.ToString();
            if (imageInfo.NewExifDate.ToString() != DateTime.MinValue.ToString())
                listViewItem.SubItems[EXIFDATE_INDEX].Text = imageInfo.NewExifDate.ToString();
            else
                listViewItem.SubItems[EXIFDATE_INDEX].Text = "";
            if (imageInfo.FileInfo.Name != imageInfo.NewFilename)
                listViewItem.SubItems[NEW_NAME_INDEX].ForeColor = Color.Orange;
            else
                listViewItem.SubItems[NEW_NAME_INDEX].ForeColor = SystemColors.WindowText;
            if (imageInfo.NewFilenameLocked)
                listViewItem.SubItems[NEW_NAME_INDEX].ForeColor = Color.LightGreen;


            if (imageInfo.FileInfo.LastWriteTime.ToString() != imageInfo.NewWriteDate.ToString())
                listViewItem.SubItems[WRITEDATE_INDEX].ForeColor = Color.Orange;
            else
                listViewItem.SubItems[WRITEDATE_INDEX].ForeColor = SystemColors.WindowText;
            if (imageInfo.NewWriteDateLocked)
                listViewItem.SubItems[WRITEDATE_INDEX].ForeColor = Color.LightGreen;

            if (imageInfo.ExifDate.ToString() != imageInfo.NewExifDate.ToString())
                listViewItem.SubItems[EXIFDATE_INDEX].ForeColor = Color.Orange;
            else
                listViewItem.SubItems[EXIFDATE_INDEX].ForeColor = SystemColors.WindowText;
            if (imageInfo.NewExifDateLocked)
                listViewItem.SubItems[EXIFDATE_INDEX].ForeColor = Color.LightGreen;

            listViewItem.Tag = imageInfo;
            Cursor.Current = Cursors.Default;
            return listViewItem;
        }
        #endregion

        #region Add/Insert Item
        public ListViewItem AddItem(FileInfo fileInfo)
        {
            return this.InsertItem(fileInfo, this.Items.Count);
        }

        public ListViewItem InsertItem(FileInfo fileInfo, int Index)
        {
            ListViewItem listViewItem = FillListViewItem(fileInfo);
            if (listViewItem != null)
                return this.Items.Insert(Index, listViewItem);
            return null;
        }

        public ListViewItem ReplaceItem(FileInfo fileInfo, int Index)
        {
            ListViewItem listViewItem = FillListViewItem(fileInfo);
            if (listViewItem != null)
                this.Items[Index] = listViewItem;
            return this.Items[Index];
        }

        public void AddItems(String[] files)
        {
            this.InsertItems(files, this.Items.Count);
        }

        public void InsertItems(String[] files, int Index)
        {
            files = files.OrderBy(f => f).ToArray();

            if (this.ProgressBar != null)
            {
                this.ProgressBar.Value = 0;
                this.ProgressBar.Maximum = files.Length;
            }
            this.BeginUpdate();
            foreach (String file in files)
            {
                if (this.ProgressBar != null)
                {
                    this.ProgressBar.Value++;
                }
                FileInfo fileInfo = new FileInfo(file);
                ListViewItem listViewItem = this.InsertItem(fileInfo, Index);
                if (listViewItem != null)
                {
                    listViewItem.Selected = true;
                    listViewItem.Focused = true;
                    this.EnsureVisible(listViewItem.Index);
                    Index++;
                }
                this.EndUpdate();
            }
            if (this.ProgressBar != null)
                this.ProgressBar.Value = 0;
            if (this.Items.Count > 0)
            {
                Graphics graphics = Graphics.FromHwnd(this.Handle);

                chNewFilename.Width = (int)graphics.MeasureString(
                    this.Items.Cast<ListViewItem>().OrderByDescending(i => i.SubItems[NEW_NAME_INDEX].Text.Length).First().SubItems[NEW_NAME_INDEX].Text,
                    this.Font).Width + 20;
                graphics.Dispose();
            }
            this.Refresh();
        }
        #endregion

        #region Mouse Move and Click
        public class ListItemClickEventArgs : EventArgs
        {
            public ListItemClickEventArgs(ListViewItem item, ListViewItem.ListViewSubItem subItem, int subItemIndex)
            {
                Item = item;
                SubItem = subItem;
                SubItemIndex = subItemIndex;
            }

            public ListViewItem Item { get; }
            public ListViewItem.ListViewSubItem SubItem { get; }
            public int SubItemIndex { get; }
            public bool Handled { get; set; }
        }

        public delegate void ListItemClickEventHandler(object sender, ListItemClickEventArgs e);
        public event ListItemClickEventHandler ItemClick;

        public delegate void ListItemDoubleClickEventHandler(object sender, ListItemClickEventArgs e);
        public event ListItemClickEventHandler ItemDoubleClick;

        private int _latestMouseX = 0;
        private void MyListView_MouseDown(object sender, MouseEventArgs e)
        {
            _latestMouseX = e.X;
        }

        private ListViewItem.ListViewSubItem GetListViewSubItemFromCoordinates(int X)
        {
            int accumulated = 0;
            int currentColWidth = 0;
            ListViewItem.ListViewSubItem selectedSubItem = null;
            for (int i = 0; i < this.Columns.Count; i++)
            {
                currentColWidth = this.Columns[i].Width;
                if (accumulated < X && X < accumulated + currentColWidth)
                {
                    selectedSubItem = this.SelectedItems[0].SubItems[i];
                    break;
                }
                accumulated += currentColWidth;
            }
            return selectedSubItem;
        }

        private void MyListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.SelectedItems.Count > 0)
            {
                SelectedItem = this.SelectedItems[0];
                SelectedSubItem = GetListViewSubItemFromCoordinates(_latestMouseX);
                if (SelectedSubItem == null)
                    SelectedSubItemIndex = -1;
                else
                    SelectedSubItemIndex = SelectedItem.SubItems.IndexOf(SelectedSubItem);
                if (ItemClick != null)
                {
                    var args = new ListItemClickEventArgs(SelectedItem, SelectedSubItem, SelectedSubItemIndex);
                    ItemClick(this, args);
                }
            }
        }

        private void MyListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ItemDoubleClick != null)
            {
                var args = new ListItemClickEventArgs(SelectedItem, SelectedSubItem, SelectedSubItemIndex);
                ItemDoubleClick(this, args);
                if (!args.Handled)
                    EditSelectedItems();
            }
            else
                EditSelectedItems();
        }

        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            if (this.manualReorder && MessageBox.Show(
                "There is an existing manual reorder.\nDo you really want to reset it through '" + this.Columns[e.Column].Text + "' column sorting ?",
                "Confirm reorder ?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;
            this.manualReorder = false;
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvSorter.Order == SortOrder.Ascending)
                {
                    lvSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvSorter.SortColumn = e.Column;
                lvSorter.Order = SortOrder.Ascending;
            }
            this.Sort();
        }
        #endregion

        #region EditItems
        private void EditSelectedItems()
        {
            if (SelectedSubItemIndex == FILESIZE_INDEX) return;
            if (SelectedSubItemIndex == NAME_INDEX || SelectedSubItemIndex == THUMBNAIL_INDEX)
            {
                ImageInfo imageInfo = (ImageInfo)this.SelectedItems[0].Tag;
                Process.Start(imageInfo.FileInfo.Directory + "\\" + imageInfo.FileInfo.Name);
                return;
            }

            string prevValue = "";
            foreach (ListViewItem item in this.SelectedItems)
            {
                ImageInfo imageInfo = (ImageInfo)item.Tag;
                string value = item.SubItems[SelectedSubItemIndex].Text;
                string nextValue = "";
                if (item.Index > 0)
                    prevValue = this.Items[item.Index - 1].SubItems[SelectedSubItemIndex].Text;
                if (item.Index < this.Items.Count - 1)
                    nextValue = this.Items[item.Index + 1].SubItems[SelectedSubItemIndex].Text;
                string originalValue = "";

                switch (SelectedSubItemIndex)
                {
                    case NEW_NAME_INDEX: originalValue = imageInfo.FileInfo.Name; break;
                    case WRITEDATE_INDEX: originalValue = imageInfo.FileInfo.LastWriteTime.ToString(); break;
                    case EXIFDATE_INDEX: originalValue = imageInfo.ExifDate.ToString(); break;
                }
                FrmInputDialog frmFilenameInput = new FrmInputDialog(
                    this.SelectedItems.Count > 1,
                    prevValue,
                    value, originalValue,
                    nextValue,
                    (SelectedSubItemIndex == WRITEDATE_INDEX || SelectedSubItemIndex == EXIFDATE_INDEX)
                    ? FrmInputDialog.InputScopeEnum.Date : FrmInputDialog.InputScopeEnum.Filename
                    );
                frmFilenameInput.Text = "Edit " + this.Columns[SelectedSubItemIndex].Text;
                DialogResult res = frmFilenameInput.ShowDialog();
                if (res == DialogResult.OK)
                {
                    if (value != frmFilenameInput.Value)
                    {
                        ConvertValueForImageInfo(imageInfo, frmFilenameInput.Value);
                        if (this.AfterLabelEdit != null)
                            AfterLabelEdit(this, new LabelEditEventArgs(item.Index));
                    }
                }
                else if (res == DialogResult.Abort)
                {
                    break;
                }
            }
        }
        private void ConvertValueForImageInfo(ImageInfo imageInfo, string newValue)
        {
            switch (SelectedSubItemIndex)
            {
                case NEW_NAME_INDEX:
                    imageInfo.NewFilenameLocked = false;
                    imageInfo.NewFilename = newValue;
                    imageInfo.NewFilenameLocked = imageInfo.NewFilename != imageInfo.FileInfo.Name;
                    break;
                case WRITEDATE_INDEX:
                    imageInfo.NewWriteDateLocked = false;
                    imageInfo.NewWriteDate = DateTime.Parse(newValue);
                    imageInfo.NewWriteDateLocked = imageInfo.NewWriteDate.ToString() != imageInfo.FileInfo.LastWriteTime.ToString();
                    break;
                case EXIFDATE_INDEX:
                    imageInfo.NewExifDateLocked = false;
                    imageInfo.NewExifDate = DateTime.Parse(newValue);
                    imageInfo.NewExifDateLocked = imageInfo.NewExifDate.ToString() != imageInfo.ExifDate.ToString();
                    break;
                default:
                    break;
            }
        }

        public void SetNewFilename(ListViewItem item, string newFilename)
        {
            ((ImageInfo)item.Tag).NewFilename = newFilename;
            RefreshListViewItem(item);
        }

        public void ApplyChanges(ListViewItem item)
        {
            ApplyNewFilename(item);
            ApplyNewWriteDate(item);
            ApplyNewExifDate(item);
            RefreshListViewItem(item);
        }

        public void ApplyChange(ListViewItem item)
        {
            switch (SelectedSubItemIndex)
            {
                case NEW_NAME_INDEX:
                    ApplyNewFilename(item);
                    break;
                case WRITEDATE_INDEX:
                    ApplyNewWriteDate(item);
                    break;
                case EXIFDATE_INDEX:
                    ApplyNewExifDate(item);
                    break;
                default:
                    break;
            }
        }

        public bool ApplyNewFilename(ListViewItem item)
        {
            ImageInfo imageInfo = (ImageInfo)item.Tag;
            if (imageInfo.ApplyNewFilename())
            {
                RefreshListViewItem(item);
                return true;
            }
            else
                return false;
        }

        private void ApplyNewWriteDate(ListViewItem item)
        {
            ImageInfo imageInfo = (ImageInfo)item.Tag;
            imageInfo.ApplyNewWriteDate();
            RefreshListViewItem(item);
        }

        private void ApplyNewExifDate(ListViewItem item)
        {
            ImageInfo imageInfo = (ImageInfo)item.Tag;
            imageInfo.ApplyNewExifDate();
            RefreshListViewItem(item);
        }
        #endregion

        private void MyListView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Back:
                    if (this.SelectedItems.Count > 0)
                    {
                        foreach (ListViewItem item in this.SelectedItems)
                        {
                            ImageInfo imageInfo = (ImageInfo)item.Tag;
                            imageInfo.Reset();
                            RefreshListViewItem(item);
                        }
                    }
                    break;
                case Keys.Delete:
                    {
                        this.BeginUpdate();
                        if (this.SelectedItems.Count > 0)
                        {
                            int index = -1; ;
                            foreach (ListViewItem item in this.SelectedItems)
                            {
                                index = Math.Max(item.Index, index);
                                item.Remove();
                            }
                            index = Math.Min(index, this.Items.Count - 1);
                            if (index != -1)
                            {
                                this.Items[index].Selected = true;
                                this.Items[index].Focused = true;
                            }
                        }
                        bufferedGraphics.Graphics.Clear(BackColor);
                        this.EndUpdate();
                        if (this.Items.Count == 0)
                        {
                            this.manualReorder = false;
                            this.lvSorter.Order = SortOrder.None;
                        }
                    }
                    break;
                case Keys.F2:
                    {
                        if (this.SelectedItems.Count > 0)
                        {
                            EditSelectedItems();
                        }
                        break;
                    }
                case Keys.L:
                    this.BeginUpdate();
                    foreach (ListViewItem item in this.SelectedItems)
                    {
                        ImageInfo imageInfo = (ImageInfo)item.Tag;
                        imageInfo.NewFilenameLocked = imageInfo.NewWriteDateLocked = imageInfo.NewWriteDateLocked = !imageInfo.NewFilenameLocked;
                        if (this.AfterLabelEdit != null)
                            AfterLabelEdit(this, new LabelEditEventArgs(item.Index));
                        RefreshListViewItem(item);
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    this.EndUpdate();
                    break;
                case Keys.A:
                    if (e.Control)
                    {
                        this.BeginUpdate();
                        foreach (ListViewItem item in this.Items)
                            item.Selected = true;
                        this.EndUpdate();
                    }
                    break;
            }
        }

        public void Redraw()
        {
            this.SuspendLayout();
            this.Height = this.Height + 1;
            this.Height = this.Height - 1;
            base.Refresh();
            this.ResumeLayout();
        }

        public void LoadFolder(string path)
        {
            if (Directory.Exists(path))
            {
                this.Items.Clear();
                this.Refresh();
                String[] files = Directory.GetFiles(path, "*.*");

                this.AddItems(files);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (bufferedGraphics != null)
                bufferedGraphics.Dispose();
            bufferedGraphics = BufferedGraphicsManager.Current.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Bounds.Width, this.Bounds.Height));
            bufferedGraphics.Graphics.Clear(BackColor);
            base.OnSizeChanged(e);
        }

        public void ForEachItems(Action<ListViewItem, ImageInfo> func, bool showProgress = true, bool selectedOnly = false)
        {
            if (selectedOnly || SelectedItems.Count > 0)
            {
                if (showProgress && ProgressBar != null)
                {
                    ProgressBar.Value = 0;
                    ProgressBar.Maximum = this.SelectedItems.Count;
                }
                foreach (ListViewItem item in this.SelectedItems)
                {

                    func(item, (ImageInfo)item.Tag);
                    if (showProgress && ProgressBar != null)
                    {
                        ProgressBar.Value++;
                        ProgressBar.Update();
                    }
                }
            }
            else
            {
                if (showProgress && ProgressBar != null)
                {
                    ProgressBar.Value = 0;
                    ProgressBar.Maximum = this.Items.Count;
                }
                foreach (ListViewItem item in this.Items)
                {
                    func(item, (ImageInfo)item.Tag);
                    if (showProgress && ProgressBar != null)
                    {
                        ProgressBar.Value++;
                        ProgressBar.Update();
                    }
                }
            }
            if (showProgress && ProgressBar != null)
            {
                ProgressBar.Value = 0;
                ProgressBar.Update();
            }
        }
    }
}
