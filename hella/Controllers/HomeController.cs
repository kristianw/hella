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
            _client = new RestClient("https://10.10.1.12:8091");
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

        }

        public ActionResult Index()
        {
           
            return View();

        }

        // http://localhost:xx/home/getcounts

        public ActionResult GetCounts()
        {
            var accessToken = RetrieveAccessToken();
            var cameraDatas = RetrieveCameraData(accessToken);

            return Json(JsonConvert.SerializeObject(cameraDatas), JsonRequestBehavior.AllowGet);
        }




        private string RetrieveAccessToken()
        {
            // Access Token
            var request = new RestRequest("/auth", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var authenticationDetails = new AuthenticationModel
            {
                Username = "user-role-edit",
                Password = "Sm4rtCity"
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