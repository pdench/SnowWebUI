using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Snow_Vendors_UI.ViewModels
{
    public class VendorViewModel
    {
        public int Id { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ValidFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ValidThru { get; set; }
    }
}