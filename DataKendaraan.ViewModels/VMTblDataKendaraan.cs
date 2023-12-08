using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataKendaraan.ViewModels
{
    public class VMTblDataKendaraan
    {
        public long No { get; set; }
        [Key]
        [Column("No_Registrasi")]
        [StringLength(100)]
        public string NoRegistrasi { get; set; } = null!;

        [Column("Nama_Pemilik")]
        [StringLength(100)]
        public string NamaPemilik { get; set; } = null!;

        [StringLength(200)]
        public string? Alamat { get; set; }

        [Column("Merk_Kendaraan")]
        [StringLength(50)]
        public string? MerkKendaraan { get; set; }

        [Column("Tahun_Pembuatan")]
        public int? TahunPembuatan { get; set; }

        [Column("Kapasitas_Silinder")]
        public int? KapasitasSilinder { get; set; }

        [Column("Warna_Kendaraan")]
        [StringLength(50)]
        public string? WarnaKendaraan { get; set; }

        [Column("Bahan_Bakar")]
        [StringLength(50)]
        public string? BahanBakar { get; set; }
    }
}
