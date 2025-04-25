using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IProduct _product;

        public ProfileController()
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
        }

        // GET: Profile
        public ActionResult GeneralProfile()
        {
            return View();
        }

        // GET: ArticlesProfile
        [HttpGet]
        public ActionResult ArticlesProfile()
        {
            var cookie = Request.Cookies["X-KEY"].Value;
            if (string.IsNullOrEmpty(cookie))
            {
                TempData["ErrorMessage"] = "You are not authenticated!";
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var productsMinimals = _product.GetProductsByUser(cookie);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductMinimal, ProductCompact>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                   .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.ProductDescription))
                   .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.ProductPrice))
                   .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.ProductImageUrl));
            });

            var mapper = config.CreateMapper();
            var products = mapper.Map<List<ProductCompact>>(productsMinimals);

            return View(products);
        }

        // GET: ChangePasswordProfile
        public ActionResult ChangePasswordProfile()
        {
            return View();
        }

        // GET: PurchaseHistoryProfile
        public ActionResult PurchaseHistoryProfile()
        {
            return View();
        }

        // GET: SalesProfile
        public ActionResult SalesProfile()
        {
            return View();
        }


        // GET: SettingsProfile
        public ActionResult SettingsProfile()
        {
            return View();
        }
    }
}