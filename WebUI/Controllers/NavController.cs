using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private IAutoRepository repository;
        public NavController(IAutoRepository repo)
        {
            repository = repo;
        }

       public PartialViewResult Menu(string type = null)
        {
            ViewBag.SelectedType = type;

            IEnumerable<string> types = repository.Autos
                .Select(auto => auto.Type)
                .Distinct()
                .OrderBy(x => x);

            return PartialView(types);
        }
    }
}