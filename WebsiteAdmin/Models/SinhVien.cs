using System.ComponentModel.DataAnnotations;

namespace WebsiteAdmin.Models
{
    public class SinhVien
    {

        public Guid Id { get; set; }
        [Required(ErrorMessage = "Tên Sinh Viên is required")]
        public string? tensinhvien { get; set; }
        [Required(ErrorMessage = "MSSV is required")]
        public string? mssv { get; set; }
        [Required(ErrorMessage = "Điện Thoại is required")]
          public string? dienthoai { get; set; }
        [Required(ErrorMessage = "Địa Chỉ is required")]
        public string? diachi { get; set; }
       
    }
}
