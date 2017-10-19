using hella.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace hella.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Access Token
            var client = new RestClient("https://10.10.1.12:8091");
            var request = new RestRequest("/auth", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            AuthenticationModel authenticationDetails = new AuthenticationModel
            {
                Username = "user-role-edit",
                Password = "Sm4rtCity"
            };

            request.AddJsonBody(authenticationDetails);

            //X509Certificate2 certificates = new X509Certificate2();
            //certificates.Import(...);
            //client.ClientCertificates = new X509CertificateCollection() { certificates };

            IRestResponse response = client.Execute(request);
            var content = response.Content; 
            Console.WriteLine("Response: " + content);


            // Get Camera Data
            client = new RestClient("https://10.10.1.12:8091/apiv1/sensorData");
            request = new RestRequest("/auth", Method.GET);
            request.AddHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZGVudGl0eSI6MiwibmJmIjoxNTA4MzY2Mzk1LCJleHAiOjE1MDgzNjk5OTUsImlhdCI6MTUwODM2NjM5NX0.4ntLvFuQLxZPP-22sq3_rDP5WYKiFkteYsOTBKEQtPo");
            authenticationDetails = new AuthenticationModel
            {
                Username = "user-role-edit",
                Password = "Sm4rtCity"
            };  

            request.AddJsonBody(authenticationDetails);


            response = client.Execute(request);
            var dataContent = response.Content;
            Console.WriteLine("Response: " + dataContent);


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