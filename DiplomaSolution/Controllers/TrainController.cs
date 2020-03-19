using DiplomaSolution.Models.TrainModels;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class TrainController : Controller
    {
        [HttpGet]
        public IActionResult TrainView()
        {
            var model = new TrainModel();

            return View(model);
        }

        [HttpPost]
        public string TrainView(TrainModel trainModel)
        {
            var x = trainModel;

            return "Done";
        }
    }
}
