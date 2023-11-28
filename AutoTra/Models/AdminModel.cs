using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class AdminModel
    {
        [Required(ErrorMessage = "NPK wajib diisi.")]
        [MaxLength(15, ErrorMessage = "NPK maksimal 15 karakter.")]
        [RegularExpression("^[0-9]{15}$", ErrorMessage = "NPK hanya boleh berisi angka dan harus 15 angka")]
        public string? npk { get; set; }

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
    }
}
