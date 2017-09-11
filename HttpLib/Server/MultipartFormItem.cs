using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HttpLib.Server
{
    public class MultipartFormItem
    {
        public string Name { get;  set; }
        public string FileName { get;  set; }
        public byte[] Data { get;  set; }
        public string ContentType { get;  set; }
        public FormItemType ItemType { get;  set; }

        public override string ToString()
        {
            if(ItemType == FormItemType.File)
            {
                return Name + "=file[" + FileName+"]["+Data.Length+"]";
            }
            else
            {
                return Name + "=" + this.GetDataAsString();
            }
        }
    }

    public enum FormItemType
    {
        Text,
        File
    }

    public static class MultipartFormItemExtends
    {
        public static string SaveAsFile( this MultipartFormItem item, string fileDir, string fileName = null, bool cover = true )
        {
            if (item.ItemType != FormItemType.File) throw new NotSupportedException("ItemType must be FormItemType.File");
            if (!Directory.Exists(fileDir)) Directory.CreateDirectory(fileDir);
            if (fileName == null) fileName = item.FileName;
            if (fileName == null) fileName = DateTime.Now.Ticks.ToString();
            string filePath = fileDir.EndsWith("/") || fileDir.EndsWith(@"\") ? fileDir + fileName : fileDir + @"\" + fileName;

            if (File.Exists(filePath))
            {
                if (cover) File.Delete(filePath);
                else return null;
            }

            try
            {
                using (FileStream stream = File.Create(filePath, item.Data.Length, FileOptions.WriteThrough))
                {
                    stream.Write(item.Data, 0, item.Data.Length);
                    return filePath;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
		
		public static string GetDataAsString(this MultipartFormItem item,Encoding encoding = null )
        {
            if (encoding == null) encoding = Encoding.UTF8;
            return encoding.GetString(item.Data);
        }
		
    }
}