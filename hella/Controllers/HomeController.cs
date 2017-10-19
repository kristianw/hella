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
            RestClient client;
            RestRequest request;
            AuthenticationModel authenticationDetails;
            IRestResponse response;

            RetrieveAccessToken(out client, out request, out authenticationDetails, out response);
            RetrieveCameraData(client, out request, out authenticationDetails, out response);

            return View();

        }

        private static void RetrieveCameraData(RestClient client, out RestRequest request, out AuthenticationModel authenticationDetails, out IRestResponse response)
        {
            // Get Camera Data
            request = new RestRequest("/apiv1/sensorData", Method.GET);
            request.AddHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZGVudGl0eSI6MiwibmJmIjoxNTA4MzY2Mzk1LCJleHAiOjE1MDgzNjk5OTUsImlhdCI6MTUwODM2NjM5NX0.4ntLvFuQLxZPP-22sq3_rDP5WYKiFkteYsOTBKEQtPo");
            authenticationDetails = new AuthenticationModel
            {
                Username = "user-role-edit",
                Password = "Sm4rtCity"
            };

            request.AddJsonBody(authenticationDetails);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            response = client.Execute(request);
            var dataContent = response.Content;
            Console.WriteLine("Response: " + dataContent);
        }

        private static void RetrieveAccessToken(out RestClient client, out RestRequest request, out AuthenticationModel authenticationDetails, out IRestResponse response)
        {
            // Access Token
            client = new RestClient("https://10.10.1.12:8091");
            request = new RestRequest("/auth", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            authenticationDetails = new AuthenticationModel
            {
                Username = "user-role-edit",
                Password = "Sm4rtCity"
            };
            request.AddJsonBody(authenticationDetails);

            //X509Certificate2 certificates = new X509Certificate2();
            //certificates.Import(...);
            //client.ClientCertificates = new X509CertificateCollection() { certificates };
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            response = client.Execute(request);
            var content = response.Content;
            Console.WriteLine("Response: " + content);
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