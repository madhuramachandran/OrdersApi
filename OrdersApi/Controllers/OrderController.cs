using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrdersApi.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace OrdersApi.Controllers
{
    public class OrderController : ApiController
    {
        List<Order> orders = new List<Order>();
        List<Customer> customers = new List<Customer>();
        List<OrderDetails> orderDetails = new List<OrderDetails>();
        bool dataRetrieved = false;

        private void populateOrders()
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = @"Data Source=candidates2.database.windows.net;Initial Catalog=CandidateTesting;Integrated Security=False;User ID=INTL-Candidate;Password=Can 1 hav3 a j06?;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "Select * from [Order]";
            sqlCommand.Connection = myConnection;
            myConnection.Open();
            Order order = null;
            reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                order = new Order();
                order.OrderId = Convert.ToInt32(reader.GetValue(0));
                order.CustId = Convert.ToInt32(reader.GetValue(1));
                order.OrderTotal = Convert.ToDouble(reader.GetValue(2));
                order.orderDate = Convert.ToString(reader.GetValue(3)).Split(' ')[0];
                orders.Add(order);
            }
            myConnection.Close();
            populateCustomers();
            populateOrderDetails();
            dataRetrieved = true;
        }

        private void populateCustomers()
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = @"Data Source=candidates2.database.windows.net;Initial Catalog=CandidateTesting;Integrated Security=False;User ID=INTL-Candidate;Password=Can 1 hav3 a j06?;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "Select * from [Customer]";
            sqlCommand.Connection = myConnection;
            myConnection.Open();
            reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                Customer customer = new Customer();
                customer.CustId = Convert.ToInt32(reader.GetValue(0));
                customer.FirstName = reader.GetValue(1).ToString();
                customer.LastName = reader.GetValue(2).ToString();
                customers.Add(customer);
            }
            myConnection.Close();
            
        }

        private void populateOrderDetails()
        {
            foreach(var order in orders)
            {
                OrderDetails details = new OrderDetails();
                details.CustId = order.CustId;
                details.orderDate = order.orderDate;
                details.OrderId = order.OrderId;
                details.OrderTotal = order.OrderTotal;
                var customer = customers.Find(p => p.CustId == order.CustId);
                details.FirstName = customer.FirstName;
                details.LastName = customer.LastName;
                orderDetails.Add(details);
            }
        }

        // GET: api/Order
        public IHttpActionResult Get()
        {
            if (!dataRetrieved)
                populateOrders();
            var data = new JavaScriptSerializer().Serialize(orderDetails);
            var jsonString = "callback(" + data + ")";

            return new RawJsonActionResult(jsonString);
        }

        // GET: api/Order/5
        //public Order Get(int id)
        //{
        //    if (!dataRetrieved)
        //        populateOrders();
        //    var order = orders.FirstOrDefault(); // TODO: fix this FirstOrDefault(p => p.OrderId == id);
        //    if (order == null)
        //        return null;
        //    return  order;
        //}

        public IHttpActionResult Get(int id)
        {
            if (!dataRetrieved)
                populateOrders();
           // var order = orders.Take(2).ToList(); // TODO: fix this FirstOrDefault(p => p.OrderId == id);
            var order = orderDetails.Where(p => p.OrderId == id).ToList();
            if (order == null)
                return null;
            //return Request.CreateResponse(HttpStatusCode.OK, order);
            var data = new JavaScriptSerializer().Serialize(order);
            var jsonString = "callback("+ data + ")";

            return new RawJsonActionResult(jsonString);
        }


    }

    public class RawJsonActionResult : IHttpActionResult
    {
        private readonly string _jsonString;

        public RawJsonActionResult(string jsonString)
        {
            _jsonString = jsonString;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var content = new StringContent(_jsonString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
            return Task.FromResult(response);
        }
    }
    
}