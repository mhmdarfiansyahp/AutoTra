namespace AutoTra.Models
{
    public class Notifikasi2Model
    {
        public int? id_pengajuan { get; set; }

        public string? deskripsi { get; set; }
        public DateTime tanggl_pengajuan { get; set; }
        public string? nim { get; set; }
        public int? status { get; set; }
        public bool IsRead { get; set; }

    }
}
