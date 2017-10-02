using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoProject.Models;
using System.Web.UI.WebControls;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using System.Web.Script.Serialization;


namespace DemoProject.Controllers
{
    
    public class HomeController : Controller
    {
 
        public ActionResult Index()
        {
            InitializeViewData();

            return View();
        }

     

        [HttpPost]
        public ActionResult Index(Order order)
        {
            switch (OrderViewData.btncommand)
            {
                case "SAVE":
                    {
                        OrderDemoDataBase.AddOrderToDataBase(order);
                       
                        InitializeViewData();
                        order = ClearOrderModel(order);

                        return View("Index", order);
                    }
               case "SEND":
                    {
                return View("OrderValidationSuccess", order);
                     }

                case "GETORDER":
                    {

                        foreach (Order o in OrderViewData.Orders)
                        {
                            if (o.OrderId == OrderViewData.RequestedOrder)
                            {
                                //GetAllProductsforOrderId
                                //SetOrderViewDataValues
                                o.RequestedOrderId = OrderViewData.RequestedOrder;
                                OrderViewData.Products = new List<Product>();
                                OrderViewData.Products = OrderDemoDataBase.GetProducts(o.OrderId);
                                OrderViewData.ActiveProductIndex = 0;
                                o.ProductId = OrderViewData.Products[0].ProductId;
                                o.ProductName = OrderViewData.Products[0].ProductName;
                                o.ProductPrice = OrderViewData.Products[0].ProductPrice;
                                o.IncludeProductPrice = OrderViewData.Products[0].IncludeProductPrice;
                                o.MaterialId = OrderViewData.Products[0].SelectedMaterialId;
                                OrderViewData.CalculatedPrice = (int)o.TotalPrice;
                                OrderViewData.Products[0].IsActive = true;
                                return View("Index", o);
                            }
                        }
                        return View("Index", order);
                    }
                case "NEWORDER":
                    {

                        InitializeViewData();
                        order = ClearOrderModel(order);
                        return View("Index", order);

                    }
                case "UPDATEORDER":
                    {

                           OrderDemoDataBase.UpdateOrderInDataBase(order);
                        OrderViewData.Products = OrderDemoDataBase.GetProducts(order.OrderId);

                        OrderViewData.Orders = OrderDemoDataBase.GetOrders();
                        int i = 0;
                        foreach (Order o in OrderViewData.Orders)
                        {
                            if (o.OrderId == OrderViewData.RequestedOrder)
                            {
                                o.OrderName = OrderViewData.Orders[i].OrderName;
                                o.DeliveryDate = OrderViewData.Orders[i].DeliveryDate;
                                o.ValidUntil = OrderViewData.Orders[i].ValidUntil;
                                o.TotalPrice = OrderViewData.Orders[i].TotalPrice;
                                o.RequestedOrderId = OrderViewData.RequestedOrder;

                                OrderViewData.Products = new List<Product>();
                                OrderViewData.Products = OrderDemoDataBase.GetProducts(o.OrderId);
                                OrderViewData.ActiveProductIndex = 0;
                                o.ProductId = OrderViewData.Products[0].ProductId;
                                o.ProductName = OrderViewData.Products[0].ProductName;
                                o.ProductPrice = OrderViewData.Products[0].ProductPrice;
                                o.IncludeProductPrice = OrderViewData.Products[0].IncludeProductPrice;
                                o.MaterialId = OrderViewData.Products[0].SelectedMaterialId;
                                OrderViewData.CalculatedPrice = (int) o.TotalPrice;
                                OrderViewData.Products[0].IsActive = true;
                                return View("Index", o);

                            }
                            i++;
                        }


                        return View("Index", order);
                     

                    }

            }
            return View("Index",order); 
        }
        private void InitializeViewData()
        {
            OrderViewData.btncommand = "";
            OrderViewData.Products = new List<Product>();
            OrderViewData.CalculatedPrice = 0;

            Product p = new Product();
            p.ProductId = 1;
            p.ProductName = "NewProduct";
            p.IsActive = true;
            p.SelectedMaterialId = 0;
            p.IncludeProductPrice = false;
            OrderViewData.Products.Add(p);
            OrderViewData.ActiveProductIndex = 0;
            OrderViewData.Materials = OrderDemoDataBase.GetMaterials();
            OrderViewData.Orders = OrderDemoDataBase.GetOrders();
        }

        private Order ClearOrderModel(Order order)
        {
            order.OrderId = OrderDemoDataBase.GetLastOrderId() + 1;
            order.OrderName = "";
            order.DeliveryDate = (DateTime.UtcNow).Date;
            order.ValidUntil = (DateTime.UtcNow).Date;
            order.TotalPrice = 0;
            order.ProductId = 0;
            order.ProductName = "";
            order.ProductPrice = 0;
            order.MaterialId = 0;
            order.IncludeProductPrice = false;

            return order;
        }
        [HttpPost]
        public ActionResult _OrderRoundPanel_Partial(Order order)
        {
            if (order.OrderId == 0)
            {
                order.OrderId = OrderDemoDataBase.GetLastOrderId() + 1;
                order.DeliveryDate = (DateTime.UtcNow).Date;
                order.ValidUntil = (DateTime.UtcNow).Date;
                order.TotalPrice = 0;
            }
            return PartialView("_OrderRoundPanel_Partial", order);

        }

