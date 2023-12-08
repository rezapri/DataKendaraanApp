using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataKendaraan.ViewModels
{
    public class VMResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public VMResponse()
        {
            Success = true;
            Message = string.Empty;
            Data = new object();
        }
    }
}
