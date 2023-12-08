using DataKendaraanApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using DataKendaraan.ViewModels;
using Newtonsoft.Json;
using DataKendaraan.DataModels;

namespace DataKendaraanApp.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();
        
        public HomeController(IConfiguration _config)
        {
            //_logger = logger;
            apiUrl = _config["ApiUrl"];
        }

        public async Task <IActionResult> Index()
        {
            VMResponse response = new VMResponse();
            ViewBag.Title = "Data Kendaraan";
            try
            {
                response = JsonConvert.DeserializeObject<VMResponse?>(
                    await httpClient.GetStringAsync(apiUrl + "Kendaraan/GetAll"));

                if(response == null)
                {
                    Privacy();
                }
                List<VMTblDataKendaraan>? kendaraan =
                    JsonConvert.DeserializeObject<List<VMTblDataKendaraan>>(response.Data.ToString());
                return View(kendaraan);
            }catch (Exception ex)
            {
                response.Success = false;
                Error();
            }
            return View();
        }

        public async Task<IActionResult> Add()
        {
            VMTblDataKendaraan data = new VMTblDataKendaraan();
            return View(data);
        }

        [HttpPost]
        public async Task<VMResponse?> Add(VMTblDataKendaraan data)
        {
            VMResponse response = new VMResponse();

            try
            {
                // Validasi di sisi server
                if (ModelState.IsValid)
                {
                    // Validasi di sisi server
                    bool isUnique = IsNoRegistrasiUnique(data.NoRegistrasi);

                    if (!isUnique)
                    {
                        ModelState.AddModelError("NoRegistrasi", "Nomor registrasi sudah digunakan. Gunakan nomor registrasi lain.");
                        response.Message = GetModelStateErrors();
                        response.Success = false;
                        return response;
                    }

                    string jsonData = JsonConvert.SerializeObject(data);
                    HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    response = JsonConvert.DeserializeObject<VMResponse?>(await (
                        await httpClient.PostAsync(apiUrl + "Kendaraan", httpContent)).Content.ReadAsStringAsync());

                    response.Success = true;
                    response.Message = "Data berhasil ditambahkan.";

                }
                else
                {
                    response.Message = GetModelStateErrors();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        private bool IsNoRegistrasiUnique(string noRegistrasi)
        {
            // Lakukan pemeriksaan keunikan di database menggunakan Entity Framework Core
            // Gantilah dengan DbContext dan model entity yang sesuai dengan implementasi Anda
            using (var context = new DB_KendaraanAppContext())
            {
                // Lakukan pemeriksaan keunikan di database menggunakan DbContext
                bool isUnique = !context.DataKendaraans.Any(k => k.NoRegistrasi == noRegistrasi);

                return isUnique;
            }
        }

        private string GetModelStateErrors()
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            // Gabungkan pesan-pesan kesalahan menjadi satu string, pisahkan dengan koma atau karakter pemisah lainnya jika diinginkan
            return string.Join(", ", errorMessages);
        }

        public async Task<IActionResult> Edit(int id)
        {
            VMResponse? vMResponse = new VMResponse();
            try
            {
                vMResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "Kendaraan/Get/" + id));
                if(vMResponse == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Title = "Edit Kendaraan";
                VMTblDataKendaraan? data = JsonConvert.DeserializeObject<VMTblDataKendaraan>(
                    vMResponse.Data.ToString());
                return View(data);
            }catch (Exception ex)
            {
                vMResponse.Success = false;
                vMResponse.Message = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<VMResponse?> Edit(VMTblDataKendaraan data)
        {
            VMResponse? response = new VMResponse();
            try
            {
                // Convert Form Data to Json
                string jsonData = JsonConvert.SerializeObject(data);

                // Wrapping JSon inside an HTML Body to be send as HTTP Put Request method 
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                response = JsonConvert.DeserializeObject<VMResponse?>(await (
                        await httpClient.PutAsync(apiUrl + "Kendaraan", content)).Content.ReadAsStringAsync());

                if (response.Data == null)
                {
                    throw new ArgumentException("Category API cannot be reached!");
                }

            } catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<IActionResult> Show(int id)
        {
            VMResponse? vMResponse = new VMResponse();
            try
            {
                vMResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "Kendaraan/Get/" + id));
                if (vMResponse == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Title = "Edit Kendaraan";
                VMTblDataKendaraan? data = JsonConvert.DeserializeObject<VMTblDataKendaraan>(
                    vMResponse.Data.ToString());
                return View(data);
            }
            catch (Exception ex)
            {
                vMResponse.Success = false;
                vMResponse.Message = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            VMResponse? vMResponse = new VMResponse();
            try
            {
                // Menggunakan metode DELETE untuk menghapus data
                HttpResponseMessage response = await httpClient.DeleteAsync(apiUrl + "Kendaraan/Delete/" + id);

                if (response.IsSuccessStatusCode)
                {
                    // Hapus berhasil
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Hapus gagal
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        vMResponse.Message = "Data tidak ditemukan.";
                    }
                    else
                    {
                        vMResponse.Message = "Gagal menghapus kendaraan.";
                    }

                    // Anda juga dapat menambahkan logika tambahan di sini
                    vMResponse.Success = false;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                vMResponse.Success = false;
                vMResponse.Message = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}