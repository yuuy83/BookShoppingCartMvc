using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace BookShoppingCartMvcUI.Models.DOTs
{
    public class BookDOT
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? BookName { get; set; }
        [Required]
        [MaxLength(100)]
        public string? AuthorName { get; set; }
        [Required]
        public int Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public int GenreId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }
    }
}
