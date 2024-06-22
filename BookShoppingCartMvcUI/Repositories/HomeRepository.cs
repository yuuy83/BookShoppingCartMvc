

using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;


        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genres.ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooks(string Stern = "", int genreId = 0)
        {
            Stern = Stern.ToLower();
            IEnumerable<Book> books = await(from book in _db.Books
                         join genre in _db.Genres
                         on book.GenreId equals genre.Id
                         where string.IsNullOrWhiteSpace(Stern) || (book != null && book.BookName.ToLower().StartsWith(Stern))
                         select new Book
                         {
                             Id = book.Id,
                             Image = book.Image,
                             AuthorName = book.AuthorName,
                             BookName = book.BookName,
                             GenreId = book.GenreId,
                             Price = book.Price,
                             GenreName = genre.GenreName,
                         }
                         ).ToListAsync();
            if (genreId > 0)
            {
                books = books.Where(a => a.GenreId == genreId).ToList();  
            }
            return books;
        }
    }
}
