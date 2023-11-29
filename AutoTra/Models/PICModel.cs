using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class PICModel
    {
        [Required(ErrorMessage = "NIM wajib diisi.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "NIM harus memiliki panjang antara 5 dan 15 karakter.")]
        [RegularExpression("^[0-9]{5,15}$", ErrorMessage = "NIM hanya boleh berisi angka dan harus 5 hingga 15 angka.")]
        public string? nim { get; set; }

        [Required(ErrorMessage = "Nama wajib diisi.")]
        [MaxLength(100, ErrorMessage = "Nama maksimal 100 karakter.")]
        public string? nama { get; set; }

        [Required(ErrorMessage = "username wajib diisi.")]
        [MaxLength(50, ErrorMessage = "username maksimal 50 karakter.")]
        public string? username { get; set; }

        [Required(ErrorMessage = "Password wajib diisi")]
        [MaxLength(15, ErrorMessage = "Password hanya 15 karakter")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Peran Wajib diisi")]
        public string? peran { get; set; }
        public int? status { get; set; }

    }
}
