using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    [ExcludeFromCodeCoverage]
    public class ApiKey
    {
        public int id { get; set; }
        public String name { get; set; }
        public String apiKey { get; set; }
    }
}
