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
        public int SelectedMaterialId{ get; set; }
        public int ProductPrice { get; set; }
        public bool IsActive   { get; set; }

        public bool IncludeProductPrice { get; set; }
     
    }
   
}