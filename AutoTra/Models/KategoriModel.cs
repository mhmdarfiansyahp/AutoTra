using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class KategoriModel
    {
        public int? id { get; set; }

        [Required(ErrorMessage = "Deskripsi Standar wajib diisi.")]
        [MaxLength(300, ErrorMessage = "Nama maksimal 300 karakter.")]
        public string? nama { get; set; }
        public int? status { get; set; }
    }
}
