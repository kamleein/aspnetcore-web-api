namespace my_books.Data.ViewModels
{
    public class AuthorVM
    {
        public String FullName { get; set; }
    }

    public class AuthorWithBookVM
    {
        public string FullName { get; set; }
        public List<string> BookTitles { get; set; }
    }
}
