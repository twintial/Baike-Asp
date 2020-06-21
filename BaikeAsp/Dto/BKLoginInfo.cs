using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKLoginInfo
    {
        [Required(ErrorMessage = "account can't be empty")]
        [EmailAddress(ErrorMessage = "account must be a e-mail address")]
        public string Account { get; set; }
        [Required(ErrorMessage = "password can't be empty")]
        public string Password { get; set; }
    }
}
