using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Snow_Vendors_Model.Models;
using Snow_Vendors_UI.ViewModels;

namespace Snow_Vendors_UI.Controllers
{
    public class VendorController : Controller
    {
        protected string dashboardUrlBase = "http://localhost:8099/api/vendors";
        // azure link: http://productssample.azurewebsites.net/api/products
        //Product prodList;

        // GET: Products
        public ActionResult Index()
        {

            IEnumerable<Vendor> vendList;

            WebClient client = new WebClient();

            string output = client.DownloadString(dashboardUrlBase);
            vendList = JsonConvert.DeserializeObject<List<Vendor>>(output);

            VendorListViewModel vendorList = new VendorListViewModel();
            List<VendorViewModel> vendVMs = new List<VendorViewModel>();

            foreach (Vendor v in vendList)
            {
                VendorViewModel vvm = new VendorViewModel();
                
                vvm.VendorName = v.VendorName;
                vvm.VendorCode = v.VendorCode;
                vvm.ValidFrom = v.ValidFrom;
                vvm.ValidThru = v.ValidThru;
                vendVMs.Add(vvm);
            };
            vendorList.Vendors = vendVMs;
            
            return View("Index",vendorList);
        }
    }
}