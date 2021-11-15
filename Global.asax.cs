using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace TFI_PAD
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                //get the forms authentication ticket
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var aux = authTicket.Name.Split(':');
                //here we suppose userData contains roles joined with ","
                string[] roles = aux.Take(aux.Count() - 1).ToArray();

                //at this point we already have Context.User set by forms authentication module
                //we don't change it but add roles
                var principal = new GenericPrincipal(Context.User.Identity, roles);

                // set new principal with roles
                Context.User = principal;
            }
        }
    }
}
