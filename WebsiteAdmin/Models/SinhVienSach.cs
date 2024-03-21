using System.ComponentModel.DataAnnotations;
namespace WebsiteAdmin.Models
{
    public class SinhVienSach
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "SinhVienId is required")]
        public Guid SinhVienId { get; set; }
        [Required(ErrorMessage = "SachId is required")]
        public Guid SachId { get; set; }
        [Required(ErrorMessage = "ngaymuon is required")]
        public DateTime ngaymuon { get; set; }
        [Required(ErrorMessage = "ngaytra Viên is required")]
        public DateTime ngaytra { get; set; }
        public SinhVien? SinhVien { get; set; }
        public Sach? Sach { get; set; }

    }
}
