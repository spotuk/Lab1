using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebUI.Controllers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
   public  class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Books()
        {

            //Arrange
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            mock.Setup(m => m.Autos).Returns(new List<Auto>
            {
                new Auto{Id = 1,Name = "AUDI"},
                new Auto{Id = 2,Name = "BENTLEY"},
                new Auto{Id = 3,Name = "BMW"},
                new Auto{Id = 4,Name = "FERRARI"},
                new Auto{Id = 5,Name = "HYUNDAI"},

            });

            AdminController controller = new AdminController(mock.Object);

            //Act
            List<Auto> result = ((IEnumerable<Auto>)controller.Index().ViewData.Model).ToList();
            //Assert

            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual(result[0].Name, "BENTLEY");
            Assert.AreEqual(result[1].Name, "AUDI");
        }

        [TestMethod]
        public void Can_Edit_Auto()
        {

            //Arrange
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            mock.Setup(m => m.Autos).Returns(new List<Auto>
            {
                new Auto{Id = 1,Name = "AUDI"},
                new Auto{Id = 2,Name = "BENTLEY"},
                new Auto{Id = 3,Name = "BMW"},
                new Auto{Id = 4,Name = "FERRARI"},
                new Auto{Id = 5,Name = "HYUNDAI"},

            });

            AdminController controller = new AdminController(mock.Object);

            //Act
            Auto auto1 = controller.Edit(1).ViewData.Model as Auto;
            Auto auto2 = controller.Edit(2).ViewData.Model as Auto;
            Auto auto3 = controller.Edit(3).ViewData.Model as Auto;
            //Assert
            Assert.AreEqual(1, auto1.Id);
            Assert.AreEqual(2, auto2.Id);
            Assert.AreEqual(3, auto3.Id);

        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Auto()
        {

            //Arrange
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            mock.Setup(m => m.Autos).Returns(new List<Auto>
            {
                new Auto{Id = 1,Name = "AUDI"},
                new Auto{Id = 2,Name = "BENTLEY"},
                new Auto{Id = 3,Name = "BMW"},
                new Auto{Id = 4,Name = "FERRARI"},
                new Auto{Id = 5,Name = "HYUNDAI"},

            });

            AdminController controller = new AdminController(mock.Object);

            //Act
            Auto result = controller.Edit(7).ViewData.Model as Auto;

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            AdminController controller = new AdminController(mock.Object);

            Auto auto = new Auto { Name = "Test" };

            ActionResult result = controller.Edit(auto);

            mock.Verify(m=>m.SaveAuto(auto));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Save_IValid_Changes()
        {
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            AdminController controller = new AdminController(mock.Object);

            Auto auto = new Auto { Name = "Test" };

            controller.ModelState.AddModelError("error","error");

            ActionResult result = controller.Edit(auto);

            mock.Verify(m => m.SaveAuto(It.IsAny<Auto>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
