using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace OrdersApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //var config = GlobalConfiguration.Configuration;
            //config.Formatters.Insert(0, new JsonpMediaTypeFormatter());

        }
    }
}
