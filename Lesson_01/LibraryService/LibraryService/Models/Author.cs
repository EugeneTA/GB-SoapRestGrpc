using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryService.Models
{
    public class Author
    {
        [JsonProperty("author")]
        public string Name { get; set; }

        public override string ToString() => $"{Name}";
    }
}