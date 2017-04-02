using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkFlowaMig.Models
{
    public class Paged<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public int NumberOfPages { get; set; }
    }

    
}