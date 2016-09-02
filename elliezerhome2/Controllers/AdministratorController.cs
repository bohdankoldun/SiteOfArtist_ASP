using eliezerhome2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;

namespace eliezerhome2.Controllers
{
    [Authorize]
    public class AdministratorController : Controller
    {
        //создаем обьекты контекста для связи з бд
        private WorkContext dbWorks = new WorkContext();
        private GalleryContext dbGalleries = new GalleryContext();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Works()
        {
            @ViewBag.Title = "List of works";
            IList<Work> works = dbWorks.Works.ToList();
            return View("~/Views/Administrator/Work/Works.cshtml", works);
        }


        #region методы добавления работы

        [HttpGet]
        public ActionResult AddWork()
        {
            @ViewBag.Title = "Add work";

            return View("~/Views/Administrator/Work/AddWork.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddWork(Work new_work, IList<HttpPostedFileBase> uploadImages)
        {
            if (ModelState.IsValid && uploadImages[0] != null)
            {
                string path = "", path_for_html = "";

                foreach (var image in uploadImages)
                {
                    if (image == null) continue;

                    path = Server.MapPath("~/Paintings/") + new_work.Name + "_" + Path.GetFileName(image.FileName);
                    image.SaveAs(path);

                    path_for_html = new_work.Name + "_" + Path.GetFileName(image.FileName);
                    dbWorks.Photos.Add(new WorkPhoto { URL = path_for_html, Work = new_work });

                }

                dbWorks.Works.Add(new_work);
                dbWorks.SaveChanges();

                return RedirectToAction("Works");
            }

            ViewBag.Message = "Your data are not valid! Please, correct and add photos!";
            @ViewBag.Title = "Add work";

            return View("~/Views/Administrator/Work/AddWork.cshtml", new_work);
        }

        #endregion


        #region редактирование работ

        [HttpGet]
        public ActionResult EditWork(int id)
        {
            //находим редактируемую работу и передаем ее у представление
            Work work = dbWorks.Works.Find(id);

            @ViewBag.Title = "Edit work";

            return View("~/Views/Administrator/Work/EditWork.cshtml", work);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWork(Work edited_work, IList<HttpPostedFileBase> uploadImages)
        {
            //find edited work
            Work edited_work_db = dbWorks.Works.Find(edited_work.WorkId);

            if (ModelState.IsValid && edited_work_db != null)
            {
                //change field of the work
                edited_work_db.Name = edited_work.Name;
                edited_work_db.Date = edited_work.Date;
                edited_work_db.Comment = edited_work.Comment;
                edited_work_db.Price = edited_work.Price;
                edited_work_db.IsHome = edited_work.IsHome;
                edited_work_db.IsPainting = edited_work.IsPainting;

                //add new photos
                string path = "", path_for_html = "";
                foreach (var image in uploadImages)
                {
                    if (image == null) continue;

                    path = Server.MapPath("~/Paintings/") + edited_work.Name + "_" + Path.GetFileName(image.FileName);
                    image.SaveAs(path);

                    path_for_html = edited_work.Name + "_" + Path.GetFileName(image.FileName);
                    dbWorks.Photos.Add(new WorkPhoto { URL = path_for_html, Work = edited_work_db });

                }

                //save changes
                dbWorks.SaveChanges();

                return RedirectToAction("Works");
            }

            ViewBag.Message = "Your data are not valid! Please, correct!";
            @ViewBag.Title = "Edit work";

            return View("~/Views/Administrator/Work/EditWork.cshtml", edited_work);
        }

        /// <summary>
        /// для удаления фото изменяемой работы
        /// </summary>
        /// <param name="id"> id  изменяемой работы</param>
        /// <param name="url"> массив url фото, которые мы удаляем</param>
        [HttpPost]
        public ActionResult DeleteWorkPhotos(int id, string[] url)
        {
            var work = dbWorks.Works.Find(id);


            if (url != null)
            {

                foreach (string scr in url)
                {
                    foreach (WorkPhoto p in work.Photos)
                    {
                        if (work.Photos.Count == 1)
                        {
                            ViewBag.Message = "Потрібно, щоб залишилось одне фото!";
                            return PartialView("~/Views/Administrator/Work/DeleteWorkPhotos.cshtml", work.Photos);
                        }
                        if (("/Paintings/" + p.URL) == scr)
                        {
                            string path = Server.MapPath("~/Paintings/") + p.URL;
                            System.IO.File.Delete(path);
                            dbWorks.Photos.Remove(p);
                            dbWorks.SaveChanges();
                            break;
                        }
                    }
                }

            }

            return PartialView("~/Views/Administrator/Work/DeleteWorkPhotos.cshtml", work.Photos);
        }

        #endregion


        #region методы удаления работы

        public ActionResult DeleteWorkShow(int id)
        {
            //находим удаляемую работу и передаем ее у представление
            Work work = dbWorks.Works.Find(id);

            return View("~/Views/Administrator/Work/DeleteWorkShow.cshtml", work);
        }


        public ActionResult DeleteWorkDo(int id)
        {
            //строка результата удаления
            string result = "";

            //находим удаляему работу и url всех фото к ней
            Work work = dbWorks.Works.Find(id);
            IEnumerable<WorkPhoto> photos = dbWorks.Photos.Where(p => p.WorkId == id).ToList();

            if (work != null)
            {
                //удаляем фото
                foreach (WorkPhoto p in photos)
                {
                    string path = Server.MapPath("~/Paintings/") + p.URL;
                    System.IO.File.Delete(path);
                }

                //удаляем саму работу из бд
                dbWorks.Works.Remove(work);
                dbWorks.SaveChanges();
                result = "Congrats! Work was deleted!";

            }
            else
                result = "The work are not founded!";

            //записуем строку результата в ViewBag
            ViewBag.result = result;

            return View("~/Views/Administrator/Work/DeleteWorkDo.cshtml");
        }

        #endregion


        public ActionResult Galleries()
        {
            @ViewBag.Title = "List of galleris";
            IList<Gallery> galleris = dbGalleries.Galleries.ToList();
            return View("~/Views/Administrator/Gallery/Galleries.cshtml", galleris);
        }


        #region методы добавления, редактирования, удаления галерей

        [HttpGet]
        public ActionResult AddGallery()
        {
            @ViewBag.Title = "Add gallery";

            return View("~/Views/Administrator/Gallery/AddGallery.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGallery(Gallery new_gallery, IList<HttpPostedFileBase> uploadImages)
        {
            if (ModelState.IsValid && uploadImages[0] != null)
            {
                string path = "", path_for_html = "";

                foreach (var image in uploadImages)
                {
                    if (image == null) continue;

                    path_for_html = new_gallery.Name + "_" + DateTime.Now.Millisecond + Path.GetFileName(image.FileName);

                    path = Server.MapPath("~/GalleryPhotos/") + path_for_html;
                    image.SaveAs(path);


                    dbGalleries.Photos.Add(new GalleryPhoto { URL = path_for_html, Comment = "cv", Gallery = new_gallery });

                }

                dbGalleries.Galleries.Add(new_gallery);
                dbGalleries.SaveChanges();

                return RedirectToAction("Galleries");
            }

            ViewBag.Message = "Your data are not valid! Please, correct and add photos!";
            @ViewBag.Title = "Add gallery";

            return View("~/Views/Administrator/Gallery/AddGallery.cshtml", new_gallery);
        }

        [HttpGet]
        public ActionResult EditGallery(int id)
        {
            //находим редактируемую галерею и передаем ее у представление
            Gallery gallery = dbGalleries.Galleries.Find(id);

            @ViewBag.Title = "Edit gallery";

            return View("~/Views/Administrator/Gallery/EditGallery.cshtml", gallery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGallery(Gallery edited_gallery, string[] url, IList<HttpPostedFileBase> uploadImages)
        {
            //find edited gallery
            Gallery edited_gallery_db = dbGalleries.Galleries.Find(edited_gallery.GalleryId);

            if (ModelState.IsValid && edited_gallery_db != null)
            {
                //change name of tht gallery
                edited_gallery_db.Name = edited_gallery.Name;

                //add new photos
                foreach (var image in uploadImages)
                {
                    if (image == null) continue;

                    string path_for_html = edited_gallery.Name + "_" + DateTime.Now.Millisecond + Path.GetFileName(image.FileName);
                    string path = Server.MapPath("~/GalleryPhotos/") + path_for_html;
                    image.SaveAs(path);

                    dbGalleries.Photos.Add(new GalleryPhoto { URL = path_for_html, Comment = path, Gallery = edited_gallery_db });

                }

                //save changes
                dbGalleries.SaveChanges();
                ViewBag.Message = "Зміни збереженні!";

                //delete chosen photos
                if (url != null)
                {

                    foreach (string scr in url)
                    {
                        foreach (GalleryPhoto p in edited_gallery_db.Photos)
                        {
                            if (edited_gallery_db.Photos.Count == 1)
                            {
                                ViewBag.MessageDelPhotos = "Потрібно, щоб залишилось одне фото!";
                                return View("~/Views/Administrator/Gallery/EditGallery.cshtml", edited_gallery_db);
                            }
                            if (("/GalleryPhotos/" + p.URL) == scr)
                            {
                                try
                                {
                                    string path = Server.MapPath("~/GalleryPhotos/") + p.URL;
                                    System.IO.File.Delete(path);
                                    dbGalleries.Photos.Remove(p);
                                    dbGalleries.SaveChanges();
                                    break;
                                }
                                catch(Exception e)
                                {
                                    break;
                                }
                               
                            }
                        }
                    }

                }
            }

            @ViewBag.Title = "Edit gallery";
            return View("~/Views/Administrator/Gallery/EditGallery.cshtml", edited_gallery_db);
        }

        public ActionResult DeleteGallery(int id)
        {

            Gallery gallery = dbGalleries.Galleries.Find(id);

            return View("~/Views/Administrator/Gallery/DeleteGallery.cshtml", gallery);
        }

        public ActionResult DeleteGalleryDo(int id)
        {
            //находим удаляему работу и url всех фото к ней
            Gallery gallery = dbGalleries.Galleries.Find(id);
            IEnumerable<GalleryPhoto> photos = dbGalleries.Photos.Where(p => p.GalleryId == id).ToList();

            if (gallery != null)
            {
                //удаляем фото
                foreach (GalleryPhoto p in photos)
                {
                    string path = Server.MapPath("~/GalleryPhotos/") + p.URL;
                    System.IO.File.Delete(path);
                }

                //удаляем саму работу из бд
                dbGalleries.Galleries.Remove(gallery);
                dbGalleries.SaveChanges();

            }

            return RedirectToAction("Galleries");
        }


        #endregion


        #region редактирование информации об художнике!

        [HttpGet]
        public ActionResult EditArtist()
        {
            //создаём объект модели artist
            Artist eli_ezer = new Artist();

            //загружаем документ xml
            string path_xml = Server.MapPath("~/App_Data/") + "Artist.xml";
            XDocument xDoc = XDocument.Load(path_xml);

            // получаем корневой элемент
            XElement xArtist = xDoc.Element("artist");

            //получаем текст biography
            if (xArtist.Elements("biography").ToList().Count != 0)
                eli_ezer.biography = xArtist.Elements("biography").ToList()[0].Value;

            //получаем все photos 
            IList<XElement> xPhotos = xArtist.Elements("photos").ToList();
            if (xPhotos.Count != 0)
            {
                IEnumerable<XElement> Photos = xPhotos[0].Elements("photo").ToList();
                GetInfoList(eli_ezer.Photos, Photos);
            }

            //получаем все mails 
            IList<XElement> xMails = xArtist.Elements("mails").ToList();
            if (xMails.Count != 0)
            {
                IEnumerable<XElement> Mails = xMails[0].Elements("mail").ToList();
                GetInfoList(eli_ezer.Mails, Mails);
            }

            //получаем все phones 
            IList<XElement> xPones = xArtist.Elements("phones").ToList();
            if (xPones.Count != 0)
            {
                IEnumerable<XElement> Phones = xPones[0].Elements("phone").ToList();
                GetInfoList(eli_ezer.Phones, Phones);
            }


            //получаем все facebooks 
            IList<XElement> xFacebooks = xArtist.Elements("facebooks").ToList();
            if (xFacebooks.Count != 0)
            {
                IEnumerable<XElement> Facebook = xFacebooks[0].Elements("facebook").ToList();
                GetInfoList(eli_ezer.Facebooks, Facebook);
            }

            //получаем все vks 
            IList<XElement> xVks = xArtist.Elements("vks").ToList();
            if (xFacebooks.Count != 0)
            {
                IEnumerable<XElement> Vks = xVks[0].Elements("vk").ToList();
                GetInfoList(eli_ezer.Vks, Vks);
            }

            //получаем все instagrams 
            IList<XElement> xInstagrams = xArtist.Elements("instagrams").ToList();
            if (xFacebooks.Count != 0)
            {
                IEnumerable<XElement> Instagrams = xInstagrams[0].Elements("instagram").ToList();
                GetInfoList(eli_ezer.Instagrams, Instagrams);
            }

            return View(eli_ezer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditArtistSave(Artist eli_ezer, string[] url, IList<HttpPostedFileBase> uploadImages)
        {
            //загружаем документ xml
            string path_xml = Server.MapPath("~/App_Data/") + "Artist.xml";
            XDocument xDoc = XDocument.Load(path_xml);

            // получаем корневой элемент
            XElement xArtist = xDoc.Element("artist");

            //получаем элемент photos и в него добавляем новыe фото
            IList<XElement> xPhotos = xArtist.Elements("photos").ToList();
            if (xPhotos.Count == 0)
            {
                xArtist.Add(new XElement("photos"));
                xPhotos = xArtist.Elements("photos").ToList();
            }


            if (ModelState.IsValid)
            {
                //меняем текст biography
                if (xArtist.Elements("biography").ToList().Count != 0)
                    xArtist.Elements("biography").ToList()[0].Value = eli_ezer.biography;
                else
                    xArtist.Add(new XElement("biography", eli_ezer.biography));


                if (uploadImages != null)
                    foreach (var image in uploadImages)
                    {
                        if (image == null) continue;

                        string path_for_html = DateTime.Now.Millisecond + Path.GetFileName(image.FileName);

                        string path = Server.MapPath("~/MyPhotos/") + path_for_html;
                        image.SaveAs(path);

                        // добавляем новый элемент в файл xml
                        xPhotos[0].Add(new XElement("photo", path_for_html));
                    }

                //сохраняем изменения в файле
                xDoc.Save(path_xml);

                //удаляем выбраные фото
                DeletePhotosArtist(url, xPhotos[0]);

                //изменяем контактные данные в файле
                SetContactInfo(xArtist, "mail", eli_ezer.Mails);
                SetContactInfo(xArtist, "phone", eli_ezer.Phones);
                SetContactInfo(xArtist, "vk", eli_ezer.Vks);
                SetContactInfo(xArtist, "instagram", eli_ezer.Instagrams);
                SetContactInfo(xArtist, "facebook", eli_ezer.Facebooks);

                //сохраняем изменения в файле
                xDoc.Save(path_xml);
                ViewBag.Message = "Зміни збереженні!";
            }

            //для передачи в представление записуем список фото в объект художника
            IList<XElement> Photos = xPhotos[0].Elements("photo").ToList();
            foreach (XElement p in Photos)
                eli_ezer.Photos.Add(p.Value);

            return View("EditArtist", eli_ezer);
        }

        /// <summary>
        /// переписывает с списка <XElement> в <string>
        /// </summary>
        /// <param name="artist_info_list"> any list of object Artist</param>
        /// <param name="info_list"> list of XElement</param>
        private void GetInfoList(IList<string> artist_info_list, IEnumerable<XElement> info_list)
        {
            foreach (XElement item in info_list)
            {
                if (item.Value != "")
                    artist_info_list.Add(item.Value);
            }
        }

        /// <summary>
        /// изменяет контактные данные в файле xml
        /// </summary>
        /// <param name="xArtist">корневой элемент файла</param>
        /// <param name="contact_name"> название контактных данных</param>
        /// <param name="contacts">список этих данных</param>
        private void SetContactInfo(XElement xArtist, string contact_name, IList<string> contacts)
        {
            if (xArtist.Elements(contact_name + "s").ToList().Count != 0)
                xArtist.Elements(contact_name + "s").ToList()[0].Remove();
            XElement mails = new XElement(contact_name + "s");
            xArtist.Add(mails);

            foreach (string contact in contacts)
            {
                if (contact != "")
                    mails.Add(new XElement(contact_name, contact));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"> url фото которые нужно удалить</param>
        private void DeletePhotosArtist(string[] url, XElement xPhotos)
        {
            if (url != null)
            {
                IList<XElement> Photos = xPhotos.Elements("photo").ToList();
                foreach (string scr in url)
                {
                    for (int i = 0; i < Photos.Count; i++)
                    {
                        if (Photos.Count == 1)
                        {
                            ViewBag.MessageDelPhoto = "Потрібно, щоб залишилось одне фото!";
                            break;
                        }

                        if (("/MyPhotos/" + Photos[i].Value) == scr)
                        {
                            string path = Server.MapPath("~/MyPhotos/") + Photos[i].Value;
                            System.IO.File.Delete(path);
                            xPhotos.Elements("photo").ToList()[i].Remove();
                            Photos = xPhotos.Elements("photo").ToList();
                            break;
                        }
                    }
                }

            }
        }

        #endregion


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbWorks.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}