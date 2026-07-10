namespace my_books.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public String Description { get; set; }
        public bool IsRead { get; set; }
        public DateOnly? DateRead { get; set; }
        public int? Rate { get; set; }
        public String Genre { get; set; }
        public String Author { get; set; }
        public string CoverUrl { get; set; }
        public DateOnly? DateAdded { get; set; }

    }
}
