using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProjectApi.Models
{
    public class ProductSearchParams
    {
        public string SearchQuery { get; set; }
        public string Category { get; set; }
        public string OrderBy { get; set; } 
        public string Fields { get; set; }
    }
}
