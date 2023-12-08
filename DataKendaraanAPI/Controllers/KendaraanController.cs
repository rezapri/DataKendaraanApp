using DataKendaraan.DataAccess;
using DataKendaraan.DataModels;
using DataKendaraan.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataKendaraan.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KendaraanController : Controller
    {
        private DAKendaraan kendaraan;
        private VMResponse response = new();

        //constructor
        public KendaraanController(DB_KendaraanAppContext dB)
        {
            kendaraan = new DAKendaraan(dB);
        }

        [HttpGet("[action]")]
        public VMResponse GetAll() => kendaraan.GetAll();

        [HttpGet("[action]/{id}/")]
        public VMResponse Get(int id) => kendaraan.Get(id);

        [HttpPost]
        public VMResponse Create(VMTblDataKendaraan kendaraanData) => kendaraan.Add(kendaraanData);

        [HttpPut]
        public VMResponse Update(VMTblDataKendaraan kendaraanData) => kendaraan.Update(kendaraanData);

        [HttpDelete("[action]/{id}")]
        public VMResponse Delete(int id) => kendaraan.Delete(id);
    }
}
