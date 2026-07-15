namespace my_books.Data.ViewModels
{
    public class BookVM
    {
        public string Title { get; set; }
        public String Description { get; set; }
        public bool IsRead { get; set; }
        public DateOnly? DateRead { get; set; }
        public int? Rate { get; set; }
        public String Genre { get; set; }
        public string CoverUrl { get; set; }

        public int PublisherId { get; set; }
        public List<int> AuthorIds { get; set; }
    }

    public class BookWithAuthorsVM
    {
        public string Title { get; set; }
        public String Description { get; set; }
        public bool IsRead { get; set; }
        public DateOnly? DateRead { get; set; }
        public int? Rate { get; set; }
        public String Genre { get; set; }
        public string CoverUrl { get; set; }

        public string PublisherName { get; set; }
        public List<string> AuthorNames { get; set; }
    }
}
