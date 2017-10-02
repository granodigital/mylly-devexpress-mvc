using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoProject.Models
{
    public class OrderViewData
    {

        private static List<Product> productslist = new List<Product>();
        public static List<Product> Products
        {
            get { return productslist; }
            set { productslist = value; }
        }



        private static List<Material> materialslist = new List<Material>();
        public static List<Material> Materials
        {
            get { return materialslist; }
            set { materialslist = value; }
        }

        private static List<Order> orderslist = new List<Order>();
        public static List<Order> Orders
        {
            get { return orderslist; }
            set { orderslist = value; }
        }

        public static int ActiveProductIndex = 0;
        public static string btncommand { get; set; }

        public static int CalculatedPrice { get; set; }
             
        public static int RequestedOrder { get; set; }

    }
}