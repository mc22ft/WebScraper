using HtmlAgilityPack;
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
    public class Resturant
    {
        public List<string> getTable(MovieStatus movie, InfoModel infoModel)
        {
            //Hämta ut tider i restaurangen det finns och välja på 
            //Jag har dag och tid = när bion börjar (2h lång)
            
            //Gå in till restaurang länk
            var restaurangUrl = infoModel.linkList[2];
            //Nytt anrop
            var web = new HtmlWeb();
            var restaurangDoc = web.Load(restaurangUrl);
            
            //Håller värdet i radio knapp
            List<string> radioNodeList = new List<string>();

            //Hämtar ut dagar det gäller
            foreach (HtmlNode link in restaurangDoc.DocumentNode.SelectNodes("//input[@type='radio']"))
            {
                int counter = 0;
                foreach (var item in link.Attributes)
                {
                    //vill åt sista attrebutet 3/3
                    if (counter == 2)
                    {
                        radioNodeList.Add(item.Value);
                    }
                    counter++;
                }
            }

            //matcha rätt dag det gäller
            string day = movie.Day;
            
            day = day.Replace("å", "a");
            day = day.Replace("ä", "a");
            day = day.Replace("ö", "o");

            //Hämta ut tre första bokstäverna i list där dag finnns
            string orginalDay = day.Substring(0, 3);
            orginalDay = orginalDay.ToLower();

            //Håller den dagen som fungerar att äta på 
            List<string> dayTimeList = new List<string>();
            foreach (var item in radioNodeList)
            {
                string matchDay = item.Substring(0, 3);
                if (orginalDay == matchDay)
                {
                    dayTimeList.Add(item);
                }
            }

            //Hämta klockan på när filmen börjar
            string time = movie.Time;
            
            int orginalTimeNr = Int32.Parse(time.Substring(0, 2));
            //Lägger till "2h" för längden på filmen
            orginalTimeNr += 2;

            //Håller dom tiderna som fungerar att äta på
            List<string> restaurangTimesList = new List<string>();

            foreach (var item in dayTimeList)
            {
                //lor1618
                string timeString = item.Remove(0, 3);
                int timeNr = Int32.Parse(timeString.Substring(0, 2));
                if (orginalTimeNr <= timeNr)
                {
                    //Dessa tider finns lediga tider
                    restaurangTimesList.Add(item);
                }
            }



            //Hämta länken i form action /dinner/login för posta inloggningen

            foreach (HtmlNode link in restaurangDoc.DocumentNode.SelectNodes("//form[@action]"))
            {
                var newLink = link.Attributes["action"].Value;
                var newUrl = infoModel.Url + newLink;
                infoModel.linkList.Add(newUrl);
            }


            return restaurangTimesList;
        }
    }
}