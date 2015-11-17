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
    public class Calendar
    {
        public List<int> getDayList(InfoModel infoModel)
        {

            
            //Nytt anrop
            var web = new HtmlWeb();
            var document = web.Load(infoModel.Url);

            //Håller länkarna till localhost:8080
            List<string> linksList = new List<string>();

            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//li/a"))
            {
                var newLink = link.Attributes["href"].Value;
                //newLink = newLink.Replace("/", "");
                var newUrl = infoModel.Url + newLink;
                linksList.Add(newUrl);
            }

            infoModel.linkList = linksList;
            
            //Hämta länkar med alla kalender namn
            //Gå in till kalender länk
            var calenderUrl = linksList[0];
            //Nytt anrop
            var CalenderDoc = web.Load(calenderUrl);

            //Hämtar namnen i kalendern
            List<string> nameList = new List<string>();

            foreach (HtmlNode link in CalenderDoc.DocumentNode.SelectNodes("//li/a"))
            {
                var newLink = link.Attributes["href"].Value;
                var newUrl = calenderUrl + "/" + newLink;
                //Lista med alla länkar på localhost:8080/calendar
                nameList.Add(newUrl);
            }
            
            //Array som håller listor med value (ok eller inte)
            List<string>[] nameInfoList = new List<string>[nameList.Count];
            int x = 0;
            foreach (var nameLink in nameList)
            {
                //skapar array som håller värdet av om man kan eller inte
                List<string> okORnotList = new List<string>();

                //laddar ny sida
                var nameInfoDoc = web.Load(nameLink);

                //lägger till ok eller inte 
                foreach (HtmlNode link in nameInfoDoc.DocumentNode.SelectNodes("//tbody/tr/td"))
                {
                    okORnotList.Add(link.InnerText);
                }
                //Lägger till listan i array
                nameInfoList[x] = okORnotList;
                x++;
            }
            
            //Hämta lista med dagar att jämföra på vilka som det fungerar på

            //Lista med svar
            List<string> answearList = new List<string>();

            for (int y = 0; y < nameInfoList.Length; y++)
            {
                var okCounter = 0;
                //for = kollar om alla är ok
                for (int i = 0; i < nameInfoList.Length; i++)
                {
                    //loopar på inner listans "ok" på alla först värde andra värde osv...
                    var lista = nameInfoList[i];
                    var ok = lista[y];
                    if (ok.ToLower() == "ok")
                    {
                        okCounter++;
                    }
                }

                //sätter ok om alla var lika
                if (okCounter == nameInfoList.Length)
                {
                    //Ok på alla dagar
                    //Vilken dag? index?
                    var indexDay = y;
                    answearList.Add("ok");
                }
                else
                {
                    answearList.Add("--");
                }
            }

            //Raw List med dagar det gäller
            List<string> dayList = new List<string>();

            //laddar ny sida
            var dayDoc = web.Load(nameList[0]);

            //lägger till ok eller inte 
            foreach (HtmlNode link in dayDoc.DocumentNode.SelectNodes("//thead/tr/th"))
            {
                dayList.Add(link.InnerText);
            }

            int counter = 1;
            //Vilka dagar är ok?
            List<int> okDayList = new List<int>();
            for (int i = 0; i < dayList.Count; i++)
            {
                if (answearList[i] == "ok")
                {
                    okDayList.Add(counter);
                    //okDayList.Add(dayList[i]);
                }
                counter++;
            }

            //Lista med ordning 
            var t = answearList.Count;

            return okDayList;
        }
    }
}