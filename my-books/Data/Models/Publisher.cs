using System.Text.Json.Serialization;

namespace my_books.Data.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public String Name { get; set; }

        //Navigation Properties
        [JsonIgnore]
        public List<Book> Books { get; set; }
    }
}
