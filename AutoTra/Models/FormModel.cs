using System.ComponentModel.DataAnnotations;
namespace AutoTra.Models
{
    public class FormModel
    {
        public int? id_form { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Car Model must be chosen!.")]
        public int? id_mobil { get; set; }
        [RegularExpression(@"^(?!null$).*", ErrorMessage = "Scope must be chosen!")]
        public string? skala { get; set; }
        [RegularExpression(@"^(?!null$).*", ErrorMessage = "Type of Form must be chosen!")]
        public string? jenis_form { get; set; }
        public int? status { get; set; }
    }
}
