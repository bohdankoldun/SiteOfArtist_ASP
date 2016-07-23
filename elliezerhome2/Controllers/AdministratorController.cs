
using eliezerhome2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;

namespace eliezerhome2.Controllers
{
    public class AdministratorController : Controller
    {
        private PaintingContext db = new PaintingContext();

        // GET: Administrator
        public ActionResult Index()
        {

            IEnumerable<Painting> paintings = db.Paintings;
            return View(paintings);
        }

        [HttpGet]
        public ActionResult AddPainting()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddPainting(Painting new_painting, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                //saving file of image
                string path = "", path_for_html = "", path_for_small_img ="";
                path = Server.MapPath("~/Paintings/") + new_painting.Name + "_" + Path.GetFileName(uploadImage.FileName);
                path_for_html =   new_painting.Name + "_" + Path.GetFileName(uploadImage.FileName);
                path_for_small_img = Server.MapPath("~/Paintings/") +"s_"+ new_painting.Name + "_" + Path.GetFileName(uploadImage.FileName);
                uploadImage.SaveAs(path);



                //write to db url of image
                new_painting.URL = path_for_html;

                db.Paintings.Add(new_painting);
                db.SaveChanges();


                Bitmap big_painting = new Bitmap(path);
                int width = big_painting.Width / (big_painting.Height / 150);
                Bitmap small_painting = new Bitmap(big_painting, width, 150);
                small_painting.Save(path_for_small_img);



                return RedirectToAction("Index");
            }

            return View(new_painting);

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