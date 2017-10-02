using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DemoProject.Models
{
    public class OrderDemoDataBase
    {

        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings[""].ConnectionString;
        private static string connStringMyllyDB = System.Configuration.ConfigurationManager.ConnectionStrings[""].ConnectionString;

        private static SqlConnection conn;
        private static Order _ordercontent = new Order();

        private static List<Material> _materials;
        private static Material _material;

        private static List<Order> _orders;
        private static Order _order;

        private static List<Product> _products;
        private static Product _product;

        public static int GetLastOrderId()
        {
             conn = new SqlConnection(connString);
            using (conn)
            {
                conn.Open();
                 _ordercontent.OrderId = GetLastOrderDataId();
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return _ordercontent.OrderId;
        }
        private static int GetLastOrderDataId()
        {
            var cmd = new SqlCommand("GetLastOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    var OrderIdOrdinal = rdr.GetOrdinal("OrderId");
                    _ordercontent.OrderId = (int) rdr.GetValue(OrderIdOrdinal);
                    
                }
            }

            return _ordercontent.OrderId;
        }

        public static List<Material> GetMaterials()
        {
            _materials = new List<Material>();
            conn = new SqlConnection(connStringMyllyDB);
            var sql = @"SELECT [Id],[Name],[FI_Name],[RU_Name],[SV_Name] 
                        FROM [dbo].[Material]";

            var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            using (conn)
            {
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        _material = new Material();
                        _material.MaterialId = (int) (byte) rdr["Id"];
                        _material.MaterialName = rdr["Name"].ToString();
                        _materials.Add(_material);
                    }
                }

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            return _materials;
        }

        public static List<Order> GetOrders()
        {
            _orders = new List<Order>();
            conn = new SqlConnection(connString);
            var sql = @"SELECT [OrderId],[OrderName],[TotalPrice],[DesiredDeliveryDate],[ValidUntil] 
                        FROM [dbo].[Order]";

            var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            using (conn)
            {
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        _order = new Order();

                        _order.OrderId = (int) rdr["OrderId"];
                        _order.OrderName = rdr["OrderName"].ToString();
                        _order.TotalPrice = (int) rdr["TotalPrice"];
                        _order.DeliveryDate = (DateTime) rdr["DesiredDeliveryDate"];
                        _order.ValidUntil = (DateTime) rdr["ValidUntil"];
                        _orders.Add(_order);
                    }
                }

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            return _orders;
        }

        public static List<Product> GetProducts(int orderid)
        {
            _products = new List<Product>();
            conn = new SqlConnection(connString);
            var sql = @"SELECT [ProductId],[ProductName],[MaterialId],[ProductPrice],[IncludeProductPrice] 
                        FROM [dbo].[Product] WHERE [OrderId] = '"+ orderid.ToString()+"'";

            var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            using (conn)
            {
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        _product = new Product();
                        _product.ProductId = (int) rdr["ProductId"];
                        _product.ProductName = rdr["ProductName"].ToString();
                        _product.SelectedMaterialId = (int) rdr["MaterialId"];
                        _product.ProductPrice = (int) rdr["ProductPrice"];
                        _product.IncludeProductPrice = rdr.GetBoolean( rdr.GetOrdinal("IncludeProductPrice"));
                        
                        _products.Add(_product);
                    }
                }

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            return _products;
        }


        public static Order ReturnOrderData(int id, Order ordercontent)
        {
            _ordercontent = ordercontent;
            conn = new SqlConnection(connString);
            using (conn)
            {
                conn.Open();
                _ordercontent = GetOrderData(id);
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return _ordercontent;
        }
        private static Order GetOrderData(int id)
        {
            var cmd = new SqlCommand("GetOrderInformation", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", id);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    var OrderNameOrdinal = rdr.GetOrdinal("OrderName");
                    _ordercontent.OrderName = rdr.IsDBNull(OrderNameOrdinal) ? null : (string) rdr.GetValue(OrderNameOrdinal);
                    
                    var DesiredDeliveryDateOrdinal = rdr.GetOrdinal("DesiredDeliveryDate");
                    _ordercontent.DeliveryDate = rdr.IsDBNull(DesiredDeliveryDateOrdinal) ? null : (DateTime?) rdr.GetValue(DesiredDeliveryDateOrdinal);


                    var ValidUntilOrdinal = rdr.GetOrdinal("ValidUntil");
                    _ordercontent.ValidUntil = rdr.IsDBNull(ValidUntilOrdinal) ? null : (DateTime?) rdr.GetValue(ValidUntilOrdinal);


                    var TotalPriceOrdinal = rdr.GetOrdinal("TotalPrice");
                    _ordercontent.TotalPrice = rdr.IsDBNull(TotalPriceOrdinal) ? null : (int?) rdr.GetValue(TotalPriceOrdinal);
                                       
                    _ordercontent.OrderId = id;
                }
            }
            
            return _ordercontent;
        }
        public static void UpdateOrderInDataBase( Order ordercontent)
        {
            conn = new SqlConnection(connString);
            using (conn)
            {
                conn.Open();
                 UpdateOrderData(ordercontent);
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ;
        }
        private static void UpdateOrderData( Order ordercontent)
        {

             UpdateOrder( ordercontent);

            /////// Add Product ///////////////////

            foreach (var product in OrderViewData.Products)
            {

                 UpdateProduct(ordercontent.OrderId,  product);

            }
            return;
        }
        private static void UpdateOrder( Order ordercontent)
        {
            var cmd = new SqlCommand("UpdateOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", ordercontent.OrderId);
            cmd.Parameters.AddWithValue("@OrderName", ordercontent.OrderName);
            cmd.Parameters.AddWithValue("@DesiredDeliveryDate", ordercontent.DeliveryDate);
            cmd.Parameters.AddWithValue("@ValidUntil", ordercontent.ValidUntil);
            cmd.Parameters.AddWithValue("@TotalPrice", ordercontent.TotalPrice);

            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd.ExecuteScalar();

            return ;
        } 
        private static void UpdateProduct(int _orderid, Product product)
        {
            var cmd = new SqlCommand("UpdateProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", _orderid);
            cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("@MaterialId", product.SelectedMaterialId);
            cmd.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);
            if (product.IncludeProductPrice == true)
                cmd.Parameters.AddWithValue("@IncludeProductPrice", 1);
            else
                cmd.Parameters.AddWithValue("@IncludeProductPrice", 0);

            if (conn.State == ConnectionState.Closed)
                conn.Open();
           cmd.ExecuteScalar();

            return;
        }


        public static Order AddOrderToDataBase(Order ordercontent)
        {
            conn = new SqlConnection(connString);
            using (conn)
            {
                conn.Open();
                     ordercontent = AddOrder(ordercontent);
                
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            return ordercontent;
        }

        private static Order AddOrder(Order ordercontent)
        {
           
           int _orderId = AddNewOrder(ordercontent);

            /////// Add Product ///////////////////

            foreach (var product in OrderViewData.Products)
            {
                
               int _rowId = AddNewProduct(ordercontent.OrderId,product);
               
            }
            return ordercontent;
        }
        private static int AddNewOrder(Order ordercontent)
        {
            var cmd = new SqlCommand("AddNewOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", ordercontent.OrderId);
            cmd.Parameters.AddWithValue("@OrderName", ordercontent.OrderName);
            cmd.Parameters.AddWithValue("@DesiredDeliveryDate", ordercontent.DeliveryDate);
            cmd.Parameters.AddWithValue("@ValidUntil", ordercontent.ValidUntil);
            cmd.Parameters.AddWithValue("@TotalPrice", ordercontent.TotalPrice);

            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var result = cmd.ExecuteScalar();

            return (int) result;
        }
        private static int AddNewProduct(int _orderid, Product Product)
        {
            var cmd = new SqlCommand("AddNewProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", _orderid);
            cmd.Parameters.AddWithValue("@ProductId", Product.ProductId);
            cmd.Parameters.AddWithValue("@ProductName", Product.ProductName);
            cmd.Parameters.AddWithValue("@MaterialId", Product.SelectedMaterialId);
            cmd.Parameters.AddWithValue("@ProductPrice", Product.ProductPrice);
            if(Product.IncludeProductPrice == true)
            cmd.Parameters.AddWithValue("@IncludeProductPrice", 1);
            else
                cmd.Parameters.AddWithValue("@IncludeProductPrice", 0);
            
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var result = cmd.ExecuteScalar();

            return (int) result;
        }



    }



}

