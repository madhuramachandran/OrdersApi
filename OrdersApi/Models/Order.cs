using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersApi.Models
{
    public class Order
    {
        public int CustId { get; set; }
        public int OrderId { get; set; }
        public double OrderTotal { get; set; }
        public string orderDate { get; set; }
    }
}
