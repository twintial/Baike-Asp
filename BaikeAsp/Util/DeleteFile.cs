using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Util
{
    public class DeleteFile
    {
        //public static bool deleteFile(FileSystemInfo file)
        //{
        //    if (!file.Exists)
        //    {
        //        return false;
        //    }
        //    if ((file.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
        //    {
        //        DirectoryInfo file_x = (DirectoryInfo)file;
        //        FileSystemInfo[] files = file_x.GetFileSystemInfos();
        //        foreach (FileSystemInfo i in files)
        //        {
        //            deleteFile(i);
        //        }
        //    }
        //    try
        //    {
        //        file.Delete();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
    }
}
