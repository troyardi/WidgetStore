using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheWidgetStore.Models;
using TheWidgetStore.ViewModels;

namespace TheWidgetStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductListing()
        {

            return View();
        }

        [HttpGet, OutputCache(NoStore = true, Duration = 0)]
        public JsonResult GetProductListing(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            ProductsViewModel productsViewModel = new ProductsViewModel();
            productsViewModel.page = page;
            productsViewModel.limit = limit;
            productsViewModel.sortBy = sortBy;
            productsViewModel.direction = direction;
            productsViewModel.searchString = searchString;

            productsViewModel.GetProductList(productsViewModel);

            List<Widget> records = productsViewModel.ProductList;
            int total = productsViewModel.totalRecords;

            JsonResult gridData = Json(new { records, total }, JsonRequestBehavior.AllowGet);
            return gridData;
        }

        [HttpGet, OutputCache(NoStore = true, Duration = 0)]
        public JsonResult GetCartProducts(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            ProductsViewModel productsViewModel = new ProductsViewModel();
            productsViewModel.page = page;
            productsViewModel.limit = limit;
            productsViewModel.sortBy = sortBy;
            productsViewModel.direction = direction;
            productsViewModel.searchString = searchString;

            List<int> cartList = new List<int>();
            if (Session["cartList"] != null)
                cartList = (List<int>)Session["cartList"];

            productsViewModel.productIDList = cartList;

            productsViewModel.GetCartProducts(productsViewModel);

            List<Widget> records = productsViewModel.ProductList;
            int total = productsViewModel.totalRecords;

            JsonResult gridData = Json(new { records, total }, JsonRequestBehavior.AllowGet);
            return gridData;
        }

        public ActionResult MyCart()
        {
            ProductsViewModel productsViewModel = new ProductsViewModel();

            List<int> cartList = new List<int>();
            if (Session["cartList"] != null)
                cartList = (List<int>)Session["cartList"];

            productsViewModel.productIDList = cartList;
            productsViewModel.GetCartTotal();

            ViewBag.cartTotal = productsViewModel.cartTotal; 

            return View();
        }

        public ActionResult AddEditAccount()
        {
            ViewBag.Message = "Create Account";
            return View();
        }

        [HttpPost]
        public string CreateUserAccount(UserAccountViewModel userViewModel)
        {
            UserAccountViewModel userViewModelNewAccount = userViewModel.CreateUserAccount(userViewModel);
            string msg = "";

            if (userViewModelNewAccount != null)
            {
                Session["CustomerID"] = userViewModelNewAccount.CustomerID;
                Session["Name"] = userViewModelNewAccount.Name;
                Session["Email"] = userViewModelNewAccount.Email;
                Session["Password"] = userViewModelNewAccount.Password;
            }
            else
            {
                msg = "Unable to create account :(";
            }

            return msg;
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Login";
            return View();
        }

        [HttpPost]
        public string CustomerLogin(UserAccountViewModel userViewModel)
        {
            UserAccountViewModel userViewModelLoggedIn = userViewModel.ValidateUserLogin(userViewModel);
            string msg = "";

            if (userViewModelLoggedIn != null)
            {
                Session["CustomerID"] = userViewModelLoggedIn.CustomerID;
                Session["Name"] = userViewModelLoggedIn.Name;
                Session["Email"] = userViewModelLoggedIn.Email;
                Session["Password"] = userViewModelLoggedIn.Password;
            }
            else
            {
                msg = "Invalid Email and/or Password";
            }

            return msg;
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return View("~/Views/Home/Index.cshtml");
        }

        public void AddToCart(int productID)
        {
            List<int> cartList = new List<int>();

            if (Session["cartList"] != null)
                cartList = (List<int>)Session["cartList"];

            cartList.Add(productID);
            Session["cartList"] = cartList; 
        }

        public void RemoveFromCart(int productID)
        {
            List<int> cartList = (List<int>)Session["cartList"];

            cartList.RemoveAll(i => i == productID);
            Session["cartList"] = cartList;
        }
    }
}