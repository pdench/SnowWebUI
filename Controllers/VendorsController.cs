using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Snow_Vendors_Model.Models;
using Snow_Vendors_UI.ViewModels;
using Newtonsoft.Json;
using Snow_Vendors_UI.Models;
using Snow_Vendors_UI.Misc;

namespace Snow_Vendors_UI.Controllers
{
    public class VendorsController : Controller
    {
        protected string dashboardUrlBase = "http://localhost:8099/api/vendors";

        Snow_Vendors_UIContext db = new Snow_Vendors_UIContext();
        
        // GET: Vendors
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            IEnumerable<Vendor> vendList;

            WebClient client = new WebClient();

            string output = client.DownloadString(dashboardUrlBase);
            vendList = JsonConvert.DeserializeObject<List<Vendor>>(output);

            if (String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "name";
            };

            string inSortOrder = sortOrder;

            if (Session != null)
            {
                if (Session["sort"] == null)
                {
                    Session["sort"] = "";
                }
                if (Session["sortDir"] == null) {
                    Session["sortDir"] = "asc";
                }
            }
            string sortDir = Session["sortDir"].ToString();

            if (sortOrder == Session["sort"].ToString())
            {
                sortDir = sortDir == "asc" ? "desc" : "asc";
            }
            else
            {
                sortDir = "asc";
            }

            Session["sortDir"] = sortDir;
            Session["sort"] = inSortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var vendors = from v in vendList
                          select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                vendors = vendors.Where(v => v.VendorName.ToLower().Contains(searchString.ToLower()));
            }
            switch (sortOrder+sortDir)
            {
                case "namedesc":
                    vendors = vendors.OrderByDescending(v => v.VendorName);
                    break;
                case "codedesc":
                    vendors = vendors.OrderByDescending(v => v.VendorCode);
                    break;
                case "codeasc":
                    vendors = vendors.OrderBy(v => v.VendorCode);
                    break;
                case "fromasc":
                    vendors = vendors.OrderBy(v => v.ValidFrom);
                    break;
                case "fromdesc":
                    vendors = vendors.OrderByDescending(v => v.ValidFrom);
                    break;
                case "thruasc":
                    vendors = vendors.OrderBy(v => v.ValidThru);
                    break;
                case "thrudesc":
                    vendors = vendors.OrderByDescending(v => v.ValidThru);
                    break;
                default:
                    vendors = vendors.OrderBy(v => v.VendorName);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            
            VendorListViewModel vendorList = new VendorListViewModel();
            List<VendorViewModel> vendVMs = new List<VendorViewModel>();

            foreach (Vendor v in vendors)
            {
                VendorViewModel vvm = new VendorViewModel();

                vvm.VendorName = v.VendorName;
                vvm.VendorCode = v.VendorCode;
                vvm.ValidFrom = v.ValidFrom;
                vvm.ValidThru = v.ValidThru;
                vvm.Id = v.Id;
                vendVMs.Add(vvm);
            };
            vendorList.Vendors = vendVMs;


            return View("Index", vendorList);
        }

        // GET: Vendors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: Vendors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VendorCode,VendorName,ValidFrom,ValidThru")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Vendors.Add(vendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VendorCode,VendorName,ValidFrom,ValidThru")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendor vendor = db.Vendors.Find(id);
            db.Vendors.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

           