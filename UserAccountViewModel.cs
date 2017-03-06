using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WidgetStore.ViewModels
{
    public class UserAccountViewModel
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}