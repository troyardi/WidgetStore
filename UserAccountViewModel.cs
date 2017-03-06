using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheWidgetStore.Models; 

namespace TheWidgetStore.ViewModels
{
    public class UserAccountViewModel
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserAccountViewModel ValidateUserLogin(UserAccountViewModel userViewModel)
        {
            HomeModel homeModel = new HomeModel();
            return homeModel.ValidateUser(userViewModel); 
        }

        public UserAccountViewModel CreateUserAccount(UserAccountViewModel userViewModel)
        {
            HomeModel homeModel = new HomeModel();
            return homeModel.CreateUserAccount(userViewModel); 
        }
    }

}