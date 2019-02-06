using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.HtmlHelpers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
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

            AutosController controller = new AutosController(mock.Object);
            controller.pageSize = 3;
            //Act
            AutoListViewModel result = (AutoListViewModel)controller.List(null, 2).Model;
            //Assert
            List<Auto> autos = result.Autos.ToList();
            Assert.IsTrue(autos.Count == 2);
            Assert.AreEqual(autos[0].Name, "BENTLEY");
            Assert.AreEqual(autos[1].Name, "AUDI");
        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                          + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                          + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());


        }
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
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

            AutosController controller = new AutosController(mock.Object);
            controller.pageSize = 3;
            //Act
            AutoListViewModel result = (AutoListViewModel)controller.List(null, 2).Model;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }
        [TestMethod]
        public void Can_Filter_Autos()
        {
            //Arrange
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            mock.Setup(m => m.Autos).Returns(new List<Auto>
            {
                new Auto{Id = 1,Name = "AUDI",  Type = "Type1"},
                new Auto{Id = 2,Name = "BENTLEY",    Type = "Type2"},
                new Auto{Id = 3,Name = "BMW",     Type = "Type1"},
                new Auto{Id = 4,Name = "FERRARI",   Type = "Type3"},
                new Auto{Id = 5,Name = "HYUNDAI",      Type = "Type2"},
            });

            AutosController controller = new AutosController(mock.Object);
            controller.pageSize = 3;
            //Act
            List<Auto> result = ((AutoListViewModel)controller.List("Type2",1).Model).Autos.ToList();

            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "BENTLEY" && result[0].Type == "Type2");
            Assert.IsTrue(result[1].Name == "AUDI" && result[1].Type == "Type2");
        }
        [TestMethod]
        public void Can_Create_Categories()
        {
            //Arrange
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            mock.Setup(m => m.Autos).Returns(new List<Auto>
            {
                new Auto{Id = 1,Name = "AUDI",  Type = "Type1"},
                new Auto{Id = 2,Name = "BENTLEY",    Type = "Type2"},
                new Auto{Id = 3,Name = "BMW",     Type = "Type1"},
                new Auto{Id = 4,Name = "FERRARI",   Type = "Type3"},
                new Auto{Id = 5,Name = "HYUNDAI",      Type = "Type2"},
            });


            NavController target = new NavController(mock.Object);
            //Act
            List<string> result =((IEnumerable<string>)target.Menu().Model).ToList();

            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0], "Type1");
            Assert.AreEqual(result[1], "Type2");
            Assert.AreEqual(result[2], "Type3");
        }
        [TestMethod]
        public void Indicates_Selected_Type()
        {
            //Arrange
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            mock.Setup(m => m.Autos).Returns(new List<Auto>
            {
                new Auto{Id = 1,Name = "AUDI",  Type = "Type1"},
                new Auto{Id = 2,Name = "BENTLEY",    Type = "Type2"},
                new Auto{Id = 3,Name = "BMW",     Type = "Type1"},
                new Auto{Id = 4,Name = "FERRARI",   Type = "Type3"},
                new Auto{Id = 5,Name = "HYUNDAI",      Type = "Type2"},
            });


            NavController target = new NavController(mock.Object);

            string TypeToSelect = "Type2";
            //Act
            string result = target.Menu(TypeToSelect).ViewBag.SelectedType;

            Assert.AreEqual(TypeToSelect,result);
            
        }
        [TestMethod]
        public void Generate_Auto_Specific_Auto_Count()
        {
            //Arrange
            Mock<IAutoRepository> mock = new Mock<IAutoRepository>();
            mock.Setup(m => m.Autos).Returns(new List<Auto>
            {
                new Auto{Id = 1,Name = "AUDI",  Type = "Type1"},
                new Auto{Id = 2,Name = "BENTLEY",    Type = "Type2"},
                new Auto{Id = 3,Name = "BMW",     Type = "Type1"},
                new Auto{Id = 4,Name = "FERRARI",   Type = "Type3"},
                new Auto{Id = 5,Name = "HYUNDAI",      Type = "Type2"},
            });


            AutosController controller = new AutosController(mock.Object);
            controller.pageSize = 3;

            int res1 = ((AutoListViewModel)controller.List("Type1").Model).PagingInfo.TotalItems;
            int res2 = ((AutoListViewModel)controller.List("Type2").Model).PagingInfo.TotalItems;
            int res3 = ((AutoListViewModel)controller.List("Type3").Model).PagingInfo.TotalItems;
            int resAll = ((AutoListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
