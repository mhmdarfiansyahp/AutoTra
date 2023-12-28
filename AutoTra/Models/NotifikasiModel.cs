namespace AutoTra.Models
{
    public class NotifikasiModel
    {
        public int? id_pengajuan { get; set; }

        public string? deskripsi { get; set; }
        public DateTime tanggl_pengajuan { get; set; }
        public string? npk { get; set; }
        public int? status { get; set; }
        public bool IsRead { get; set; }

    }
}
