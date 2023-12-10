using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class ItemModel
    {
        public int? id_item { get; set; }
        [Required(ErrorMessage = "Description Inspection Item must be filled.")]
        [MaxLength(300, ErrorMessage = "Input Name Max 300 character.")]
        public string? item_pemeriksaan { get; set; }
        [Required(ErrorMessage = "Inspection Standart must be filled.")]
        public string? kategori_pemeriksaan { get; set; }
        [Required(ErrorMessage = "Inspection Category must be filled.")]
        public string? standart_pemeriksaan { get; set; }
        [Required(ErrorMessage = "Inspection Method must be filled.")]
        public string? metode_pemeriksaan { get; set; }
        public int? status { get; set; }    

    }
}
