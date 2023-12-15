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

        public string? jenis_mobil { get; set; }

        public string? nama_mobil { get; set; }

        public string? vin { get; set; }

        public string? no_engine { get; set; }

        public string? warna { get; set; }

        public string? kilometer { get; set; }

        public string? bahan_bakar { get; set; }
    }
}
