using System;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using Moq;
using Domain.Abstract;
using WebUI.Controllers;
using System.Web.Mvc;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //org
            Auto auto1 = new Auto { Id = 1, Name = "Auto1" };
            Auto auto2 = new Auto { Id = 2, Name = "Auto2" };
            //act
            Cart cart = new Cart();
            cart.AddItem(auto1, 1);
            cart.AddItem(auto2, 1);

            List<CartLine> results = cart.Lines.ToList();
            //утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Auto, auto1);
            Assert.AreEqual(results[1].Auto, auto2);
        }
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //org
            Auto auto1 = new Auto { Id = 1, Name = "Auto1" };
            Auto auto2 = new Auto { Id = 2, Name = "Auto2" };
            //act
            Cart cart = new Cart();
            cart.AddItem(auto1, 1);
            cart.AddItem(auto2, 1);
            cart.AddItem(auto1, 5);

            List<CartLine> results = cart.Lines.OrderBy(c => c.Auto.Id).ToList();
            //утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //org
            Auto auto1 = new Auto { Id = 1, Name = "Auto1" };
            Auto auto2 = new Auto { Id = 2, Name = "Auto2" };
            Auto auto3 = new Auto { Id = 3, Name = "Auto3" };
            //act
            Cart cart = new Cart();
            cart.AddItem(auto1, 1);
            cart.AddItem(auto2, 1);
            cart.AddItem(auto1, 5);
            cart.AddItem(auto3, 2);

            cart.RemoveLine(auto2);
            //утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Auto == auto2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //org
            Auto auto1 = new Auto { Id = 1, Name = "Auto1", Price = 100 };
            Auto auto2 = new Auto { Id = 2, Name = "Auto2", Price = 55 };

            //act
            Cart cart = new Cart();
            cart.AddItem(auto1, 1);
            cart.AddItem(auto2, 1);
            cart.AddItem(auto1, 5);

            decimal result = cart.ComputeTotalValue();
            //утверждение
            Assert.AreEqual(result, 655);
        }
        [TestMethod]
        public void Can_Clear_Contents()
        {
            //org
            Auto auto1 = new Auto { Id = 1, Name = "Auto1", Price = 100 };
            Auto auto2 = new Auto { Id = 2, Name = "Auto2", Price = 55 };

            //act
            Cart cart = new Cart();
            cart.AddItem(auto1, 1);
            cart.AddItem(auto2, 1);
            cart.AddItem(auto1, 5);

            cart.Clear();
            //утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            mock.Setup(m => m.Autos).Returns(new List<Auto>{
                new Auto { Id = 1, Name = "Auto1", Type = "Type1"} }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object, null);

            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Auto.Id, 1);

        }
        [TestMethod]
        public void Adding_Book_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            mock.Setup(m => m.Autos).Returns(new List<Auto>{
                new Auto { Id = 1, Name = "Auto1", Type = "Type1"} }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object,null);

            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"],"Index");
            Assert.AreEqual(result.RouteValues["returnUrl"],"myUrl");

        }

        [TestMethod]
        public void Can_View_cart_Contents()
        {
            Cart cart = new Cart();

            CartController target = new CartController(null,null);

            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnURL, "myUrl");

        }

        [TestMethod]
        public void Cannot_Check_Checkout_Empty_cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();

            CartController controller = new CartController(null, mock.Object);

            ViewResult result = controller.CheckOut(cart, shippingDetails);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Check_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Auto(), 1);

            CartController controller = new CartController(null, mock.Object);
            controller.ModelState.AddModelError("error", "error");

            ViewResult result = controller.CheckOut(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Check_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Auto(), 1);

            CartController controller = new CartController(null, mock.Object);
          

            ViewResult result = controller.CheckOut(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
    }

