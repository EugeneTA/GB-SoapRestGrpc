using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryService.Models
{
    public class Book
    {
        [JsonProperty("author")]
        public string Author { get; set; }
        public string Country { get; set; }
        public string ImageLink { get; set; }
        public string Language { get; set; }
        public string Link { get; set; }
        public int Pages { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

        public override string ToString() => $"{Title} ({Author}, {Language}, {Year} г., {Pages} стр.)";
    }
}