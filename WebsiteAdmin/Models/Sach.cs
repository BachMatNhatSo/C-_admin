using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteAdmin.Models
{
    public class Sach
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Tên Sách is required")]
        public string? tenSach { get; set; }
        [Required(ErrorMessage = "Tác Giả is required")]
        public string? tacGia { get; set; }
        [Required(ErrorMessage = "Giá Tiền is required")]
        public double? giaTien { get; set; }
        [Required(ErrorMessage = "Nhà Xuất Bản is required")]
        public string? nxb { get; set; }
       
    }
}
