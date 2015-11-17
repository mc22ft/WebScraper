using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.XPath;
using WebScraper.Models;

namespace WebScraper.Controllers
{
    public class StartController : Controller
    {
        //Session
        private InfoModel PrevMovies
        {
            get { return Session["PrevMovies"] as InfoModel; }
            set { Session["PrevMovies"] = value; }
        }
        
        // Start
        public ActionResult Index()
        {
            InfoModel infoModel = new InfoModel();
            return View(infoModel);
        }
        
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post_Index(InfoModel infoModel)
        {
            //Noll ställer session om det är en ny post, ny url anrop
            if (PrevMovies != null)
            {
                PrevMovies = infoModel;
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    //Hämtar ut lista med vilka dagar som fungerar
                    Calendar calendar = new Calendar();
                    List<int> dayList = calendar.getDayList(infoModel);
                    infoModel.dayList = dayList;

                    //Hämtar ut lista med filmer som fungerar
                    Cinema cinema = new Cinema();
                    List<MovieStatus> moviesList = cinema.getCinema(infoModel);
                    infoModel.moviesList = moviesList;

                    //Hanterar och hämtar cookie
                    cookieHandeler(infoModel);

                    PrevMovies = infoModel;
                    return View(infoModel);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(String.Empty, "Du har anget en felaktig url, försök igen...");
                }
            }
            return View(infoModel);
        }

      
        public ActionResult Details(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Hämtar movie list
                    var movies = PrevMovies.moviesList;
                    Resturant resturant = new Resturant();

                    //Hämta filmem som det gäller med id
                    var movie = movies[id];
                    PrevMovies.movieObject = movie;
                    //tillbaka = restaurang obj
                    var restaurang = resturant.getTable(movie, PrevMovies);
                    PrevMovies.restaurantTimeList = restaurang;
            
                    return View("Index", "_Layout", PrevMovies);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(String.Empty, "Något gick fel i hämtningen, försök igen!");
                }
            }
            return View("Index", "_Layout", PrevMovies);
        }



        public ActionResult Login(string bookingTime)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var movies = PrevMovies;
                    //inloggningen och bekräftelsen
                    Login login = new Login();
                    login.doLogin(movies, bookingTime);

                    return View("Index", "_Layout", PrevMovies);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(String.Empty, "Något gick fel i hämtningen, försök igen!");
                }
            }
            return View("Index", "_Layout", PrevMovies);
        }


        public void cookieHandeler(InfoModel model)
        {
            //hämtar cookie i responset och sparar i session
            CookieContainer cookieContainer = new CookieContainer();
            var request = (HttpWebRequest)HttpWebRequest.Create(model.Url); //url
            request.CookieContainer = cookieContainer;

            var response = request.GetResponse();
            //cookien
            var cookie = cookieContainer.GetCookies(request.RequestUri);
            cookieContainer.Add(cookie);
            //session
            Session["cookieContainer"] = cookieContainer;

        }
    }
}

