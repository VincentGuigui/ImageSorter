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
        private string[] headers = { "Thumbnail", "Filename", "New filename", "Filesize", "Date", "Exif Date" };
        private String path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private ListViewItem previousHoverItem = null;
        public ProgressBar ProgressBar
        {
            get; set;
        }
        public int DropIndex = -1;
        public String Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
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
        private System.Windows.Forms.ColumnHeader chThumb;
        private System.Windows.Forms.ColumnHeader chFilename;
        private System.Windows.Forms.ColumnHeader chNewFilename;
        private System.Windows.Forms.ColumnHeader chSize;
        private System.Windows.Forms.ColumnHeader chDate;
        private System.Windows.Forms.ColumnHeader chExifDate;

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


        public MyListView() : base()
        {
            this.drawMode = DrawMode.Normal;
            this.allowRowReorder = true;
            InitializeComponent();
        }

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
                        //this.Invalidate(new Rectangle(previousHoverItem.Bounds.X, previousHoverItem.Bounds.Y+previousHoverItem.Bounds.Height-1, previousHoverItem.Bounds.Width, 3));
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
                    //this.Invalidate(new Rectangle(dragToItem.Bounds.X, dragToItem.Bounds.Y+dragToItem.Bounds.Height-1, dragToItem.Bounds.Width, 3));
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
                //dragToItem.BackColor = SystemColors.Window;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
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
                    }
                    else if (base.Items.Count - base.SelectedItems.Count >= 0)
                    {
                        DropIndex = base.Items.Count;
                    }
                    this.InsertItems(files, DropIndex);
                    //dragToItem.BackColor = SystemColors.Window;
                }
            }
            if (dragToItem != null)
            {
                this.Invalidate(new Rectangle(dragToItem.Bounds.X, dragToItem.Bounds.Y - 1, dragToItem.Bounds.Width, 3));
                //this.Invalidate(new Rectangle(dragToItem.Bounds.X, dragToItem.Bounds.Y+dragToItem.Bounds.Height-1, dragToItem.Bounds.Width, 3));
            }
            else if (this.Items.Count > 0)
                this.Invalidate(new Rectangle(this.Items[this.Items.Count - 1].Bounds.X, this.Items[this.Items.Count - 1].Bounds.Y + this.Items[this.Items.Count - 1].Bounds.Height - 1, this.Items[this.Items.Count - 1].Bounds.Width, 3));
            else if (this.Items.Count == 0)
                this.Invalidate();
            DropIndex = -1;
            base.OnDragDrop(e);
            this.ResumeLayout();
        }

        #region Code généré par le Concepteur Windows Form
        private void InitializeComponent()
        {
            this.chThumb = new System.Windows.Forms.ColumnHeader();
            this.chFilename = new System.Windows.Forms.ColumnHeader();
            this.chNewFilename = new System.Windows.Forms.ColumnHeader();
            this.chSize = new System.Windows.Forms.ColumnHeader();
            this.chDate = new System.Windows.Forms.ColumnHeader();
            this.chExifDate = new System.Windows.Forms.ColumnHeader();
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
            this.FullRowSelect = true;
            this.HideSelection = false;
            this.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MyListView_KeyDown);
            this.MouseDown += MyListView_MouseDown;
            this.MouseClick += MyListView_MouseClick;
            this.MouseDoubleClick += MyListView_MouseDoubleClick;
        }

        #endregion

        #region Structures
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
        //private SolidBrush foreColorBrush;
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case (int)ReflectedMessages.OCM_CTLCOLOREDIT:
                case (int)ReflectedMessages.OCM_DRAWITEM:
                    {
                        DrawItemStruct dis =
                            (DrawItemStruct)m.GetLParam(typeof(DrawItemStruct));

                        Graphics graph = Graphics.FromHdc(dis.hDC);
                        Rectangle rect = new
                            Rectangle(dis.rcItem.left, dis.rcItem.top, dis.rcItem.right -
                            dis.rcItem.left, dis.rcItem.bottom - dis.rcItem.top);
                        int index = dis.itemID;
                        DrawItemState state;
                        if (dis.itemState == 1)
                        {
                            state = DrawItemState.Selected;
                        }
                        else
                            if (dis.itemState == 17)
                        {
                            state = DrawItemState.Focus;
                        }
                        else
                        {
                            state = DrawItemState.None;
                        }

                        System.Windows.Forms.DrawItemEventArgs e = new
                            System.Windows.Forms.DrawItemEventArgs(graph, Font, rect, index,
                            state, this.Items[index].ForeColor, this.Items[index].BackColor);
                        this.OnDrawItem(e);
                        graph.Dispose();
                        break;
                    }

                case (int)ReflectedMessages.OCM_MESUREITEM:
                    this.WmReflectMeasureItem(ref m);
                    break;
                default:
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

        private DrawMode drawMode;
        public virtual DrawMode DrawMode
        {
            get { return drawMode; }
            set { drawMode = value; }
        }
        public new event DrawItemEventHandler DrawItem;
        public new event LabelEditEventHandler AfterLabelEdit;
        public event MeasureItemEventHandler MeasureItem;
        public void Redraw()
        {
            this.SuspendLayout();
            this.Columns[THUMBNAIL_INDEX].Width = thumbSize;
            this.Height = this.Height + 1;
            this.Height = this.Height - 1;
            base.Refresh();
            this.ResumeLayout();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= (drawMode != DrawMode.Normal) ? LVS_OWNERDRAWFIXED : 0;
                return cp;
            }
        }

        protected virtual void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            if (this.DrawItem != null)
            {
                this.DrawItem(this, e);
            }
            else
            {
                this.SuspendLayout();
                ListViewItem listViewItem = this.Items[e.Index];

                // Select the appropriate brush depending on if the item is selected.
                if (listViewItem.Selected)//.(State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
                }
                if (DropIndex == e.Index)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds.X, e.Bounds.Y - 1, e.Bounds.Width, 3);
                }
                else if (DropIndex == e.Index + 1)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds.X, e.Bounds.Y + e.Bounds.Height - 1, e.Bounds.Width, 3);
                }
                ImageInfo imageInfo = (ImageInfo)listViewItem.Tag;
                if (imageInfo.ThumbImage != null && this.thumbSize != 0)
                    e.Graphics.DrawImage(imageInfo.ThumbImage,
                        new RectangleF(e.Bounds.X + e.Bounds.Height / 2 - imageInfo.ThumbImage.Width / 2,
                            e.Bounds.Y + e.Bounds.Height / 2 - imageInfo.ThumbImage.Height / 2,
                            imageInfo.ThumbImage.Width,
                            imageInfo.ThumbImage.Height));
                int colWidth = 0;
                int i = 0;
                foreach (ListViewItem.ListViewSubItem subItem in listViewItem.SubItems)
                {
                    String text = subItem.Text;
                    while ((int)e.Graphics.MeasureString(text, subItem.Font).Width > this.Columns[i].Width && text.Length > 6)
                    {
                        text = text.Substring(0, text.Length - 6) + "...";
                    }
                    e.Graphics.DrawString(text, subItem.Font, new SolidBrush(subItem.ForeColor), e.Bounds.X + colWidth, e.Bounds.Y + e.Bounds.Height / 2 - subItem.Font.GetHeight() / 2);
                    colWidth += this.Columns[i].Width;
                    i++;
                }
                if (this.Items[e.Index].Focused)
                {
                    System.Drawing.Drawing2D.HatchBrush b = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.DottedGrid, SystemColors.Highlight, Color.FromArgb((byte)~SystemColors.Highlight.R, (byte)~SystemColors.Highlight.G, (byte)~SystemColors.Highlight.B));
                    e.Graphics.DrawRectangle(new Pen(b), e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                }
                this.ResumeLayout();
            }
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

        public void RefreshListViewItems()
        {
            if (this.ProgressBar != null)
            {
                this.ProgressBar.Value = 0;
                this.ProgressBar.Maximum = Items.Count;
            }
            this.BeginUpdate();
            foreach (ListViewItem item in Items)
            {
                RefreshListViewItem(item);
                if (this.ProgressBar != null)
                {
                    this.ProgressBar.Value++;
                }
                this.Update();
            }
            this.EndUpdate();
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
            this.Refresh();
        }

        public void LoadFolder(String Path)
        {
            if (Directory.Exists(Path))
            {
                this.Path = Path;
                this.Items.Clear();
                this.Refresh();
                String[] files = Directory.GetFiles(this.Path, "*.*");

                this.AddItems(files);
            }
        }

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

        private ListViewItem SelectedItem;
        private ListViewItem.ListViewSubItem SelectedSubItem;
        private int SelectedSubItemIndex;

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
                        this.EndUpdate();
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
    }
}
