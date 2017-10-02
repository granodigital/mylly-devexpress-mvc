using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoProject.Models;
using System.Web.UI.WebControls;
using DevExpress.Web.Mvc;
using DevExpress.Web;

namespace DemoProject.Controllers
{
    [ValidateInput(false)]

    public class HomeController : Controller
    {


        public static Order order;

        public ActionResult Index()
        {

            order = new Order();

            if (order.Products.Count() == 0)
            {
                  order.Products.Add(new Product()
                {
                    ProductId = 1,
                    ProductName = "NewProduct",
                    Category = "",
                    Material = "",
                    Price = 0,
                    ProductIndex = 0,
                    IsActive = true,
                });
            }
           
            return View(order);
        }



        [HttpPost]
        public ActionResult Index(Order order1)
        {
            if (order1 == null)
            {
                order1 = order;
                order1.Products = order.Products;
            }
            //if (ModelState.IsValid)
            //{
            //    object redirectActionName = "Index";
            //    return View("OrderValidationSuccess", redirectActionName);
            //}

            return View("OrderValidationSuccess", order1);

            //return View("Index", order);
         }
        [HttpPost]
        public ActionResult _OrderRoundPanel_Partial(string JSONModel)
        {
            //Order o = new Order();

            //o = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(JSONModel);
            //order = o;
            return PartialView("_OrderRoundPanel_Partial", order);

           
        }



        [HttpPost]
        public ActionResult _ProductTabControl_Partial(string command, string _ActiveProductTab, string JSONModel)
        {
            //order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(JSONModel);
            {
                int newpart = order.Products.Count;
                int ActiveProductIndex = (Int32.Parse(_ActiveProductTab));

                switch (command)
                {
                    case "ADDPRODUCT":
                        {
                            foreach (var i in order.Products)
                                i.IsActive = false;
                            order.Products.Add(new Product()
                            {
                                ProductIndex = newpart,
                                ProductId = newpart + 1,
                                ProductName = "NewProduct",
                                IsActive = true,

                            });

                            order.ActiveProductIndex = newpart;
                        }
                        break;

                }
            }

            return PartialView("_ProductTabControl_Partial", order);
        }


        public ActionResult _ProductTabControl_Partial(Order order1)
        {
            if (order1 == null)
                order1 = order;

            return PartialView("_ProductTabControl_Partial", order1);
        }
        public ActionResult _ProductTabInfo_Partial(Order order1)
        {
            if (order1 == null)
                order1 = order;
            return PartialView("_ProductTabInfo_Partial");
        }
        public ActionResult _OrderInfo_Partial(Order order1)

        {
            if (order1 == null)
                order1 = order;
             

            return PartialView("_OrderInfo_Partial", order1);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult _OrderRoundPanel_Partial(Order order1)
        {
            if (order1 == null)
                order1 = order;
            return PartialView("_OrderRoundPanel_Partial", order);
        }

    }

 }