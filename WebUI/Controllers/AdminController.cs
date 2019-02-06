using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        IAutoRepository repository;

        public AdminController(IAutoRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            if(HttpContext.User.Identity.Name.ToUpper() == "ADMIN")
            return View(repository.Autos);
            return View();
        }

        public ViewResult Edit(int Id)
        {
            Auto auto = repository.Autos.FirstOrDefault(b => b.Id == Id);
            return View(auto);
        }
        [HttpPost]
        public ActionResult Edit(Auto auto, HttpPostedFileBase image = null)
        {
            if(ModelState.IsValid)
            {
                if (image != null)
                {
                    auto.ImageMimeType = image.ContentType;
                    auto.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(auto.ImageData, 0, image.ContentLength);
                }

                repository.SaveAuto(auto);
                TempData["message"] = string.Format("Изменения информации о авто \"{0}\" сохранены", auto.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(auto);
            }
        }
        public ViewResult Create()
        {
            return View("Edit", new Auto());
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Auto deletedAuto = repository.DeleteAuto(Id);
            if (deletedAuto != null)
            {
                TempData["message"] = string.Format("Товар \"{0}\" была удалена",
                    deletedAuto.Name);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Inside(User user)
        {
            if(user.Email.ToUpper() == "ADMIN")
            return RedirectToAction("AdminPanel","Admin");

            return RedirectToAction("List","Autos");
        }
    }
}