using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CurrencyAPB
{
    public class CurrencyApbParser
    {
        RestClient client;
        RestRequest request;
        private string adressUrl = "https://www.agroprombank.com/xmlinformer.php";
        public CurrencyApbParser(string url = null)
        {
            if (!string.IsNullOrEmpty(url))
                adressUrl = url;
            client = new RestClient(adressUrl);            
            request = new RestRequest("", Method.GET);
        }

        /// <summary>
        /// Возвращает все курсы валют на сегодня
        /// </summary>
        /// <returns></returns>
        public Courses GetAllToday()
        {            
            return GetRequest(request);
        }

        /// <summary>
        /// Возвращает все курсы валют за определённую дату (YYYY-MM-DD)
        /// </summary>
        /// <returns></returns>
        public Courses GetAllByDate(DateTime date)
        {
            var formatDate = date.ToString("yyyy-MM-dd");
            request.AddParameter("date", formatDate);
            return GetRequest(request);
        }

        private Courses GetRequest(RestRequest requestData)
        {
            var response = client.Execute<Currency>(requestData);
            var inputString = response.Content.Replace("<?xml version=\"1.0\" encoding=\"windows-1251\"?>", "");
            XmlSerializer serializer = new XmlSerializer(typeof(Courses));
            var result = new Courses();
            using (TextReader reader = new StringReader(inputString))
            {
                result = (Courses)serializer.Deserialize(reader);
            }
            return result;
        }

    }

    [XmlRoot(ElementName = "currency")]
    public class Currency
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlText]
        public string Text { get; set; }
        [XmlElement(ElementName = "currencySell")]
        public string CurrencySell { get; set; }
        [XmlElement(ElementName = "currencyBuy")]
        public string CurrencyBuy { get; set; }
    }

    [XmlRoot(ElementName = "course")]
    public class Course
    {
        [XmlElement(ElementName = "currency")]
        public List<Currency> Currency { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; }
    }

    [XmlRoot(ElementName = "courses")]
    public class Courses
    {
        [XmlElement(ElementName = "course")]
        public List<Course> Course { get; set; }
        [XmlAttribute(AttributeName = "bank")]
        public string Bank { get; set; }
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
    }
}