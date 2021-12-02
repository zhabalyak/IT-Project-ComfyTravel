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
    public class MainController : Controller
    {
        private IMongoCollection<Objects> _objectsCollection;
        private IMongoCollection<Comments> _commentsCollection;
        private IMongoCollection<SaveRoutes> _saveroutesCollection;

        public MainController(IMongoClient client)
        {
            //получение через клиента базы данных и коллекций
            var database = client.GetDatabase("ComfyTravel");

            _objectsCollection = database.GetCollection<Objects>("objects");
            _commentsCollection = database.GetCollection<Comments>("comments");
            _saveroutesCollection = database.GetCollection<SaveRoutes>("saveroutes");
        }


        public IActionResult Index(int? id)
        {
            ViewData["Points_x"] = "0";
            ViewData["Points_y"] = "0";
            ViewData["Points_names"] = "0";

            return View("Index");
        }

        public IActionResult Start()
        {
            return View("Start");
        }


        [HttpGet]
        public IActionResult GetParams(ModelOfCheckedParams checkedParams)
        {
            List<Objects> AllPlaces = new List<Objects>();

            if (checkedParams.park == "Парки")
                AllPlaces.AddRange(_objectsCollection.Find(s => s.Type == TypesOfObjects.Park).ToList());
            if (checkedParams.boulevard == "Бульвары")
                AllPlaces.AddRange(_objectsCollection.Find(s => s.Type == TypesOfObjects.Boulevard).ToList());
            if (checkedParams.museum == "Музеи")
                AllPlaces.AddRange(_objectsCollection.Find(s => s.Type == TypesOfObjects.Museum).ToList());
            if (checkedParams.monument == "Памятники")
                AllPlaces.AddRange(_objectsCollection.Find(s => s.Type == TypesOfObjects.Monument).ToList());
            if (checkedParams.cinema == "Кино")
                AllPlaces.AddRange(_objectsCollection.Find(s => s.Type == TypesOfObjects.Cinema).ToList());
            if (checkedParams.all == "Всё")
            {
                AllPlaces.AddRange(_objectsCollection.Find(s => s.Type != "none").ToList());
            }
            if (checkedParams.children == "Дети")
                AllPlaces.RemoveAll(s => !s.Kids);

            ViewData["Points_x"] = String.Join(", ", AllPlaces.Select(x => x.Coordinates.Item1).ToList());
            ViewData["Points_y"] = String.Join(", ", AllPlaces.Select(x => x.Coordinates.Item2).ToList());
            ViewData["Points_names"] = String.Join(", ", AllPlaces.Select(x => x.Name).ToList());

            return View("Index");
        }
    }
}
