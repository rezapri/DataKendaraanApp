using DataKendaraan.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataKendaraan.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace DataKendaraan.DataAccess
{
    public class DAKendaraan
    {
        private readonly DB_KendaraanAppContext db;
        private readonly VMResponse response = new();

        public DAKendaraan(DB_KendaraanAppContext _db)
        {
            db = _db;
        }

        private TblDataKendaraan? GetByReg (string reg)
        {
            return (from k in db.DataKendaraans
                    where
                    k.NoRegistrasi == reg
                    select new TblDataKendaraan
                    {
                        No = k.No,
                        NoRegistrasi = k.NoRegistrasi,
                        NamaPemilik = k.NamaPemilik,
                        Alamat = k.Alamat,
                        MerkKendaraan = k.MerkKendaraan,
                        TahunPembuatan = k.TahunPembuatan,
                        KapasitasSilinder = k.KapasitasSilinder,
                        WarnaKendaraan = k.WarnaKendaraan,
                        BahanBakar = k.BahanBakar,
                    }).FirstOrDefault();
        }

        private TblDataKendaraan? GetByNo(int no)
        {
            return (from k in db.DataKendaraans
                    where
                    k.No == no
                    select new TblDataKendaraan
                    {
                        No = k.No,
                        NoRegistrasi = k.NoRegistrasi,
                        NamaPemilik = k.NamaPemilik,
                        Alamat = k.Alamat,
                        MerkKendaraan = k.MerkKendaraan,
                        TahunPembuatan = k.TahunPembuatan,
                        KapasitasSilinder = k.KapasitasSilinder,
                        WarnaKendaraan = k.WarnaKendaraan,
                        BahanBakar = k.BahanBakar,
                    }).FirstOrDefault();
        }

        public VMResponse GetAll()
        {
            try
            {
                List<VMTblDataKendaraan> kendaraan = (
                    from k in db.DataKendaraans orderby k.No select new VMTblDataKendaraan
                    {
                        No = k.No,
                        NoRegistrasi = k.NoRegistrasi,
                        NamaPemilik = k.NamaPemilik,
                        Alamat = k.Alamat,
                        MerkKendaraan = k.MerkKendaraan,
                        TahunPembuatan = k.TahunPembuatan,
                        KapasitasSilinder = k.KapasitasSilinder,
                        WarnaKendaraan = k.WarnaKendaraan,
                        BahanBakar = k.BahanBakar,
                    }).ToList();
                response.Data = kendaraan;
                response.Message = "Data Kendaraan berhasil didapatkan !";
            } catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public VMResponse Get(int id)
        {
            VMResponse response = new VMResponse();
            try
            {
                response.Data = (GetByNo(id) == null) ? null : GetByNo(id);
                response.Message = (response.Data == null) ? "No Data" : $"Get data No = {id} successfully fetched!";

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                response.Data = null;
            }
            return response;
        }

        public VMResponse Add(VMTblDataKendaraan inputDataKendaraan)
        {
            VMResponse response = new VMResponse();

            try
            {
                TblDataKendaraan newData = new TblDataKendaraan
                {
                    NoRegistrasi = inputDataKendaraan.NoRegistrasi,
                    NamaPemilik = inputDataKendaraan.NamaPemilik,
                    Alamat = inputDataKendaraan.Alamat,
                    MerkKendaraan = inputDataKendaraan.MerkKendaraan,
                    TahunPembuatan = inputDataKendaraan.TahunPembuatan,
                    KapasitasSilinder = inputDataKendaraan.KapasitasSilinder,
                    WarnaKendaraan = inputDataKendaraan.WarnaKendaraan,
                    BahanBakar = inputDataKendaraan.BahanBakar
                };

                db.Add(newData);
                db.SaveChanges();

                response.Message = "Kendaraan berhasil ditambahkan!";
                response.Data = inputDataKendaraan;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse Update(VMTblDataKendaraan inputDataKendaraan)
        {
            VMResponse response = new VMResponse();

            try
            {
                TblDataKendaraan dataAwal = GetByReg(inputDataKendaraan.NoRegistrasi);

                if (dataAwal != null)
                {
                    inputDataKendaraan.No = dataAwal.No;
                    dataAwal.NoRegistrasi = inputDataKendaraan.NoRegistrasi;
                    dataAwal.NamaPemilik = inputDataKendaraan.NamaPemilik;
                    dataAwal.Alamat = inputDataKendaraan.Alamat;
                    dataAwal.MerkKendaraan = inputDataKendaraan.MerkKendaraan;
                    dataAwal.WarnaKendaraan = inputDataKendaraan.WarnaKendaraan;
                    dataAwal.TahunPembuatan = inputDataKendaraan.TahunPembuatan;
                    dataAwal.KapasitasSilinder = inputDataKendaraan.KapasitasSilinder;
                    dataAwal.BahanBakar = inputDataKendaraan.BahanBakar;

                    // Assuming you have a stored procedure named 'sp_UpdateKendaraan'
                    // that updates the record based on parameters
                    db.Database.ExecuteSqlRaw("EXEC sp_UpdateKendaraan @NoRegistrasi, @NamaPemilik, @Alamat, @MerkKendaraan, @WarnaKendaraan, @TahunPembuatan, @KapasitasSilinder, @BahanBakar",
                        new SqlParameter("@NoRegistrasi", dataAwal.NoRegistrasi),
                        new SqlParameter("@NamaPemilik", dataAwal.NamaPemilik),
                        new SqlParameter("@Alamat", dataAwal.Alamat),
                        new SqlParameter("@MerkKendaraan", dataAwal.MerkKendaraan),
                        new SqlParameter("@WarnaKendaraan", dataAwal.WarnaKendaraan),
                        new SqlParameter("@TahunPembuatan", dataAwal.TahunPembuatan),
                        new SqlParameter("@KapasitasSilinder", dataAwal.KapasitasSilinder),
                        new SqlParameter("@BahanBakar", dataAwal.BahanBakar)
                    );

                    response.Message = "Kendaraan berhasil diupdate!";
                }
                else
                {
                    response.Message = "Record not found for update.";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse Delete(int id)
        {
            VMResponse response = new VMResponse();

            try
            {
                TblDataKendaraan data = GetByNo(id);
                TblDataKendaraan dataToDelete = db.DataKendaraans.Find(data.NoRegistrasi);

                if (dataToDelete != null)
                {
                    db.DataKendaraans.Remove(dataToDelete);
                    db.SaveChanges();
                    response.Message = "Kendaraan berhasil dihapus!";
                }
                else
                {
                    response.Message = "Kendaraan tidak ditemukan.";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

    }
}
