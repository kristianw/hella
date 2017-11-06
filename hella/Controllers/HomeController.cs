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
        private readonly RestClient _client;

        public HomeController()
        {
            _client = new RestClient("https://192.168.0.100:8091"); // Camera IP on the local network
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; }; // Bypass security certificate
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCounts()
        {
            var accessToken = RetrieveAccessToken(); // retrieve access token from camera for authentication
            var cameraDatas = RetrieveCameraData(accessToken); // retrieve camera data with authentication successful

            return Json(JsonConvert.SerializeObject(cameraDatas), JsonRequestBehavior.AllowGet);
        }

        private string RetrieveAccessToken()
        {
            // Access Token
            var request = new RestRequest("/auth", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var authenticationDetails = new AuthenticationModel
            {
                Username = "user-role-edit", // Camera username to be edited here
                Password = "Sm4rtCity" // Camera password to be edited here
            };
            var jsonBody = JsonConvert.SerializeObject(authenticationDetails);
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            var response = _client.Execute(request);
            var content = response.Content;

            Console.WriteLine("Response: " + response.Content);

            var responseModel = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);
            var token = responseModel["access_token"].ToString();
            return token;
        }

        private List<CameraData> RetrieveCameraData(string token)
        {
            // Get Camera Data
            var request = new RestRequest("/apiv1/sensorData/counts", Method.GET);
            request.AddHeader("Authorization", string.Format("Bearer {0}", token));

            var response = _client.Execute(request);

            var responseModel = JsonConvert.DeserializeObject<ContainerModel>(response.Content);
            Console.WriteLine("Response: " + response.Content);

            var jsonSection = responseModel.Counts.FirstOrDefault(c => c.Name == "Door");
            var cameraDatas = jsonSection.Datas;

            return cameraDatas;
        }
    }
}