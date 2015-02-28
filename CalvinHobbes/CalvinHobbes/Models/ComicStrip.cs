using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalvinHobbes
{
    public class ComicStrip
    {
        public double MultiplyingFactor = 1.3;

        public ComicStrip(string imageUrl, DateTime date, int width, int height)
        {
            ImageUrl = imageUrl;
            VisibleDate = date.ToString("dd MMMM yyyy");
            Date = date;
            AdjustedWidth = width * MultiplyingFactor;
            AdjustedHeight = height * MultiplyingFactor;
        }

        public string ImageUrl { get; set; }

        public string VisibleDate { get; set; }

        public DateTime Date { get; set; }

        public double AdjustedWidth { get; set; }

        public double AdjustedHeight { get; set; }
    }
}
