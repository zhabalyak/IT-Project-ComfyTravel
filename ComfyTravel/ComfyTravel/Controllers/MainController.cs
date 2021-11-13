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
        private IMongoCollection<Objects> _commentsCollection;
        private IMongoCollection<Objects> _saveroutesCollection;

        public MainController(IMongoClient client)
        {
            //получение через клиента базы данных и коллекций
            var database = client.GetDatabase("ComfyTravel");

            _objectsCollection = database.GetCollection<Objects>("objects");
            _commentsCollection = database.GetCollection<Objects>("comments");
            _saveroutesCollection = database.GetCollection<Objects>("saveroutes");
        }

        public IActionResult Index()
        {
            //тестовая проверочка работы с бд
            //плюс передача данных с контроллера во вьюшку (там ображение к @ViewData["Testing"])
            ViewData["Testing"] = _objectsCollection.Find(s => s.Type == TypesOfObjects.Park).ToList().Count;

            return View();
        }
    }
}
