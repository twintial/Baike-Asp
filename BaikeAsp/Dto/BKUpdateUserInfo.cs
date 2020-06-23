using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BaikeAsp.Dto
{
    public class BKUpdateUserInfo
    {
        [Required(ErrorMessage = "nickName can't be empty")]
        public string NickName { get; set; }
        [Required(ErrorMessage = "introduction can't be empty")]
        public string Introduction { get; set; }
    }
}
