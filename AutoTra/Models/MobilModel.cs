using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class MobilModel
    {
        public string? id_mobil { get; set; }

        [Required(ErrorMessage = "Jenis mobil wajib diisi.")]
        public string? jenis_mobil { get; set; }

        [Required(ErrorMessage = "Nama wajib diisi.")]
        [MaxLength(100, ErrorMessage = "Nama maksimal 100 karakter.")]
        public string? nama { get; set; }

        [Required(ErrorMessage = "Nomor VIN wajib diisi.")]
        [MaxLength(17, ErrorMessage = "nomor VIN maksimal 17 karakter.")]
        public string? vin { get; set; }

        [Required(ErrorMessage = "Nomor Engine wajib diisi")]
        [MaxLength(20, ErrorMessage = "Password hanya 20 karakter")]
        public string? no_engine { get; set; }

        [Required(ErrorMessage = "Warna mobil Wajib diisi")]
        public string? warna { get; set; }

        [Required(ErrorMessage = "Kilometer mobil Wajib diisi")]
        public string? kilometer { get; set; }

        [Required(ErrorMessage = "Bahan bakar mobil Wajib diisi")]
        public string? bahan_bakar { get; set; }
        public int? status { get; set; }
    }
}
