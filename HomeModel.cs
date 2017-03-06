using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheWidgetStore.ViewModels;

namespace TheWidgetStore.Models
{
    public class HomeModel
    {
        public UserAccountViewModel ValidateUser(UserAccountViewModel userViewModel)
        {
            WidgetDBEntities entity = new WidgetDBEntities();
            var customerAccount = (from customer in entity.Customers
                                   where customer.Email == userViewModel.Email
                                      && customer.Password == userViewModel.Password
                                   select customer).FirstOrDefault();

            if (customerAccount != null)
            {
                userViewModel.CustomerID = customerAccount.CustomerID;
                userViewModel.Name = customerAccount.Name;
                return userViewModel;
            }
            else
            {
                return null;
            }
        }

        public UserAccountViewModel CreateUserAccount(UserAccountViewModel userViewModel)
        {
            try
            {
                WidgetDBEntities entity = new WidgetDBEntities();
                Customer customer = new Customer()
                {
                    Name = userViewModel.Name,
                    Email = userViewModel.Email,
                    Password = userViewModel.Password
                };

                entity.Customers.Add(customer);
                entity.SaveChanges();


                userViewModel.CustomerID = customer.CustomerID;
            }
            catch (Exception ex)
            {
                //TODO Change method to return SaveResult
                //saveResult.ErrorMessage = String.Format("There was an error creating your account {0}.", ex.Message);
            }
            return userViewModel;
        }

        public List<Widget> GetAllProducts(ProductsViewModel productsViewModel, out int totalRecords)
        {
            WidgetDBEntities entity = new WidgetDBEntities();
            var widgets = (from w in entity.Products
                           select w);

            string sortBy = productsViewModel.sortBy;
            string direction = productsViewModel.direction;

            switch (sortBy)
            {
                case "ProductID":
                    if (direction == "asc")
                        widgets = widgets.OrderBy(w => w.ProductID);
                    else
                        widgets = widgets.OrderByDescending(w => w.ProductID);
                    break;

                case "ProductName":
                    if (direction == "asc")
                        widgets = widgets.OrderBy(w => w.Name);
                    else
                        widgets = widgets.OrderByDescending(w => w.Name);
                    break;

                case "Price":
                    if (direction == "asc")
                        widgets = widgets.OrderBy(w => w.Price);
                    else
                        widgets = widgets.OrderByDescending(w => w.Price);
                    break;

                default:
                    widgets = widgets.OrderBy(w => w.ProductID);
                    break;
            }

            totalRecords = widgets.Count();

            if (productsViewModel.page != null & productsViewModel.limit != null)
            {
                int page = (int)productsViewModel.page - 1;
                int limit = (int)productsViewModel.limit;

                widgets = widgets.Skip(page * limit).Take(limit);
            }

            List<Widget> widgetList = new List<Widget>();
            foreach (Product product in widgets)
            {
                Widget widget = new Widget()
                {
                    ProductID = product.ProductID,
                    ProductName = product.Name,
                    Price = product.Price
                };
                widgetList.Add(widget);
            }
            return widgetList;
        }

        public decimal GetCartTotal(List<int> productIDList)
        {
            decimal cartTotal = 0;

            if (productIDList.Count > 0)
            {
                WidgetDBEntities entity = new WidgetDBEntities();
                var productSum = (from item in entity.Products
                                  where productIDList.Contains(item.ProductID)
                                  select item.Price).Sum();

                cartTotal = productSum;
            }

            return cartTotal; 
        }

        public List<Widget> GetCartProducts(ProductsViewModel productsViewModel, out int totalRecords)
        {
            // Get all products from the db 
            List<Widget> widgetList = GetAllProducts(productsViewModel, out totalRecords);

            // retrieve the cart ProductID list from the Session object
            List<int> cartList = productsViewModel.productIDList;

            // Filter the list by the products in the cartList
            List<Widget> filteredWidgetList = widgetList.Where(w => cartList.Contains(w.ProductID)).ToList<Widget>();

            return filteredWidgetList; 
        }
    }
}