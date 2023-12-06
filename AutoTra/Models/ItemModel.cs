using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class ItemModel
    {
        public int? id_item { get; set; }
        [Required(ErrorMessage = "Description Inspection Item must be filled.")]
        [MaxLength(300, ErrorMessage = "Input Name Max 300 character.")]
        public string? nama { get; set; }
        [Required(ErrorMessage = "Inspection Standart must be filled.")]
        public int? id_standart { get; set; }
        [Required(ErrorMessage = "Inspection Category must be filled.")]
        public int? id_kategori { get; set; }
        [Required(ErrorMessage = "Inspection Method must be filled.")]
        public string? metode_inspeksi { get; set; }
        public int? status { get; set; }

    }
}
