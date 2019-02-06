using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AutosController : Controller
    {
        public string Index()
        {
            string result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                result = "Ваш логин: " + User.Identity.Name;
            }
            return result;
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }
        public int pageSize = 4;
        private readonly IAutoRepository repository;
        public AutosController(IAutoRepository repo)
        {
            repository = repo;

        }
        public ViewResult List(string type, string searchString, int page = 1)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                AutoListViewModel model = new AutoListViewModel
                {
                    Autos = repository.Autos
                .Where(b => type == null || b.Type == type)
                .Where(b => b.Name.Contains(searchString))
                .OrderBy(auto => auto.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = type == null ?
                     repository.Autos.Count() :
                     repository.Autos.Where(auto => auto.Type == type).Count()
                    },
                    CurrentType = type
                };

                return View(model);
            }
            else
            {
                AutoListViewModel model = new AutoListViewModel
                {
                    Autos = repository.Autos
                    .Where(p => type == null || p.Type == type)
                    .OrderBy(auto => auto.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = type == null ?
                    repository.Autos.Count() :
                    repository.Autos.Where(auto => auto.Type == type).Count()
                    },
                    CurrentType = type
                };
                return View(model);
            }

            
        }
        public FileContentResult GetImage(int Id)
        {
            Auto auto = repository.Autos
                .FirstOrDefault(g => g.Id == Id);

            if (auto != null)
            {
                return File(auto.ImageData, auto.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}