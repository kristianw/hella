using hella.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hella.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var client = new RestClient("https://10.10.1.12:8091/");
            var request = new RestRequest("auth", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            AuthenticationModel authenticationDetails = new AuthenticationModel
            {
                Username = "user-role-edit",
                Password = "Sm4rtCity"
            };

            request.AddJsonBody(authenticationDetails);

            IRestResponse response = client.Execute(request);
            var content = response.Content;


            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}