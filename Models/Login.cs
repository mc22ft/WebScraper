using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WebScraper.Models
{
    public class Login
    {
        public void doLogin(InfoModel infomodel, string bookingTime)
        {
            //Hämtar cookien från session
            var cookieContainer = HttpContext.Current.Session["cookieContainer"] as CookieContainer;
            
            //inloggnings uppgifter
            string Username = "zeke";
            string Password = "coys";

            //bygger strängen som ska postas mot server
            string poststring = string.Format("username={0}&password={1}&group1={2}", Username, Password, bookingTime);
            byte[] postdata = Encoding.UTF8.GetBytes(poststring);

            //länken som inloggnigen ska göras mot
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(infomodel.linkList[3]);
            
            //sätter värden som krävs vid inloggningen
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = postdata.Length;
            webRequest.UserAgent = "mc22ft";
            webRequest.Credentials = new NetworkCredential(Username, Password);
            webRequest.UseDefaultCredentials = true;
            webRequest.CookieContainer = cookieContainer;
            Stream writer = webRequest.GetRequestStream();
            writer.Write(postdata, 0, postdata.Length);
            
            //response
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
            //responseData = svaret som kommer ut från inloggningen
            String responseData = streamReader.ReadToEnd();
           
            //login svar som string
            infomodel.responseData = responseData;
        }
    }
}