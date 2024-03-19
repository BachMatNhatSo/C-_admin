namespace WebsiteAdmin.Models
{
    public class SinhVienSach
    {
        public Guid Id { get; set; }
        public Guid SinhVienId { get; set; }
        public Guid SachId { get; set; }
        public DateTime ngaymuon { get; set; }
        public DateTime ngaytra { get; set; }
        public SinhVien SinhVien { get; set; }
        public Sach Sach { get; set; }

    }
}
