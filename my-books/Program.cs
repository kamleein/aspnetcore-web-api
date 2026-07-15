using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Data.Services;
using my_books.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Configure DBContext with SQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddTransient<BooksService>();
builder.Services.AddTransient<AuthorsService>();
builder.Services.AddTransient<PublishersService>();

var app = builder.Build();

AppDbInitialiser.Seed(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//Exception Handling
app.ConfigureBuildInExceptionHandler();
//app.ConfigureCustomExceptionHandler();

app.MapControllers();

app.Run();
