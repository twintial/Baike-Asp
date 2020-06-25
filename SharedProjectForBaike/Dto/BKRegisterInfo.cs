using System.ComponentModel.DataAnnotations;

namespace BaikeAsp.Dto
{
    public class BKRegisterInfo
    {
        [Required(ErrorMessage = "account can't be empty")]
        [EmailAddress(ErrorMessage = "account must be a e-mail address")]
        public string Account { get; set; }
        [Required(ErrorMessage = "password can't be empty")]
        public string Password { get; set; }
        [Required(ErrorMessage = "nickname can't be empty")]
        public string NickName { get; set; }
    }
}
