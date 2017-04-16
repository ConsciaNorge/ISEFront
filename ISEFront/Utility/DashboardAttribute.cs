using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISEFront.Utility
{
    public class DashboardAttribute : Attribute
    {
        public string Title { get; set; }
    }
}