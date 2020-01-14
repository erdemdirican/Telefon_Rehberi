using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telefon_Rehberi.Database;

namespace Telefon_Rehberi.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected RehberDbEntities context { get; private set; }
        public static string logonUserName { get; set; }
        public BaseController()
        {
            context = new RehberDbEntities();
        }
    }
}