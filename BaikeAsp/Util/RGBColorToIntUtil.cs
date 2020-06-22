using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Util
{
    public class RGBColorToIntUtil
    {
        public static int RGBToInt(string rgb)
        {
            string processedRGB = rgb.Replace("#", "");
            if (processedRGB.Length < 6)
            {
                processedRGB += processedRGB;
            }
            try
            {
                return Convert.ToInt32(processedRGB, 16);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
