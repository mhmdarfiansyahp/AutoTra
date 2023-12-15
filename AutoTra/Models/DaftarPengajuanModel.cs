using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class DaftarPengajuanModel
    {
        public int? id_pengajuan { get; set; }

        public DateTime? tanggl_pengajuan { get; set; }

        public string? nim {get; set; }
        public string? nama { get; set; }
        public string? username { get; set; }

        public string? npk { get; set; }
        public int? id_mobil { get; set; }

        public string? deskripsi { get; set; }
        
        public int? status { get; set; }
    }
}
