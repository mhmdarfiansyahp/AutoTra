using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class PICModel
    {
        [Required(ErrorMessage = "NIM must be filled in.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "NIM must be between 5 and 15 characters long.")]
        [RegularExpression("^[0-9]{5,15}$", ErrorMessage = "NIM can only contain numbers and must be 5 to 15 numbers.")]
        public string? nim { get; set; }

        [Required(ErrorMessage = "Nama is required.")]
        [MaxLength(100, ErrorMessage = "Nama name 100 characters.")]
        public string? nama { get; set; }

        [Required(ErrorMessage = "username is mandatory.")]
        [MaxLength(50, ErrorMessage = "username username is 50 characters.")]
        public string? username { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        [MaxLength(15, ErrorMessage = "Password is only 15 characters")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Peran Wajib diisi")]
        public string? peran { get; set; }
        public int? status { get; set; }

    }
}
