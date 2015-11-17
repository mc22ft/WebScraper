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

namespace WebScraper.Models
{
    public class Cinema
    {
        public List<MovieStatus> getCinema(InfoModel infoModel)
        {
            var web = new HtmlWeb();

            //Hämta länkar med alla kalender namn
            //Gå in till kalender länk
            var cinemaUrl = infoModel.linkList[1];
            //Nytt anrop
            HtmlDocument cinemaDoc = web.Load(cinemaUrl);
            
            //Raw List med filmer det gäller
            //Hämtar ut filmer det gäller
            string xpath1 = "//*[@name='movie']/option";
            infoModel.filmNameList = getInfo(infoModel, cinemaDoc, xpath1);

            //Raw List med dagar det gäller
            //Hämtar ut dagar det gäller
            string xpath2 = "//*[@id='day']/option";
            infoModel.daysList = getInfo(infoModel, cinemaDoc, xpath2);
            

            //Håller json anrop och sparar till objekt
            List<MovieStatus> moviesList = new List<MovieStatus>();
            int id = 0;
            //loopar ut dagarna som gruppen kan
            for (int i = 0; i < infoModel.dayList.Count; i++)
            {
                int day = infoModel.dayList[i];
                int dayCount = infoModel.dayList[i];

                for (int x = 0; x < infoModel.filmNameList.Count; x++)
                {
                    int movieNr = x;
                    movieNr++;
                    string uri = String.Format("{0}/cinema/check?day=0{1}&movie=0{2}", infoModel.Url, day, movieNr);
                    
                    //Startar anrop
                    WebRequest webRequest = WebRequest.Create(uri);
                    WebResponse response = webRequest.GetResponse();
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    String responseData = streamReader.ReadToEnd();
                    //json
                    var checkMovie = JsonConvert.DeserializeObject<List<MovieStatus>>(responseData);
                    
                    //Bygger upp objektet
                    foreach (var item in checkMovie)
                    {
                        //sätter film namnet
                        int movNr = Int32.Parse(item.Movie);
                        movNr--;
                        item.Movie = infoModel.filmNameList[movNr];

                        //Sätter film dagen
                        item.Day = infoModel.daysList[dayCount];
                        //Sätter id
                        item.Id = id;
                        
                        moviesList.Add(item);
                        id++;
                    }
                }
            }
            return moviesList;
        }

        private List<string> getInfo(InfoModel infoModel, HtmlDocument cinemaDoc, string xpath)
        {
            

            //Raw List med filmer det gäller
            List<string> list = new List<string>();

            //Hämtar ut filmer det gäller
            var counter = 0;
            foreach (HtmlNode link in cinemaDoc.DocumentNode.SelectNodes(xpath))
            {
                if (counter > 0)
                {
                    list.Add(link.NextSibling.InnerText);
                }
                counter++;
            }
            infoModel.filmNameList = list;


            return list;
        }
    }
}