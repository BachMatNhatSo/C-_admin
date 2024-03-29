
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebsiteAdmin.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Vui lòng nhập Email!")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
       
    }
}
