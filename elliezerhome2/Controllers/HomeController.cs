using eliezerhome2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace elliezerhome2.Controllers
{
    public class HomeController : Controller
    {
        private PaintingContext db = new PaintingContext();

        public ActionResult Index()
        {

            IEnumerable<Painting> paintings = db.Paintings;
            return View(paintings);
        }

        public ActionResult About()
        {
            IEnumerable<Painting> paintings = db.Paintings;
            ViewBag.Paintings = paintings;



            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}