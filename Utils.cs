using System;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ImageRenamer
{
    /// <summary>
    /// Description résumée de Utils.
    /// </summary>
    public class Utils
    {
        public static string FormatFileSize(long fileSize)
        {
            long NumberOfBytes = fileSize;
            if (NumberOfBytes >= 1073741824)
            {
                return string.Format("{0:#0.00}", NumberOfBytes / 1024, 1024, 1024 + " MB");
            }
            else if (NumberOfBytes >= 1048576)
            {
                return string.Format("{0:#0.00}", NumberOfBytes / 1024 / 1024 + " MB");
            }
            else if (NumberOfBytes >= 1024)
            {
                return string.Format("{0:#0.00}", NumberOfBytes / 1024 + " KB");
            }
            else if (NumberOfBytes > 0 && NumberOfBytes < 1024)
            {
                return string.Format("{0:#0.00}", NumberOfBytes + " Bytes");
            }
            else
            {
                return "0 Byte";
            }
        }

        public static bool Rename(FileInfo fileInfo, String newFilename)
        {
            try
            {
                File.Move(
                    Path.Combine(fileInfo.Directory.FullName, fileInfo.Name),
                    Path.Combine(fileInfo.Directory.FullName, newFilename));
                return true;
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
                return false;
            }
        }

        public static FileInfo DeclineFilename(string filename, List<string> excludedFilenames = null)
        {
            FileInfo fileInfo = new FileInfo(filename);
            int counter = 1;
            string newFilename;
            do
            {
                newFilename = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_" + counter.ToString() + fileInfo.Extension;
                counter++;
            } while ((File.Exists(newFilename) || (excludedFilenames != null && excludedFilenames.Contains(newFilename.ToLower()))));
            fileInfo = new FileInfo(Path.Combine(fileInfo.Directory.FullName, newFilename));
            return fileInfo;
        }
    }
}
