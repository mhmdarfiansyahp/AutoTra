﻿using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class MobilModel
    {
        public int? id_mobil { get; set; }

        [Required(ErrorMessage = "Jenis mobil wajib diisi.")]
        public string? jenis_mobil { get; set; }

        [Required(ErrorMessage = "Nama wajib diisi.")]
        [MaxLength(100, ErrorMessage = "Nama maksimal 100 karakter.")]
        public string? nama { get; set; }

        [Required(ErrorMessage = "Nomor VIN wajib diisi.")]
        [RegularExpression(@"^\w{17}$", ErrorMessage = "Nomor VIN harus terdiri dari 17 karakter alfanumerik.")]
        public string? vin { get; set; }

        [Required(ErrorMessage = "Nomor Engine wajib diisi")]
        [MaxLength(20, ErrorMessage = "Password hanya 20 karakter")]
        public string? no_engine { get; set; }

        [Required(ErrorMessage = "Warna mobil Wajib diisi")]
        public string? warna { get; set; }

        [Required(ErrorMessage = "Kilometer mobil Wajib diisi")]
        public string? kilometer { get; set; }

        [RegularExpression(@"^(?!null$).*", ErrorMessage = "Fuel must be chosen!")]
        public string? bahan_bakar { get; set; }
        public int? status { get; set; }
    }
}
