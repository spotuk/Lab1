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
    public class CartController : Controller
    {
        private IAutoRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IAutoRepository repo,IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }
        public ViewResult Index(Cart cart,string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnURL = returnUrl
            });
        }
       
        public RedirectToRouteResult AddToCart(Cart cart,int autoID,string returnURL)
        {
            Auto auto = repository.Autos
                .FirstOrDefault(b => b.Id == autoID);

            if(auto != null)
            {
                cart.AddItem(auto, 1);
            }
            return RedirectToAction("Index", new { returnURL });
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart,int autoID, string returnURL)
        {
            Auto auto = repository.Autos.FirstOrDefault(b => b.Id == autoID);

            if (auto != null)
            {
                cart.RemoveLine(auto);
            }
            return RedirectToAction("Index", new { returnURL });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public ViewResult CheckOut()
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ViewResult CheckOut(Cart cart, ShippingDetails shippingDetails)
        {
            if(cart.Lines.Count() ==0)
            {
                ModelState.AddModelError("", "Извините,корзина пуста!!");
            }
            if(ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(new ShippingDetails());
            }
            
        }

    }
}