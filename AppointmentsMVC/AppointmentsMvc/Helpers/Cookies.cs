using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointmentsMvc.Helpers
{
    public class Cookies
    {
        public static string CompanyLinkingKeyCookieName = "specialKey_Cookie";


        public string GetCookieAndDelete(Controller ctrl, string cookieName)
        {
            string returnValue = null;
            HttpCookie cookie = ctrl.HttpContext.Request.Cookies[Helpers.Cookies.CompanyLinkingKeyCookieName];
            if (cookie != null)
            {
                var expiry = DateTime.Now.AddDays(-1);
                returnValue = cookie.Value;
                cookie.Expires = expiry;
                ctrl.HttpContext.Response.Cookies.Add(cookie);
            }

            return returnValue;
        }


        public void SetCookieValue(Controller ctrl, string cookieName, string value, DateTime expiry)
        {
            if(value == null)
            {
                return;
            }

            HttpCookie cookie = new HttpCookie("specialKey_Cookie");
            cookie.Value = value;
            cookie.Expires = expiry;
            ctrl.HttpContext.Response.Cookies.Add(cookie);
        }

    }
}