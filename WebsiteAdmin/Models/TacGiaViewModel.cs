using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebsiteAdmin.Models
{
    public class TacGiaViewModel
    {
        public List<Sach>? ListSachs { get; set; }
        public SelectList? TacGias { get; set; }
        public string? searchForTacGia { get;set; }  
        public string? searchString { get; set; }   
    }
}
