namespace my_books.Data.ViewModels
{
    public class PublisherVM
    {
        public String Name { get; set; }
    }

    public class PublisherWithBooksAndAuthorsVM
    {
        public String Name { get; set; }
        public List<BookAuthorVM> BookAuthors { get; set; }
    }

    public class BookAuthorVM
    {
        public string BookName { get; set; }
        public List<string> BookAuthors { get; set; }
    }
}
