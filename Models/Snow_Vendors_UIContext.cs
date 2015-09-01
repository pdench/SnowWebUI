using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Snow_Vendors_UI.Models
{
    public class Snow_Vendors_UIContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Snow_Vendors_UIContext() : base("name=Snow_Vendors_UIContext")
        {
        }

        public System.Data.Entity.DbSet<Snow_Vendors_Model.Models.Vendor> Vendors { get; set; }
    
    }
}
