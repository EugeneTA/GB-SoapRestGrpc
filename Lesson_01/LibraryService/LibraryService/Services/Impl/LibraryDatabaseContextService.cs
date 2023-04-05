using LibraryService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace LibraryService.Services.Impl
{
    public class LibraryDatabaseContextService : ILibraryDatabaseContextService
    {
        private IList<Book> _library;

        public LibraryDatabaseContextService()
        {
            Initialize();
        }

        private void Initialize()
        {
            _library =  JsonConvert.DeserializeObject<IList<Book>>(Encoding.UTF8.GetString(Properties.Resources.books));
        }

        public IList<Book> Books => _library;
    }
}