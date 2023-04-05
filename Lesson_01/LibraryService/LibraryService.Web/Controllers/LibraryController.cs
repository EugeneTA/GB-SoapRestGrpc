using LibraryService.Web.Models;
using LibraryServiceReference;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryService.Web.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILogger<LibraryController> _logger;

        public LibraryController(ILogger<LibraryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(SearchType searchType, string searchString)
        {
            try
            {
                LibraryWebServiceSoapClient client = new LibraryWebServiceSoapClient(LibraryWebServiceSoapClient.EndpointConfiguration.LibraryWebServiceSoap);

                if ((string.IsNullOrEmpty(searchString) == false) && searchString.Length > 2)
                {
                    switch (searchType)
                    {
                        case SearchType.Title:
                            {
                                return View(new BookViewModel { Books = client.GetBooksByTitle(searchString).ToArray<Book>() });
                            }
                        case SearchType.Author:
                            {
                                return View(new BookViewModel { Books = client.GetBooksByAuthor(searchString).ToArray<Book>() });
                            }
                        case SearchType.Country:
                            {
                                return View(new BookViewModel { Books = client.GetBooksByCountry(searchString).ToArray<Book>() });
                            }
                        default: { } break;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }

            return View(
                new BookViewModel { Books = new Book[] { } }
                );
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}