        private Order Store_Current_Product_In_ViewData(Order order)
        {
            OrderViewData.Products[OrderViewData.ActiveProductIndex].ProductId = order.ProductId;
            OrderViewData.Products[OrderViewData.ActiveProductIndex].ProductName = order.ProductName;
            OrderViewData.Products[OrderViewData.ActiveProductIndex].ProductPrice = order.ProductPrice;
            OrderViewData.Products[OrderViewData.ActiveProductIndex].IncludeProductPrice = order.IncludeProductPrice;
            OrderViewData.Products[OrderViewData.ActiveProductIndex].SelectedMaterialId = order.MaterialId;
            
            return order;
        }

        private Order Retreive_Processed_Product_ViewDataValues_in_Model(Order order)
        {
            // update only those which are not bind to ViewData class
            order.ProductId = OrderViewData.Products[OrderViewData.ActiveProductIndex].ProductId;
            order.ProductName = OrderViewData.Products[OrderViewData.ActiveProductIndex].ProductName;
            order.MaterialId = OrderViewData.Products[OrderViewData.ActiveProductIndex].SelectedMaterialId;
            order.ProductPrice = OrderViewData.Products[OrderViewData.ActiveProductIndex].ProductPrice;
            order.IncludeProductPrice = OrderViewData.Products[OrderViewData.ActiveProductIndex].IncludeProductPrice;

            return order;
        }


        [HttpPost]
        public ActionResult _ProductTabControl_Partial(Order order,string command, string parameter)
        {

            int requestedTabIndex = 0;
            switch (command)
                {               
                case "ADDPRODUCT":
                        {
                        order = Store_Current_Product_In_ViewData(order);
                        requestedTabIndex = Convert.ToInt32(parameter) - 1;
                        OrderViewData.ActiveProductIndex = requestedTabIndex;
                        foreach (Product p in OrderViewData.Products)
                            p.IsActive = false;
                        OrderViewData.Products.Add(new Product()
                        {
                            ProductId = requestedTabIndex + 1,
                            ProductName = "NewProduct",
                            ProductPrice = 0,
                            SelectedMaterialId = 0,
                            IncludeProductPrice = false
                          
                        });
                        OrderViewData.Products[OrderViewData.ActiveProductIndex].IsActive = true;
                        order = Retreive_Processed_Product_ViewDataValues_in_Model(order);       }
                    break;
                case "SETFIRSTPRODUCT":
                    {

                        order.ProductId =  1;
                         order.ProductName = "NewProduct";
                        order.ProductPrice = 0;
                        order.MaterialId = 0;
                        order.IncludeProductPrice = false;
                        
                    }
                    break;
                case "GETPRODUCT":
                    {

                        order = Store_Current_Product_In_ViewData(order);

                        requestedTabIndex = Convert.ToInt32(parameter) - 1;
                        OrderViewData.ActiveProductIndex = requestedTabIndex;
                        foreach (Product p in OrderViewData.Products)
                         p.IsActive = false;
                        OrderViewData.Products[OrderViewData.ActiveProductIndex].IsActive = true;

                        order = Retreive_Processed_Product_ViewDataValues_in_Model(order);

                    }
                    break;

                case "UPDATEPRODUCTNAME":
                    {
                        OrderViewData.Products[OrderViewData.ActiveProductIndex].ProductName = parameter;
                    }
                    break;
                case "UPDATESAVEPRODUCT":
                    {

                        order = Store_Current_Product_In_ViewData(order);
                        
                        order = Retreive_Processed_Product_ViewDataValues_in_Model(order);
                    }
                    break;  }

            return PartialView("_ProductTabControl_Partial",order);
        }

        public ActionResult _ProductTabControl_Partial(Order order)
        {
            return PartialView("_ProductTabControl_Partial", order);
        }
        public ActionResult _ProductTabInfo_Partial(Order order)
        {
           return PartialView("_ProductTabInfo_Partial", order);
        }
        public ActionResult _OrderInfo_Partial(Order order)
        {
             return PartialView("_OrderInfo_Partial", order);
        }

               
        public ActionResult _CallBack_Partial()
        {
           
            return PartialView("_CallBack_Partial");
        }

        [HttpPost]
        public ActionResult _CallBack_Partial(Order order,string command)
        {          

            switch (command)
            {
                
                case "ADDPRICE":
                    {
                        if (OrderViewData.Products[OrderViewData.ActiveProductIndex].IncludeProductPrice == false) // already not included then Add
                        {
                            OrderViewData.CalculatedPrice += order.ProductPrice;
                            OrderViewData.Products[OrderViewData.ActiveProductIndex].IncludeProductPrice = true;
                        }
                    }
                    break;
                case "REMOVEPRICE":
                    {
                        if (OrderViewData.Products[OrderViewData.ActiveProductIndex].IncludeProductPrice == true) // if it was already added, then Remove it
                        {
                            OrderViewData.CalculatedPrice -= order.ProductPrice;
                            OrderViewData.Products[OrderViewData.ActiveProductIndex].IncludeProductPrice = false;
                        }
                             
                    }
                    break;
                case "SAVE":
                    {
                        OrderViewData.btncommand = command;
                    }
                break;

                case "SEND":
                    {
                        OrderViewData.btncommand = command;
                    }
                    break;
                case "GETORDER":
                    {
                        OrderViewData.btncommand = command;
                        OrderViewData.RequestedOrder = order.RequestedOrderId;
                    }break;
                case "NEWORDER":
                    {
                        OrderViewData.btncommand = command;
                     }
                    break;
                case "UPDATEORDER":
                    {
                        OrderViewData.btncommand = command;
                        OrderViewData.RequestedOrder = order.RequestedOrderId;

                    }
                    break;

            }

            order.TotalPrice = OrderViewData.CalculatedPrice;
          return PartialView("_CallBack_Partial", order);

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

        
    }


    

}