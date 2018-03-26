using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace giftshop.Models
{
    public class CustomPrincipal : IPrincipal
    {
        public CustomPrincipal(CustomIdentity identity)
        {
            this.Identity = identity;
        }

        #region IPrincipal Members

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return true; // everyone's a winner
        }

        #endregion
    }

    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name)
        {
            this.Name = name;
        }

        #region IIdentity Members

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.Name); }
        }

        public string Name { get; private set; }

        #endregion
    }

    public static class SessionVar
    {
        public static string Username
        {
            get
            {
                if (HttpContext.Current == null) return string.Empty;
                if (HttpContext.Current.Session["username"] != null)
                    return HttpContext.Current.Session["username"] as string;
                return null;
            }
            set { HttpContext.Current.Session["username"] = value; }
        }

        public static int iduser
        {
            get
            {
                if (HttpContext.Current == null) return 0;
                if (HttpContext.Current.Session["iduser"] != null)
                    return Convert.ToInt16(HttpContext.Current.Session["iduser"]);
                return 0;

            }
            set { HttpContext.Current.Session["iduser"] = value; }
        }



        public static int idrol
        {
            get
            {
                if (HttpContext.Current == null) return 0;
                if (HttpContext.Current.Session["idrol"] != null)
                    return Convert.ToInt16(HttpContext.Current.Session["idrol"]);
                return 0;
            }
            set { HttpContext.Current.Session["idrol"] = value; }
        }

        public static void reset()
        {
            HttpContext.Current.Session["username"] = null;
            HttpContext.Current.Session["iduser"] = null;
            HttpContext.Current.Session["idrol"] = null;
            HttpContext.Current.Session.Abandon();
        }
    }
}