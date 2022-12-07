using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Aplication.Models
{
    public class ResponseModel<T>
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public T? Data { get; set; }

        public ResponseModel()
        {
        }

        public ResponseModel(T? data, string? message = "")
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public ResponseModel(string? message)
        {
            Succeeded = false;
            Message = message;
        }
    }
}
