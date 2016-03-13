﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LST.Models;

namespace LST.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<TestMatch> matches;
            using (var db = new ApplicationDbContext())
            {
                if (!db.Database.Exists())
                {
                    db.Database.Create();
                }
                DateTime time = DateTime.Now;
                matches = db.TestMatches.Where(t => t.Visible == true).ToList();
            }
            return View(matches);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult StudentNumber([Bind(Include = "StudentNumber")]RegisterViewModel model)
        {
            string message = null;
            if (!string.IsNullOrEmpty(model.StudentNumber))
            {
                using (var db = new ApplicationDbContext())
                {
                    if (db.Users.Where(u => u.StudentNumber == model.StudentNumber).Count() != 0)
                    {
                        message = "此学号已经注册。";
                        return Json(message, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                message = "错误的数据。";
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}