using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace eliezerhome2.Models
{
    #region модели для работ художника
    public class Work
    {
        [Key]
        public int WorkId { get; set; }

        [Required(ErrorMessage = "Name must be set!")]
        public string Name { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [MinLength(100,ErrorMessage = "Length must be more than 100 characters!")]
        public string Comment { get; set; }

        public int? Price { get; set; }

        public bool IsHome { get; set; }

        public bool IsPainting { get; set; }
       

        public virtual IList<WorkPhoto> Photos { get; set; }

        public Work()
        {
            Photos = new List<WorkPhoto>();
        }
    }

    public class WorkPhoto {
        [Key]
        public int PhotoId { get; set; }
        
        [Required]
        public string URL { get; set; }
       
        public int WorkId { get; set; }
        [ForeignKey("WorkId")]
        public Work Work { get; set; }
    }

    #endregion

    #region модели для галерей фото 

    public class Gallery
    {
        [Key]
        public int GalleryId { get; set; }

        [Required(ErrorMessage = "Name must be set!")]
        public string Name { get; set; }

        public virtual IList<GalleryPhoto> Photos { get; set; }

        public Gallery()
        {
            Photos = new List<GalleryPhoto>();
        }
    }

    public class GalleryPhoto
    {
        [Key]
        public int PhotoId { get; set; }

        [Required]
        public string URL { get; set; }

        public string Comment { get; set; }

        public int GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public Gallery Gallery { get; set; }
    }

    #endregion

    #region модели для событий 

    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Name must be set!")]
        public string Name { get; set; }

        public string Comment { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date only")]
        public DateTime Date { get; set; }

        public virtual IList<EventPhoto> Photos { get; set; }

        public Event()
        {
            Photos = new List<EventPhoto>();
        }
    }

    public class EventPhoto
    {
        [Key]
        public int PhotoId { get; set; }

        [Required]
        public string URL { get; set; }

        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }

    #endregion

    //модель информации об художнике
    public class Artist
    {
        [Required(ErrorMessage = "Text must be set!")]
        public string biography { get; set; }

        //url of photos
        public IList<string> Photos { get; set; }

        //contact data
        public IList<string> Mails { get; set; }
        public IList<string> Phones { get; set; }
        public IList<string> Vks { get; set; }
        public IList<string> Facebooks { get; set; }
        public IList<string> Instagrams { get; set; }

        public Artist()
        {
            Photos = new List<string>();
            Mails = new List<string>();
            Phones = new List<string>();
            Vks = new List<string>();
            Facebooks = new List<string>();
            Instagrams = new List<string>();
        }
    }

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    #region контексты для связи с бд
    public class WorkContext : DbContext
    {
        public WorkContext() : base("DefaultConnection") { }

        public DbSet<Work> Works { get; set; }
        public DbSet<WorkPhoto> Photos { get; set; }
    }

    public class GalleryContext : DbContext
    {
        public GalleryContext() : base("DefaultConnection") { }

        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<GalleryPhoto> Photos { get; set; }

    }

    public class EventContext : DbContext
    {
        public EventContext() : base("DefaultConnection") { }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventPhoto> Photos { get; set; }

    }
    #endregion
}