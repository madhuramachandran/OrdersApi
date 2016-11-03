using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersApi.Models
{
    public class OrderDetails
    {
        public int CustId { get; set; }
        public int OrderId { get; set; }
        public double OrderTotal { get; set; }
        public string orderDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}