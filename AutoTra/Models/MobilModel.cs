using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class MobilModel
    {
        public int? id_mobil { get; set; }

        [RegularExpression(@"^(?!null$).*", ErrorMessage = "type of car to choose!")]
        public string? jenis_mobil { get; set; }

        [Required(ErrorMessage = "Nama must be filled in.")]
        [MaxLength(100, ErrorMessage = "Nama maksimal 100 karakter.")]
        public string? nama { get; set; }

        [VinValidation(ErrorMessage = "VIN number must be filled in.")]
        [RegularExpression(@"^\w{17}$", ErrorMessage = "The VIN number must consist of 17 alphanumeric characters.")]
        public string? vin { get; set; }

        [Required(ErrorMessage = "Engine number must be filled in")]
        [RegularExpression(@"^\w{20}$", ErrorMessage = "Engine number is only 20 characters")]
        public string? no_engine { get; set; }

        [Required(ErrorMessage = "Car color must be filled in")]
        public string? warna { get; set; }

        [Required(ErrorMessage = "Car kilometers number must be filled in")]
        public string? kilometer { get; set; }

        [RegularExpression(@"^(?!null$).*", ErrorMessage = "Fuel must be chosen!")]
        public string? bahan_bakar { get; set; }
        public int? status { get; set; }

        public class VinValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var mobilModel = (MobilModel)validationContext.ObjectInstance;

                // Jika jenis_mobil adalah "Asset," tidak perlu validasi VIN
                if (mobilModel.jenis_mobil == "Asset")
                {
                    return ValidationResult.Success;
                }

                // Cek apakah jenis_mobil adalah "Asset" dan vin tidak kosong
                if (mobilModel.jenis_mobil == "Non Asset" && string.IsNullOrEmpty((string)value))
                {
                    return new ValidationResult(ErrorMessage); // VIN wajib diisi untuk "Asset"
                }

                // Lakukan validasi lainnya sesuai kebutuhan

                return ValidationResult.Success;
            }
        }
    }
}
