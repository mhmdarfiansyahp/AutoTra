using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class PengajuanKendaraanModel
    {
        public int? id_pengajuan { get; set; }  
        public int? id_pemeriksaan { get; set; }
        public int? id_item { get; set; }

        public DateTime? tanggl_pengajuan { get; set; }

        public string? nim {get; set; }
        public string? nama { get; set; }
        public string? npk { get; set; }
        public int? id_mobil { get; set; }

        public string? deskripsi { get; set; }
        
        public int? status { get; set; }

        public int? id_form { get; set; }
        [RegularExpression(@"^(?!null$).*", ErrorMessage = "Scope must be chosen!")]
        public string? skala { get; set; }
        [RegularExpression(@"^(?!null$).*", ErrorMessage = "Type of Form must be chosen!")]
        public string? jenis_form { get; set; }
        public string? hasil_inspeksi { get; set; }
        public string? alasan { get; set; }
    }
}
