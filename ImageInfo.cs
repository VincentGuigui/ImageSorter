using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using System.Reflection;

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
        public int ThumbSize = 0;
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

        public void CreateThumbnailImage(int ThumbSize, bool MetaDataRequired)
        {
            ExifDate = DateTime.MinValue;
            Image img = null;
            Image thumbImage = null;
            if ((ThumbSize > 0 && (this.ThumbImage == null || this.ThumbSize != ThumbSize))
                || (MetaDataRequired && this.MetaData == null))
            {
                if (IMAGE_EXTS.Contains(FileInfo.Extension.ToUpper()))
                {
                    try
                    {
                        img = Image.FromFile(this.FileInfo.Directory + "\\" + this.FileInfo.Name);
                        MetaData = img.PropertyItems;
                        if (ThumbSize > 0)
                        {
                            if (img.Width > img.Height)
                            {
                                thumbImage = img.GetThumbnailImage(ThumbSize, (int)((float)img.Height / ((float)img.Width / (float)ThumbSize)), null, IntPtr.Zero);
                            }
                            else
                            {
                                thumbImage = img.GetThumbnailImage((int)((float)img.Width / ((float)img.Height / (float)ThumbSize)), ThumbSize, null, IntPtr.Zero);
                            }
                        }
                        this.ThumbImage = thumbImage;
                        this.ThumbSize = ThumbSize;
                    }
                    catch
                    {
                    }
                }
            }
            if (MetaDataRequired && MetaData != null)
            {
                ExifDate = GetExifDate();
                if (NewExifDate == DateTime.MinValue)
                    NewExifDate = ExifDate;
            }
            if (img != null)
                img.Dispose();
            img = null;
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

        public bool ApplyNewFilename()
        {
            NewFilenameLocked = false;
            if (NewFilename.ToLower() == FileInfo.Name.ToLower()) return false;
            if (File.Exists(FileInfo.Directory + "\\" + NewFilename))
            {
                if (MessageBox.Show("File already exists. Do you want to add a counter ?", "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    NewFilename = Utils.DeclineFilename(NewFilename, FileInfo.DirectoryName, new ArrayList());
                }
                else
                {
                    return false;
                }
            }
            if (Utils.Rename(FileInfo, NewFilename))
            {
                NewFilenameLocked = false;
                FileInfo = new FileInfo(FileInfo.Directory + "\\" + NewFilename); ;
                return true;
            }
            else
                return false;
        }
    }
}
