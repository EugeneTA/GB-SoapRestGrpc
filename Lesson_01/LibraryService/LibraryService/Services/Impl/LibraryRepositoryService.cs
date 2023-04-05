using LibraryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryService.Services.Impl
{
    public class LibraryRepositoryService : ILibraryRepositoryService
    {
        private readonly ILibraryDatabaseContextService _context;

        public LibraryRepositoryService(ILibraryDatabaseContextService context)
        {
            _context = context;
        }

        public IList<Book> GetByAuthor(string author)
        {
            if (String.IsNullOrEmpty(author)) return new List<Book>();

            try
            {
                return _context.Books.Where(book => book.Author.ToLower().Contains(author.ToLower())).ToList();
            }
            catch (Exception ex)
            { 
                return new List<Book>(); 
            }

        }

        public IList<Book> GetByCountry(string country)
        {
            if (String.IsNullOrEmpty(country)) return new List<Book>();

            try
            {
                return _context.Books.Where(book => book.Country.ToLower().Contains(country.ToLower())).ToList();
            }
            catch (Exception ex)
            { 
                return new List<Book>(); 
            }
            
        }

        public IList<Book> GetByTitle(string title)
        {
            if (String.IsNullOrEmpty(title)) return new List<Book>();

            try 
            {
                return _context.Books.Where(book => book.Title.ToLower().Contains(title.ToLower())).ToList();
            }
            catch (Exception ex)
            {
                return new List<Book>(); 
            }
            
        }
    }
}