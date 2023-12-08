using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataKendaraan.DataModels
{
    [Table("Data_Kendaraan")]
    public partial class TblDataKendaraan
    {
        public long No { get; set; }
        [Key]
        [Column("No_Registrasi")]
        [StringLength(100)]
        [Unicode(false)]
        public string NoRegistrasi { get; set; } = null!;
        [Column("Nama_Pemilik")]
        [StringLength(100)]
        [Unicode(false)]
        public string NamaPemilik { get; set; } = null!;
        [StringLength(200)]
        [Unicode(false)]
        public string? Alamat { get; set; }
        [Column("Merk_Kendaraan")]
        [StringLength(50)]
        [Unicode(false)]
        public string? MerkKendaraan { get; set; }
        [Column("Tahun_Pembuatan")]
        public int? TahunPembuatan { get; set; }
        [Column("Kapasitas_Silinder")]
        public int? KapasitasSilinder { get; set; }
        [Column("Warna_Kendaraan")]
        [StringLength(50)]
        [Unicode(false)]
        public string? WarnaKendaraan { get; set; }
        [Column("Bahan_Bakar")]
        [StringLength(50)]
        [Unicode(false)]
        public string? BahanBakar { get; set; }
    }
}
