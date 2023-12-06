using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class KategoriModel
    {
        public int? id { get; set; }

        [Required(ErrorMessage = "Description Inspection Category must be filled.")]
        [MaxLength(300, ErrorMessage = "Input Name Max 300 character.")]
        public string? nama { get; set; }
        public int? status { get; set; }
    }
}
