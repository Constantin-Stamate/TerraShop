using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Profile;
using eUseControl.Web.Models.Product;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IProduct _product;
        private readonly ISession _session;
        private readonly IProfile _profile;

        public ProfileController()
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
            _session = bl.GetSessionBL();
            _profile = bl.GetProfileBL();
        }

        [HttpGet]
        public ActionResult GeneralProfile()
        {
            var cookie = Request.Cookies["X-KEY"].Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var user = _session.GetUserByCookie(cookie);
            if (user == null)
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var profile = _profile.GetProfileByUserId(user.Id);
            if (profile == null)
            {
                return RedirectToAction("Index", "Main", new { error = true });
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProfileData, ProfileCompact>();
            });

            var mapper = config.CreateMapper();
            var userProfile = mapper.Map<ProfileCompact>(profile);
            
            return View(userProfile);
        }

        [HttpGet]
        public ActionResult ArticlesProfile()
        {
            var cookie = Request.Cookies["X-KEY"].Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var user = _session.GetUserByCookie(cookie);
            if (user == null)
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var productsMinimals = _product.GetProductsByUserId(user.Id);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductMinimal, ProductCompact>();
            });

            var mapper = config.CreateMapper();
            var products = mapper.Map<List<ProductCompact>>(productsMinimals);

            return View(products);
        }

        [HttpGet]
        public ActionResult ChangePasswordProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordProfile(string currentPassword, string newPassword)
        {
            if (ModelState.IsValid)
            {
                var cookie = Request.Cookies["X-KEY"].Value;
                if (string.IsNullOrEmpty(cookie))
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var user = _session.GetUserByCookie(cookie);
                if (user == null)
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var result = _profile.ChangePassword(currentPassword, newPassword, user.Id);

                if (result.Status)
                {
                    TempData["SuccessMessage"] = result.StatusMsg;
                    return RedirectToAction("ChangePasswordProfile", "Profile", new { success = true });
                }
                else
                {
                    ModelState.AddModelError("", result.StatusMsg);
                    TempData["ErrorMessage"] = result.StatusMsg;
                    return RedirectToAction("ChangePasswordProfile", "Profile", new { error = true });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "The passwords you entered are invalid!";
                return RedirectToAction("ChangePasswordProfile", "Profile", new { error = true });
            }
        }

        [HttpGet]
        public ActionResult PurchaseHistoryProfile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SalesProfile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SettingsProfile()
        {
            var cookie = Request.Cookies["X-KEY"].Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var user = _session.GetUserByCookie(cookie);
            if (user == null)
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var profile = _profile.GetProfileByUserId(user.Id);
            if (profile == null)
            {
                return RedirectToAction("Index", "Main", new { error = true });
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProfileData, ProfileCompact>();
            });

            var mapper = config.CreateMapper();
            var userProfile = mapper.Map<ProfileCompact>(profile);

            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SettingsProfile(ProfileCompact profileCompact, HttpPostedFileBase profileImage)
        {
            if (ModelState.IsValid)
            {
                if (profileImage != null && profileImage.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(profileImage.FileName)?.ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["ErrorMessage"] = "Invalid image format!";
                        return RedirectToAction("SettingsProfile", "Profile", new { error = true });
                    }

                    string fileName = Path.GetFileName(profileImage.FileName);
                    string uploadsPath = Server.MapPath("~/Uploads/avatars/");

                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    string filePath = Path.Combine(uploadsPath, fileName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            profileImage.SaveAs(filePath);
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = "Oops! Couldn't save the image: " + ex;
                            return RedirectToAction("SettingsProfile", "Profile", new { error = true });
                        }
                    }

                    profileCompact.ProfileImageUrl = "~/Uploads/avatars/" + fileName;
                }

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ProfileCompact, ProfileData>();
                });

                var mapper = config.CreateMapper();
                var profile = mapper.Map<ProfileData>(profileCompact);

                var cookie = Request.Cookies["X-KEY"].Value;
                if (string.IsNullOrEmpty(cookie))
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var user = _session.GetUserByCookie(cookie);
                if (user == null)
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var result = _profile.UpdateProfile(user.Id, profile);

                if (result.Status)
                {
                    return RedirectToAction("GeneralProfile", "Profile", new { success = true });
                }
                else
                {
                    ModelState.AddModelError("", result.StatusMsg);
                    TempData["ErrorMessage"] = result.StatusMsg;
                    return RedirectToAction("SettingsProfile", "Profile", new { error = true });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "The model you submitted is invalid!";
                return RedirectToAction("SettingsProfile", "Profile", new { error = true });
            }
        }
    }
}