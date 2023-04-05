using System.ComponentModel.DataAnnotations;

namespace LibraryService.Web.Models
{
    public enum SearchType
    {
        [Display(Name = "По названию")]
        Title,
        [Display(Name = "По автору")]
        Author,
        [Display(Name = "По стране")]
        Country
    }
}
