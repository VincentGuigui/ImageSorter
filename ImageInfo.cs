using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace ImageRenamer
{
    /// <summary>
    /// Description résumée de ImageInfo.
    /// </summary>
    public class ImageInfo
    {
        private static string[] IMAGE_EXTS = new string[] { ".JPG", ".JPEG", ".GIF", ".PNG" };
        private static string[] VIDEO_EXTS = new string[] { ".MP4", ".MOV" };
        public FileInfo FileInfo = null;
        public DateTime ExifDate;
        private String newFilename = "";
        public bool NewFilenameLocked = false;
        private DateTime newWriteDate;
        public bool NewWriteDateLocked = false;
        private DateTime newExifDate;
        public bool NewExifDateLocked = false;

        public string NewFilename
        {
            get { return newFilename; }
            set
            {
                if (!NewFilenameLocked)
                    newFilename = value;
            }
        }
        public DateTime NewWriteDate
        {
            get { return newWriteDate; }
            set
            {
                if (!NewWriteDateLocked)
                    newWriteDate = value;
            }
        }
        public DateTime NewExifDate
        {
            get { return newExifDate; }
            set
            {
                if (!NewExifDateLocked)
                    newExifDate = value;
            }
        }
        public Image ThumbImage = null;
        private int currentThumbSize = 32;
        public System.Drawing.Imaging.PropertyItem[] MetaData = null;
        public ImageInfo()
        {
        }

        public ImageInfo(FileInfo FileInfo)
        {
            this.FileInfo = FileInfo;
            this.NewFilename = FileInfo.Name;
            this.NewWriteDate = FileInfo.LastWriteTime;
        }

        public void Reset()
        {
            NewFilenameLocked = false;
            NewWriteDateLocked = false;
            NewExifDateLocked = false;
            this.NewFilename = FileInfo.Name;
            this.NewWriteDate = FileInfo.LastWriteTime;
            this.NewExifDate = ExifDate;
        }


        public static Image CreateThumbnailImage(FileInfo fileInfo, Size thumbSize, bool fitInside)
        {
            Image img;
            Image thumbImage = CreateThumbnailImage(fileInfo, thumbSize, fitInside, out img);
            if (img != null)
                img.Dispose();
            return thumbImage;
        }

        public static Image CreateThumbnailImage(FileInfo fileInfo, Size thumbSize, bool fitInside, out Image img)
        {
            if (!IMAGE_EXTS.Contains(fileInfo.Extension.ToUpper()))
            {
                img = null;
                return System.Drawing.Icon.ExtractAssociatedIcon(fileInfo.FullName).ToBitmap();
            }
            else
            {
                try
                {
                    img = Image.FromFile(fileInfo.FullName);
                    Size newSize;

                    Image tmpImage = null;
                    if (thumbSize.Width <= 16)
                    {
                        newSize = thumbSize;
                        tmpImage = System.Drawing.Icon.ExtractAssociatedIcon(fileInfo.FullName).ToBitmap();
                    }
                    else
                    {
                        if (thumbSize.Width < 240)
                            tmpImage = img.GetThumbnailImage(thumbSize.Width, thumbSize.Height, null, IntPtr.Zero);
                        else
                            tmpImage = img;
                        newSize = (img.Width > img.Height)
                        ? new Size(thumbSize.Width,
                            (int)((float)img.Height / ((float)img.Width / (float)thumbSize.Width)))
                        : new Size((int)((float)img.Width / ((float)img.Height / (float)thumbSize.Height)),
                            thumbSize.Height);
                    }
                    Image thumbnailImage = null;
                    if (fitInside)
                        thumbnailImage = new Bitmap(thumbSize.Width, thumbSize.Height);
                    else
                        thumbnailImage = new Bitmap(newSize.Width, newSize.Height);

                    using (Graphics graphics = Graphics.FromImage(thumbnailImage))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.DrawImage(tmpImage,
                            (thumbnailImage.Width - newSize.Width) / 2,
                            (thumbnailImage.Height - newSize.Height) / 2,
                            newSize.Width, newSize.Height);
                    }
                    return thumbnailImage;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            img = null;
            return null;
        }

        public void LoadThumbnailAndMetadata(int thumbSize, bool metaDataRequired)
        {
            ExifDate = DateTime.MinValue;
            Image img = null;
            if (this.ThumbImage == null || this.currentThumbSize != thumbSize)
            {
                this.ThumbImage = CreateThumbnailImage(this.FileInfo, new Size(thumbSize, thumbSize), true, out img);
                this.currentThumbSize = thumbSize;
            }
            if (metaDataRequired && IMAGE_EXTS.Contains(FileInfo.Extension.ToUpper()))
            {
                if (MetaData == null)
                {
                    if (img == null)
                        img = Image.FromFile(this.FileInfo.FullName);
                    MetaData = img.PropertyItems;
                    ExifDate = GetExifDate();
                    if (NewExifDate == DateTime.MinValue)
                        NewExifDate = ExifDate;
                }
            }
            if (img != null)
                img.Dispose();
        }

        public bool HasMissingExifDate()
        {
            return
                IMAGE_EXTS.Contains(FileInfo.Extension.ToUpper())
                && ExifDate == DateTime.MinValue;
        }

        private DateTime GetExifDate()
        {
            if (ExifDate == DateTime.MinValue)
            {
                if (MetaData == null) return ExifDate;
                foreach (PropertyItem propItem in MetaData)
                {
                    EXIF.EXIFPropertyItem exifPropItem = new EXIF.EXIFPropertyItem(propItem);
                    if (exifPropItem.EXIFCode == EXIF.KnownEXIFIDCodes.DateTimeOriginal)
                    {
                        ExifDate = exifPropItem.ParsedDate;
                        return ExifDate;
                    }
                }
                ExifDate = DateTime.MinValue;
            }
            return ExifDate;
        }

        public void ApplyNewWriteDate()
        {
            if (FileInfo.LastWriteTime == NewWriteDate) return;
            FileInfo.LastWriteTime = NewWriteDate;
            FileInfo.CreationTime = NewWriteDate;
            NewWriteDateLocked = false;
        }

        public void ApplyNewExifDate()
        {
            if (NewExifDate == ExifDate) return;
            if (NewExifDate == DateTime.MinValue) return;
            if (MetaData == null) return;
            Image img = Image.FromFile(this.FileInfo.FullName);

            var propertyItem = img.PropertyItems[0];
            propertyItem.Id = (int)EXIF.KnownEXIFIDCodes.DateTimeOriginal;
            propertyItem.Len = 20;
            propertyItem.Type = 2;
            propertyItem.Value = Encoding.UTF8.GetBytes(NewExifDate.ToString("yyyy:MM:dd HH:mm:ss") + '\0');

            img.SetPropertyItem(propertyItem);
            try
            {
                img.Save(this.FileInfo.FullName + ".tmp");
                MetaData = img.PropertyItems;
                img.Dispose();
                File.Copy(this.FileInfo.FullName + ".tmp", this.FileInfo.FullName, true);
                File.Delete(this.FileInfo.FullName + ".tmp");
                this.FileInfo.LastWriteTime = this.FileInfo.LastWriteTime;
                ExifDate = NewExifDate;
                NewExifDateLocked = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inBatch"></param>
        /// <param name="excludedFilenames"></param>
        /// <param name="batchOKOrIgnoreForAll"></param>
        /// <returns>
        /// Yes to confirm uniqueness for this one only
        /// OK if the user has confirmed uniqueness by adding a counter for this one and the following
        /// Ignore if the user wanted to prevent adding a uniqueness counter for this one and the following
        /// Cancel if the user cancelled the process</returns>
        public DialogResult EnsureUniqueFilename(bool inBatch, List<string> excludedFilenames = null, DialogResult OKOrIgnoreForAll = DialogResult.None)
        {
            DialogResult result = OKOrIgnoreForAll;
            if (NewFilename.ToLower() == FileInfo.Name.ToLower()) return result;
            string newFullname = Path.Combine(FileInfo.Directory.FullName, NewFilename);
            if (File.Exists(newFullname) || (excludedFilenames != null && excludedFilenames.Contains(newFilename.ToLower())))
            {
                if (result == DialogResult.None)
                {
                    result = MessageBox.Show("File \"" + NewFilename + "\" already exists.\nDo you want to add a counter at the end of tbe name ?", "File exists",
                         inBatch ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes && inBatch)
                    {
                        DialogResult resultForAll = MessageBox.Show("Do you want a counter for all the following items ?", "Apply to all ?",
                            MessageBoxButtons.YesNo);
                        if (resultForAll == DialogResult.Yes) result = DialogResult.OK;
                    }
                    else if (result == DialogResult.No && inBatch)
                    {
                        DialogResult resultForAll = MessageBox.Show("Do you want to ignore possible reused name for all the following items ?", "Apply to all ?", MessageBoxButtons.YesNo);
                        if (resultForAll == DialogResult.Yes) result = DialogResult.Ignore;
                    }
                }
                if (result == DialogResult.Yes || result == DialogResult.OK)
                {
                    NewFilename = Utils.DeclineFilename(newFullname, excludedFilenames).Name;
                }
            }
            return result;
        }

        public bool ApplyNewFilename()
        {
            if (Utils.Rename(FileInfo, NewFilename))
            {
                NewFilenameLocked = false;
                FileInfo = new FileInfo(Path.Combine(FileInfo.Directory.FullName, NewFilename));
                return true;
            }
            else
                return false;
        }
    }
}
