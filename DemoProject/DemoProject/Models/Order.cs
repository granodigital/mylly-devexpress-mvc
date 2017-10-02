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

        private List<Product> productslist = new List<Product>();
        

        public int OrderId { get; set; }


        [Display(Name = "Name:")]
        [Required(ErrorMessage = "Name is required")]
        public string OrderName { get; set; }

        

        [Display(Name = "Age:")]
        [Range(18, 100, ErrorMessage = "Must be between 18 and 100")]

        public int? TotalPrice { get; set; }

        [Display(Name = "Arrival Date:")]
        [Required(ErrorMessage = "Arrival date is required")]
       public DateTime? DeliveryDate { get; set; }


        [Display(Name = "ValidUntil Date:")]
        [Required(ErrorMessage = "Arrival date is required")]

        public DateTime? ValidUntil { get; set; }

        public List<Product> Products
        {
            get { return productslist; }
            set { productslist = value; }
        }

        public int ActiveProductIndex { get; set; }
    }


}