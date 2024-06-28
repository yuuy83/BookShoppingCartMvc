using System.ComponentModel.DataAnnotations;

namespace BookShoppingCartMvcUI.Models.DOTs
{
    public class GenreDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string GenreName { get; set; }
    }
}
