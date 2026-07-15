using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using my_books.Data.Models;

namespace my_books.Data
{
    public class AppDbInitialiser
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.Migrate();

            if (!context.Publishers.Any())
            {
                context.Publishers.Add(new Publisher { Name = "Default Publisher" });
                context.SaveChanges();
            }

            var publisherId = context.Publishers.First().Id;

            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book
                    {
                        Title = "Rental Person Who Does Nothing",
                        Description = "Rental Person Who Does Nothing Description",
                        IsRead = true,
                        Rate = 4,
                        Genre = "Auto Biography",
                        CoverUrl = "https://placehold.co/200x300",
                        DateAdded = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                        DateRead = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                        PublisherId = publisherId
                    },
                    new Book
                    {
                        Title = "The Secrets She Keeps",
                        Description = "The Secrets She Keeps Description",
                        Genre = "Thriller",
                        CoverUrl = "https://placehold.co/200x300",
                        IsRead = false,
                        DateAdded = DateOnly.FromDateTime(DateTime.Now),
                        PublisherId = publisherId
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
