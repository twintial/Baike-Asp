using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKVideoUploadViewModel
    {
        public int UID { get; set; }
        public int InterVideoID { get; set; }
        public string VideoName { get; set; }
        public string Introduction { get; set; }
        public int PlayVolume { get; set; }
        public int PraisePoint { get; set; }
        public int CollectPoint { get; set; }
        public string Tag { get; set; }
        public int State { get; set; }
        public DateTime UploadTime { get; set; }
        public string Icon { get; set; }
        public int? InitVideoID { get; set; }
        [Required(ErrorMessage = "视频不能为空")]
        public List<string> VideoFilesUUID { get; set; }
        [Required(ErrorMessage = "视频标题不能为空")]
        public List<string> VIdeoNames { get; set; }
        [Required(ErrorMessage = "封面不能为空")]
        public string CoverUUID { get; set; }
    }
}
