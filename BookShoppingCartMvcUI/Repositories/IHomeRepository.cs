namespace BookShoppingCartMvcUI
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Book>> GetBooks(string Stern = "", int genreId = 0);
        Task<IEnumerable<Genre>> Genres();
    }
}