using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheWidgetStore.Models;

namespace TheWidgetStore.ViewModels
{
    public class ProductsViewModel
    {
        private HomeModel homeModel = new HomeModel();

        public List<Widget> ProductList { get; set; }

        // used by JQuery grid
        public int? page { get; set; }
        public int? limit { get; set; }
        public string sortBy { get; set; }
        public string direction { get; set; }
        public string searchString { get; set; }
        public int totalRecords { get; set; }

        public List<int> productIDList { get; set; }
        public decimal cartTotal { get; set; }

        public void GetProductList(ProductsViewModel productsViewModel)
        {
            int total;
            ProductList = homeModel.GetAllProducts(productsViewModel, out total);
            totalRecords = total; 
        }

        public void GetCartProducts(ProductsViewModel productsViewModel)
        {
            int total;
            ProductList = homeModel.GetCartProducts(productsViewModel, out total);
            totalRecords = total; 
            // Loop through product list, and add each product price to cart total 
        }

        public void GetCartTotal()
        {
            cartTotal = homeModel.GetCartTotal(productIDList);
        }
    }

    public class Widget
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}