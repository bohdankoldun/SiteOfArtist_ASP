using eliezerhome2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace elliezerhome2.Controllers
{
    public class HomeController : Controller
    {
        private WorkContext dbWorks = new WorkContext();
        private GalleryContext dbGalleries = new GalleryContext();
        private EventContext dbEvents = new EventContext();

        public ActionResult Index()
        {
            XDocument xDoc = XDocument.Load(Server.MapPath("~/App_Data/") + "Artist.xml");
            XElement xArtist = xDoc.Element("artist");

            Artist eli_ezer = new Artist();
            if (xArtist.Elements("biography").ToList().Count != 0)
                eli_ezer.biography = xArtist.Elements("biography").ToList()[0].Value;


            IList<XElement> xPhotos = xArtist.Elements("photos").ToList();
            if (xPhotos.Count != 0)
            {
                IList<XElement> xPhoto = xPhotos[0].Elements("photo").ToList();
                if (xPhoto.Count > 0)
                    eli_ezer.Photos.Add(xPhoto[0].Value);
            }
            ViewBag.Artist = eli_ezer;


            IList<Work> works = dbWorks.Works.Where(p => p.IsHome == true).ToList();

            return View(works);
        }


        public ActionResult MyWorks(string selectworks)
        {
            ViewBag.selectworks = selectworks;
            //выбираем все работы
            if (selectworks == "all")
            {
                IList<Work> all_works = dbWorks.Works.ToList();
                return View(all_works);
            }
            //выбираем только картины
            else if (selectworks == "paintings")
            {
                IList<Work> paintings = dbWorks.Works.Where(p => p.IsPainting == true).ToList();
                return View(paintings);
            }
            //все работы кроме картин
            else if (selectworks == "other")
            {
                IList<Work> other_works = dbWorks.Works.Where(p => p.IsPainting == false).ToList();
                return View(other_works);
            }
            else
            {
                return View();
            }
        }

        public ActionResult WorkInfo(int id)
        {
            Work work = dbWorks.Works.Find(id);

            if (work != null)
                ViewBag.Title = work.Name;

            return View(work);
        }

        public ActionResult ArtistInfo()
        {
            XDocument xDoc = XDocument.Load(Server.MapPath("~/App_Data/") + "Artist.xml");
            XElement xArtist = xDoc.Element("artist");

            Artist eli_ezer = new Artist();
            if (xArtist.Elements("biography").ToList().Count != 0)
                eli_ezer.biography = xArtist.Elements("biography").ToList()[0].Value;


            IList<XElement> xPhotos = xArtist.Elements("photos").ToList();
            if (xPhotos.Count != 0)
            {
                IList<XElement> xPhotoList = xPhotos[0].Elements("photo").ToList();

                foreach (XElement xE in xPhotoList)
                    eli_ezer.Photos.Add(xE.Value);
            }

            return View(eli_ezer);
        }


        public ActionResult Contact()
        {
            //создаём объект модели artist
            Artist eli_ezer = new Artist();

            //загружаем документ xml
            string path_xml = Server.MapPath("~/App_Data/") + "Artist.xml";
            XDocument xDoc = XDocument.Load(path_xml);

            // получаем корневой элемент
            XElement xArtist = xDoc.Element("artist");

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


        public ActionResult Galleries()
        {
            IList<Gallery> galleries = dbGalleries.Galleries.ToList();
            return View(galleries);
        }

        public ActionResult OneGallery(int id)
        {
            Gallery gallery = dbGalleries.Galleries.Find(id);

            if (gallery != null)
                ViewBag.Title = gallery.Name;

            return View(gallery);
        }

        public ActionResult Events()
        {
            IList<Event> events = dbEvents.Events.ToList();
            return View(events);
        }

        public ActionResult OneEvent(int id)
        {
           Event _event = dbEvents.Events.Find(id);

            if (_event != null)
                ViewBag.Title = _event.Name;

            return View(_event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbWorks.Dispose();
                dbEvents.Dispose();
                dbGalleries.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}