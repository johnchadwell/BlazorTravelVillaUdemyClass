using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelVillaServer.Model
{
    public class ErrorModel
    {
        public string Title { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMsg { get; set; }
    }
}
