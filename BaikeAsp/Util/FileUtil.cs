using BaikeAsp.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Util
{
    public class FileUtil
    {
        public async static Task<ResourceResult> CreateTempFile(IFormFile file)
        {
            ResourceResult result = new ResourceResult();
            var suffix = Path.GetExtension(file.FileName);
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            using (FileStream fs = File.Create($@"{ResourcePath.TEMP}\{uuid}{suffix}"))
            {
                await file.CopyToAsync(fs);
                fs.Flush();
            }
            result.uuid = uuid.ToString();
            result.Success = true;
            result.type = suffix.Replace(".", "");
            return result;
        }

        public static bool DeleteTempFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }
    }
}
