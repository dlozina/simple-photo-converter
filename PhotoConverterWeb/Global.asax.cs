using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using PhotoConverterWebAppv2.App_Work;

namespace PhotoConverterWebAppv2
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static SaveDataApp saveDataApp;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // Store file path in memory
            saveDataApp = new SaveDataApp();
            saveDataApp.Initsavedata();
        }
    }
}
