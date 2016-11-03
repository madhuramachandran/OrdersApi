using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersApi.Models
{
    public class Customer
    {
        public int CustId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
