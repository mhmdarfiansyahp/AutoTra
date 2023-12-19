using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class AdminModel
    {
        [Required(ErrorMessage = "NPK must be filled in.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "NPK must be between 5 and 15 characters long.")]
        [RegularExpression("^[0-9]{5,15}$", ErrorMessage = "NPK can only contain numbers and must be 5 to 15 numbers.")]
        public string? npk { get; set; }

        [Required(ErrorMessage = "Name must be filled in.")]
        [MaxLength(100, ErrorMessage = "Maximum name 100 characters.")]
        public string? nama { get; set; }

        [Required(ErrorMessage = "username must be filled in.")]
        [MaxLength(50, ErrorMessage = "Maximum username is 50 characters.")]
        public string? username { get; set; }

        [Required(ErrorMessage = "Password must be filled in")]
        [MaxLength(15, ErrorMessage = "Password is only 15 characters")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Peran Wajib diisi")]
        public string? peran { get; set; }
        public int? status { get; set; }

    }
}
