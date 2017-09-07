using System;
using System.Diagnostics;
using System.IO;

namespace HttpLib
{
    public class MultipartFormItem
    {
        public string Name { get; internal set; }
        public string FileName { get; internal set; }
        public byte[] Data { get; internal set; }
        public string ContentType { get; internal set; }
        public FormItemType ItemType { get; internal set; }
    }

    public enum FormItemType
    {
        Text,
        File
    }

    public static class MultipartFormItemExtends
    {

        public static bool SaveAsFile( this MultipartFormItem item, string fileDir, string fileName = null, bool cover = true )
        {
            if (item.ItemType != FormItemType.File) throw new NotSupportedException("ItemType must be FormItemType.File");
            if (!Directory.Exists(fileDir)) Directory.CreateDirectory(fileDir);
            if (fileName == null) fileName = item.FileName;
            if (fileName == null) fileName = DateTime.Now.Ticks.ToString();
            string filePath = fileDir.EndsWith("/") || fileDir.EndsWith(@"\") ? fileDir + fileName : fileDir + "/" + fileName;

            if (File.Exists(filePath))
            {
                if (cover) File.Delete(filePath);
                else return false;
            }

            try
            {
                using (FileStream stream = File.Create(filePath, item.Data.Length, FileOptions.WriteThrough))
                {
                    stream.Write(item.Data, 0, item.Data.Length);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }
}