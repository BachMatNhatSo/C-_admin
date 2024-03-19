using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebsiteAdmin.Models
{
    public class ViewModelSachSinhVien
    {
        public Guid Id { get; set; }
        public string tenSinhVien { get; set; }
        public string tenSach { get; set; }
        public DateOnly ngaymuon { get; set; }
        public DateOnly ngaytra { get; set; }
        public SinhVienSach sinhVienSach { get; set; }
        public Guid SachId { get; set; } // Thêm trường SachId
        public Guid SinhVienId { get; set; } // Thêm trường SinhVienId


    }
}
