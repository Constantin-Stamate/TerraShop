using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic;
using AutoMapper;
using eUseControl.Domain.Entities.Product;
using System.Collections.Generic;
using eUseControl.Web.Models.Product;

namespace eUseControl.Web.Controllers
{
    public class MainController : BaseController
    {
        private readonly IProduct _product;

        public MainController()
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
        }

        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Navbar()
        {
            var categoryProductCounts = _product.GetCategoryProductCounts();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductSummary, ProductMini>();
                cfg.CreateMap<Category, ProductCategory>();
            });

            var mapper = config.CreateMapper();
            var productCountsByCategory = mapper.Map<Dictionary<ProductCategory, int>>(categoryProductCounts);

            var model = new ProductCatalogViewModel
            {
                Categories = productCountsByCategory
            };

            return PartialView("_Navbar", model);
        }

        [HttpGet]
        public ActionResult Error404(bool? error)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}