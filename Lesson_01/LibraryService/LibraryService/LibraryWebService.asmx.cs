using LibraryService.Models;
using LibraryService.Services;
using LibraryService.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace LibraryService
{
    /// <summary>
    /// Summary description for LibraryWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LibraryWebService : System.Web.Services.WebService
    {
        private readonly ILibraryRepositoryService _repository;

        public LibraryWebService()
        {
            _repository = new LibraryRepositoryService(new LibraryDatabaseContextService());
        }

        [WebMethod]
        public Book[] GetBooksByTitle(string title)
        {
            return _repository.GetByTitle(title).ToArray<Book>();
        }

        [WebMethod]
        public Book[] GetBooksByAuthor(string author)
        {
            return _repository.GetByAuthor(author).ToArray<Book>();
        }

        [WebMethod]
        public Book[] GetBooksByCountry(string country)
        {
            return _repository.GetByCountry(country).ToArray<Book>();
        }
    }
}
