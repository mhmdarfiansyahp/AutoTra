using System.ComponentModel.DataAnnotations;

namespace AutoTra.Models
{
    public class DetailFormModel
    {
        public int? id_form { get; set; }

        [Required(ErrorMessage = "Choose the Item First!")]
        public int? id_item { get; set; }


        public int? id_mobil { get; set; }

        public string? jenis_mobil { get; set; }

        public string? nama { get; set; }

        public string? vin { get; set; }

        public string? no_engine { get; set; }

        public string? warna { get; set; }

        public string? kilometer { get; set; }

        public string? bahan_bakar { get; set; }
        public int? status { get; set; }



        public string? item_pemeriksaan { get; set; }
        public string? kategori_pemeriksaan { get; set; }
        public string? standart_pemeriksaan { get; set; }
        public string? metode_pemeriksaan { get; set; }

    }
}
