using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteAdmin.Models
{
    public class Sach
    {

        public int Id { get; set; }
        
        public string? tenSach { get; set; }
        
        public string? tacGia { get; set; }
        
        public double? giaTien { get; set; }
        public string? nxb { get; set; }
    }
}
