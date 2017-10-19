using hella.Models;
using Newtonsoft.Json;
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

            string token = RetrieveAccessToken(out client, out request, out authenticationDetails, out response);
            ContainerModel cameraData = RetrieveCameraData(client, out request, out authenticationDetails, out response, token);
       
            

            return View(cameraData);

        }


        private static ContainerModel RetrieveCameraData(RestClient client, out RestRequest request, out AuthenticationModel authenticationDetails, out IRestResponse response, string token)
        {
            // Get Camera Data
            request = new RestRequest("/apiv1/sensorData/counts", Method.GET);
            request.AddHeader("Authorization", string.Format("Bearer {0}", token));
            authenticationDetails = new AuthenticationModel
            {
                Username = "user-role-edit",
                Password = "Sm4rtCity"
            };

            request.AddJsonBody(authenticationDetails);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            response = client.Execute(request);
            var dataContent = response.Content;

            var container = JsonConvert.DeserializeObject<ContainerModel>(dataContent);
            Console.WriteLine("Response: " + dataContent);

            return container;
        }

        private static string RetrieveAccessToken(out RestClient client, out RestRequest request, out AuthenticationModel authenticationDetails, out IRestResponse response)
        {
            // Access Token
            string token = "";
            client = new RestClient("https://10.10.1.12:8091");
            request = new RestRequest("/auth", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            authenticationDetails = new AuthenticationModel
            {
                Username = "user-role-edit",
                Password = "Sm4rtCity"
            };
            var jsonBody = JsonConvert.SerializeObject(authenticationDetails);
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            //request.AddJsonBody(JsonConvert.SerializeObject(authenticationDetails));

            //X509Certificate2 certificates = new X509Certificate2();
            //certificates.Import(...);
            //client.ClientCertificates = new X509CertificateCollection() { certificates };
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            response = client.Execute(request);
            var content = response.Content;
            Console.WriteLine("Response: " + content);
            var responseModel = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);
            token = responseModel["access_token"].ToString();
            return token;
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