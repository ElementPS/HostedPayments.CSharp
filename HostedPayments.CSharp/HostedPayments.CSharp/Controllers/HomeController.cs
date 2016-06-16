using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HostedCheckout.MVC.CSharp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buy()
        {
            //ViewBag.URL = "https://hc.mercurycert.net/CheckoutIFrame.aspx?ReturnMethod=get&pid=" + resp.PaymentID;
            return View();
        }

        public ActionResult Reserve()
        {

            //ViewBag.URL = "https://hc.mercurycert.net/CheckoutIFrame.aspx?ReturnMethod=get&pid=" + resp.PaymentID;
            return View();
        }

        public ActionResult Complete(string PaymentID, string ReturnCode, string ReturnMessage)
        {

            return View();
        }


    }
}