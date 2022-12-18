using System;
using System.Collections;
using System.Windows.Forms;
using System.IO;

namespace ImageRenamer
{
	/// <summary>
	/// Description résumée de Utils.
	/// </summary>
	public class Utils
	{
		public Utils()
		{
			//
			// TODO : ajoutez ici la logique du constructeur
			//
		}
		public static string FormatFileSize(long fileSize)
		{
			long NumberOfBytes = fileSize;
			if(NumberOfBytes >=1073741824)
			{
				return string.Format("{0:#0.00}",NumberOfBytes/1024, 1024, 1024 + " MB");
			}
			else if(NumberOfBytes >=1048576)
			{
				return string.Format("{0:#0.00}", NumberOfBytes/1024/1024 + " MB");
			}
			else if(NumberOfBytes >=1024)
			{
				return string.Format("{0:#0.00}", NumberOfBytes/1024 + " KB"); 
			}
			else if(NumberOfBytes > 0 && NumberOfBytes < 1024)
			{
				return string.Format("{0:#0.00}", NumberOfBytes + " Bytes"); 
			}
			else
			{
				return "0 Byte";
			}
		}

		public static bool Rename(FileInfo fileInfo, String NewFilename)
		{
			try
			{
				File.Move(fileInfo.Directory + "\\" + fileInfo.Name, fileInfo.Directory + "\\" + NewFilename);
				return true;
			}
			catch(Exception except)
			{
				MessageBox.Show(except.Message);
				return false;
			}
		}

		public static string DeclineFilename(String Filename, String Path, ArrayList ExcludedFilenames)
		{
			FileInfo fileInfo = new FileInfo(Filename);
			int counter = 1;
			Filename = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_" + counter.ToString() + fileInfo.Extension;
			while (File.Exists(Path + "\\" + Filename) || ExcludedFilenames.Contains(Filename.ToLower()))
			{
				counter++;
				Filename = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_" + counter.ToString() + fileInfo.Extension;
			}
			return Filename;
		}
	}
}
