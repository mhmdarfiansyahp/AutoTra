using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class DosenModel
    {
        [Required(ErrorMessage = "NPK required.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "NPK should be between 5 and 15 characters long.")]
        [RegularExpression("^[0-9]{5,15}$", ErrorMessage = "NPK can only contain numbers and must be 5 to 15 numbers.")]
        public string? npk { get; set; }

        [Required(ErrorMessage = "Name required.")]
        [MaxLength(100, ErrorMessage = "Name up to 100 characters.")]
        public string? nama { get; set; }

        [Required(ErrorMessage = "username required.")]
        [MaxLength(50, ErrorMessage = "username maximum 50 characters.")]
        public string? username { get; set; }

        [Required(ErrorMessage = "Password required")]
        [MaxLength(15, ErrorMessage = "Password only 15 characters")]
        public string? password { get; set; }

        public string? peran { get; set; }
        public int? status { get; set; }
    }
}