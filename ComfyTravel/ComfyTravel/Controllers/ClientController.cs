using ComfyTravel.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ComfyTravel.Controllers
{
    public class ClientController : Controller
    {
        private IMongoCollection<Objects> _objectsCollection;
        private IMongoCollection<Comments> _commentsCollection;
        private IMongoCollection<SaveRoutes> _saveroutesCollection;

        public ClientController(IMongoClient client)
        {
            //получение через клиента базы данных и коллекций
            var database = client.GetDatabase("ComfyTravel");

            _objectsCollection = database.GetCollection<Objects>("objects");
            _commentsCollection = database.GetCollection<Comments>("comments");
            _saveroutesCollection = database.GetCollection<SaveRoutes>("saveroutes");
        }

        public IActionResult Index(int? id)
        {
            return View("Index");
        }


        [HttpGet]
        public IActionResult GetRoute(ModelOfGettedParametersForRoute routeParams)
        {
            List<Objects> AllPlaces = _objectsCollection.Find(s => s.Type != "none").ToList();
            List<bool> Types = new List<bool>();
            bool Children = false;
            DateTime Start = DateTime.Now;
            TimeSpan Time = new TimeSpan(int.Parse(routeParams.hours), int.Parse(routeParams.minutes), 0);
            string transport = TypesOfTransport.Public;

            switch (routeParams.waytogo)
            {
                case "На машине":
                    transport = TypesOfTransport.Car;
                    ViewData["Mode"] = "2";
                    break;
                case "Пешком":
                    transport = TypesOfTransport.Walk;
                    ViewData["Mode"] = "1";
                    break;
            }

            if (routeParams.park == "Парки")
                Types.Add(true);
            else
                Types.Add(false);

            if (routeParams.museum == "Музеи")
                Types.Add(true);
            else
                Types.Add(false);

            if (routeParams.boulevard == "Бульвары")
                Types.Add(true);
            else
                Types.Add(false);

            if (routeParams.cinema == "Кино")
                Types.Add(true);
            else
                Types.Add(false);

            if (routeParams.monument == "Памятники")
                Types.Add(true);
            else
                Types.Add(false);

            if (routeParams.all == "Всё")
            {
                Types = new List<bool>() { true, true, true, true, true };
            }

            if (routeParams.children == "Дети")
                Children = true;

            List<string> route = RouteGenerationModule.MainGenerate(
                AllPlaces, new List<Objects>(), Types, Children, 
                Start, Time, transport);

            ViewData["Points_x"] = route[0];
            ViewData["Points_y"] = route[1];
            ViewData["Points_names"] = route[2];

            return View("Index");
        }
    }
}
