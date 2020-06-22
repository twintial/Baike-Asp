using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Util
{
    public class ResourcePath
    {
        public static string ROOT = Directory.GetCurrentDirectory();
        public static string TEMP = Path.Combine(Directory.GetCurrentDirectory(), "resources", "temp");
    }
}
