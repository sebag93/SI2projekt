using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace SI2projekt.Plan
{
    public class Grabber
    {
        const string page = "http://arturtest.cba.pl/SI2projekt/index.php";

        private static string GetPageContent(string user, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string format =
                    string.Format("{0}?login={1}&haslo={2}",
                    page, WebUtility.UrlEncode(user), WebUtility.UrlEncode(password));//sprawdza url zeby stwierdzic poprawnosc logowania
                var response = client.GetAsync(new Uri(format)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var pageContent = response.Content.ReadAsStringAsync().Result;
                    pageContent = pageContent.Replace("&#322;", "ł");
                    pageContent = pageContent.Replace("&#324;", "ń");
                    pageContent = pageContent.Replace("&#261;", "ą");
                    pageContent = pageContent.Replace("&oacute;", "ó");

                    pageContent = pageContent.Replace("&#263;", "ć");
                    pageContent = pageContent.Replace("&#281;", "ę");
                    pageContent = pageContent.Replace("&#347;", "ś");
                    pageContent = pageContent.Replace("&#378;", "ź");
                    pageContent = pageContent.Replace("&#380;", "ż");


                    return pageContent;
                }
            }
            return null;
        }

        public static bool LoginSuccess(string user, string password)
        {
            string pageContent = GetPageContent(user, password);
            if (pageContent.Contains(@"class=""error"""))
                return false;

            return true;

        }
        public static List<Slot> Grab(string user, string password)
        {

            List<Slot> tmp = new List<Slot>();//Slot pojedyncze zajecia

            string pageContent = GetPageContent(user, password);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(pageContent);
            var tab = document.DocumentNode
              .Descendants("table")   //Zwraca przefiltrowane kolekcję elementów podrzędnych dla tego dokumentu lub elementu, w kolejności dokumentu. Tylko elementy, które ma pasującego XName znajdują się w kolekcji.
              .Where(d => d.Attributes.Contains("class") &&
              d.Attributes["class"].Value.Contains("dataTable")).
              FirstOrDefault();

            if (tab != null)
            {
                foreach (var row in tab.ChildNodes)
                {
                    string name = string.Empty;
                    string dates = string.Empty;
                    string startTime = string.Empty;
                    string endTime = string.Empty;
                    string room = string.Empty;
                    int cell = 0;

                    if (row.Name == "tr")
                    {
                        bool header = false;
                        foreach (var column in row.ChildNodes)
                        {
                            if (column.Name == "th")
                            {
                                header = true;
                                break;
                            }
                            if (column.Name == "td")
                            {
                                switch (cell)
                                {
                                    case 1:
                                        name = column.InnerText;
                                        break;
                                    case 2:
                                        dates = column.InnerText;
                                        break;
                                    case 4:
                                        startTime = column.InnerText;
                                        break;
                                    case 5:
                                        endTime = column.InnerText;
                                        break;
                                    case 6:
                                        room = column.InnerText;
                                        break;
                                }
                                cell++;
                            }



                        }
                        if (header == true)
                            continue;
                        foreach (var date in dates.Split(' '))
                        {
                            string adres = "50.019176, 21.988837";
                            if (room.Contains("Budynek A"))
                                adres = "50.026864, 21.985243";
                            if (room.Contains("Budynek B"))
                                adres = "50.026831, 21.984441";
                            if (room.Contains("Budynek C"))
                                adres = "50.026305, 21.983746";
                            if (room.Contains("Budynek D"))
                                adres = "50.025810, 21.983674";
                            if (room.Contains("Budynek E"))
                                adres = "50.026328, 21.984691";
                            if (room.Contains("Budynek F"))
                                adres = "50.025956, 21.983414";
                            if (room.Contains("Budynek G"))
                                adres = "50.025964, 21.984189";
                            if (room.Contains("Budynek H"))
                                adres = "50.019830, 21.985741";
                            if (room.Contains("Budynek J"))
                                adres = "50.019441, 21.980521";
                            if (room.Contains("Budynek K"))
                                adres = "50.019502, 21.985528";
                            if (room.Contains("Budynek L"))
                                adres = "50.018154, 21.986880";
                            if (room.Contains("Budynek Ł"))
                                adres = "50.018414, 21.980445";
                            if (room.Contains("Budynek O"))
                                adres = "50.020534, 21.983454";
                            if (room.Contains("Budynek P"))
                                adres = "50.019011, 21.981421";
                            if (room.Contains("Budynek R"))
                                adres = "50.019559, 21.980021";
                            if (room.Contains("Budynek S"))
                                adres = "50.019328, 21.987269";
                            if (room.Contains("Budynek W"))
                                adres = "50.017030, 21.982155";
                            if (room.Contains("Budynek V"))
                                adres = "50.019176, 21.988837";
                            //reszta budynkwo 
                            tmp.Add(new Slot() { Name = name, Location = room, LocationAdress = adres, Start = DateTime.Parse(date + " " + startTime), Stop = DateTime.Parse(date + " " + endTime) });
                        }
                    }
                }
            }
            return tmp;
        }

    }


}