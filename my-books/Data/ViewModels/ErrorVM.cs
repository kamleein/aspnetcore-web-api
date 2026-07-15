using System.Text.Json;

namespace my_books.Data.ViewModels
{
    public class ErrorVM
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
