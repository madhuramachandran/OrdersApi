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
    public class CustomerController : ApiController
    {
        List<Customer> customers = new List<Customer>();
        bool dataRetrieved = false;

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
            dataRetrieved = true;
        }
        // GET: api/Customer
        public IHttpActionResult Get()
        {
            if (!dataRetrieved)
                populateCustomers();
            var data = new JavaScriptSerializer().Serialize(customers);
            var jsonString = "callback(" + data + ")";

            return new RawJsonActionResult(jsonString);
        }

        // GET: api/Customer/5
        public IHttpActionResult Get(int id)
        {
            if (!dataRetrieved)
                populateCustomers();
            var customer = customers.Where(p => p.CustId == id).ToList();
            var data = new JavaScriptSerializer().Serialize(customer);
            var jsonString = "callback(" + data + ")";

            return new RawJsonActionResult(jsonString);
        }
    }
}