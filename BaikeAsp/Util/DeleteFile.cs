using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Util
{
    public class DeleteFile
    {
        public static bool deleteFile(FileSystemInfo file)
        {
            if (!file.Exists)
            {
                return false;
            }
            if (file is DirectoryInfo file_x)
            {
                FileSystemInfo[] files = file_x.GetFileSystemInfos();
                foreach (FileSystemInfo i in files)
                {
                    deleteFile(i);
                }
            }
            try
            {
                file.Delete();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
