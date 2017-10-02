using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;    /// add for validation
using DevExpress.Web.Mvc; /// add for validation
using System.ComponentModel.DataAnnotations; /// add for validation

namespace DemoProject.Models
{
    public class Order
    {

        public int OrderId { get; set; }

        public string OrderName { get; set; }
        public int? TotalPrice { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public DateTime? ValidUntil { get; set; }
        
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int MaterialId { get; set; }

        public int ProductPrice { get; set; }

        public bool IncludeProductPrice {get; set;}
      
        public int RequestedOrderId { get; set; }
    }


}