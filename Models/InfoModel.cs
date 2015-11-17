using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;

namespace WebScraper.Models
{
    public class InfoModel
    {
        [Required]
        [Display(Name = "Ange url: ")]
        public string Url { get; set; }
        public List<string> linkList { get; set; }
        public List<int> dayList { get; set; }
        public List<string> daysList { get; set; }
        public List<String> filmNameList { get; set; }
        public List<MovieStatus> moviesList { get; set; }

        public List<String> restaurantTimeList { get; set; }

        //Håller det aktuella movie objektet
        public MovieStatus movieObject { get; set; }
        public string responseData { get; set; }
       

    }
}