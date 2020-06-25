using BaikeAsp.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using CLRForBaike;

namespace BaikeAsp.Util
{
    public class FileUtil
    {
        public async static Task<ResourceResult> CreateTempFile(IFormFile file)
        {
            var suffix = Path.GetExtension(file.FileName);
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            using (FileStream fs = File.Create($@"{ResourcePath.TEMP}\{uuid}{suffix}"))
            {
                await file.CopyToAsync(fs);
                fs.Flush();
            }
            //ResourceResult result = new ResourceResult();
            //result.uuid = uuid.ToString();
            //result.Success = true;
            //result.type = suffix.Replace(".", "");
            return CLRForBaike.FileUtil.GetRResult(uuid, suffix);
        }

        //public static bool DeleteTempFile(string path)
        //{
        //    if (File.Exists(path))
        //    {
        //        File.Delete(path);
        //        return true;
        //    }
        //    return false;
        //}
    }
}
