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
            //тестовая проверочка работы с бд
            //плюс передача данных с контроллера во вьюшку (там ображение к @ViewData["Testing"])
            //ViewData["Testing"] = _objectsCollection.Find(s => s.Type == TypesOfObjects.Park).ToList().Count;
            //ViewData["Testing"] = _objectsCollection.Find(s => s.Type != "none").ToList().Count;

            //List<Objects> AllPlaces = _objectsCollection.Find(s => s.Type != "none").ToList();

            //ViewData["Params"] = RouteGenerationModule.MainGenerate(
            //    AllPlaces, new List<Objects>() { AllPlaces[15]},
            //    new List<bool>() { false, true, false, true, false }, true,
            //    new DateTime(DateTime.Now.Year, DateTime.Now.Month, 27, 12, 45, 0), 
            //    new TimeSpan(5, 5, 0), 
            //    TypesOfTransport.Public);


            //if (!Request.Url.AbsoluteUri.EndsWith("Main/Index"))
            //{
            //    return RedirectToAction("Main", "Index");

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

            var points = AllPlaces.Select(x => new { x.Name, x.Coordinates.Item1, x.Coordinates.Item2 }).ToList();

            //ViewData["Params"] = String.Join(" ", AllPlaces.Select(s => s.Type).ToArray());

            var json = JsonSerializer.Serialize(points);

            ViewData["Params"] = json.ToString();

            ViewData["Points_x"] = String.Join(", ", AllPlaces.Select(x => (double)x.Coordinates.Item1).ToList());
            ViewData["Points_y"] = String.Join(", ", AllPlaces.Select(x => x.Coordinates.Item2).ToList());
            ViewData["Points_names"] = String.Join(", ", AllPlaces.Select(x => x.Name).ToList());

            ViewBag.JavaScriptFunction = string.Format("ShowGreetings('{0}');", json);
            //ViewBag.JavaScriptFunction = "take_points([[[55.806059, 49.177076], [55.811681, 49.100693]], [\"точка 1\", \"точка 2\"]]);";

            return View("Index");

            //return ViewData["Params"].ToString();
        }
    }
}
