using ComfyTravel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public IActionResult Index()
        {
            //тестовая проверочка работы с бд
            //плюс передача данных с контроллера во вьюшку (там ображение к @ViewData["Testing"])
            ViewData["Testing"] = _objectsCollection.Find(s => s.Type == TypesOfObjects.Park).ToList().Count;

            return View();
        }

        [HttpPost]
        public Tuple<List<Tuple<double, double>>, List<string>> Index(bool children)
        {
            Tuple<List<Tuple<double, double>>, List<string>> data;

            if (true)
            {
                var data_all = _objectsCollection.Find(s => s.Kids).ToList();
                List<string> data_names = data_all.Select(s => s.Name).ToList();
                List<Tuple<double, double>>  data_coords = data_all.Select(s => s.Coordinates).ToList();
                data = new Tuple<List<Tuple<double, double>>, List<string>>(data_coords, data_names);

                //ViewBag.JavaScriptFunction = string.Format("SetNewPoints('{0}');", data);
            }

            return data;
        }
    }
}
