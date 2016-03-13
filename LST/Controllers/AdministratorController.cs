﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LST.Models;
using System.Data.Entity.Infrastructure;

namespace LST.Controllers
{
    public class AdministratorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Administrator
        public ActionResult Index(string message="")
        {
            return View(db.TestMatches.ToList());
        }

        // GET: Administrator/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestMatch testMatch = db.TestMatches.Find(id);
            if (testMatch == null)
            {
                return HttpNotFound();
            }
            return View(testMatch);
        }

        // GET: Administrator/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrator/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Limit,StartTime,EndTime,Visible")] TestMatch testMatch)
        {
            if (ModelState.IsValid)
            {
                testMatch.Id = Guid.NewGuid();
                db.TestMatches.Add(testMatch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(testMatch);
        }

        // GET: Administrator/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestMatch testMatch = db.TestMatches.Find(id);
            if (testMatch == null)
            {
                return HttpNotFound();
            }
            return View(testMatch);
        }

        // POST: Administrator/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Limit,StartTime,EndTime,Visible,RowVersion")] TestMatch testMatch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testMatch).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();

                    return RedirectToAction("Index", new { message="UpdateError"});
                }
                return RedirectToAction("Index");
            }
            return View(testMatch);
        }

        // GET: Administrator/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestMatch testMatch = db.TestMatches.Find(id);
            if (testMatch == null)
            {
                return HttpNotFound();
            }
            return View(testMatch);
        }

        // POST: Administrator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TestMatch testMatch = db.TestMatches.Find(id);
            db.TestMatches.Remove(testMatch);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GenerateMatches()
        {
            return View(new GenerateMatchesViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateMatches(GenerateMatchesViewModel model)
        {
            if (ModelState.IsValid)
            {
                TestHelper helper = new TestHelper();
                bool result = helper.GenerateMatches(model.Days.Split('|'), model.Lessons.Split('|'), model.Limit, model.StartTime, model.EndTime);
                if (result)
                    return RedirectToAction("Index", new { message = "success" });
                else
                {
                    ViewBag.Error = "创建失败";
                    return View(model);
                }
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public enum MessageType
        {
            Success,
            Failed,
            DbUpdateFailed
        }
    }
}
