using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace eliezerhome2.Models
{
    public class Painting
    {
        public int PaintingId { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string ShortComment { get; set; }
        public string Comment { get; set; }
        public int Price { get; set; }
        public string URL { get; set; }
    }


    public class PaintingContext : DbContext
    {
        public PaintingContext() : base("DefaultConnection") { }

        public DbSet<Painting> Paintings { get; set; }
    }
}