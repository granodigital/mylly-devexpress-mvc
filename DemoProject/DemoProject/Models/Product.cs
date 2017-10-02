using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoProject.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string Material { get; set; }

        public int Price { get; set; }
        public int ProductIndex  {  get;  set; }
        public bool IsActive   { get; set; }
        public bool IsSelected { get; set; }

    }
   
}