using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BaikeAsp.Util
{
    public class ResourcePath
    {
        public static string baseURL = "C:/Users/19134/Desktop/Baike-Asp/BaikeAsp/Resource/";
        public static string tempURL = "C:/Users/19134/Desktop/Baike-Asp/BaikeAsp/Resource/temp/";
        public static string coverImgDirURL(string interVideoID)
        {
            return baseURL + "img/videoCover/" + interVideoID + "/";
        }
        public static string videoDirURL(string interVideoID)
        {
            return baseURL + "video/" + interVideoID + "/";
        }
    }
}
