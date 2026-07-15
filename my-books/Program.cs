using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Data.Services;
using my_books.Exceptions;
using Serilog;
using System.Text.Json.Serialization;

try
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    // Add services to the container.

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddApiVersioning(config =>
    {
        config.DefaultApiVersion = new ApiVersion(1, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;

        //config.ApiVersionReader = new HeaderApiVersionReader("custom-version-header");
        //config.ApiVersionReader = new MediaTypeApiVersionReader();
    })
        .AddMvc();
    builder.Services.AddSwaggerGen();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
    // Configure DBContext with SQL
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
    builder.Services.AddTransient<BooksService>();
    builder.Services.AddTransient<AuthorsService>();
    builder.Services.AddTransient<PublishersService>();
    builder.Services.AddTransient<LogsService>();

    var app = builder.Build();

    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

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
    app.ConfigureBuildInExceptionHandler(loggerFactory);
    //app.ConfigureCustomExceptionHandler();

    app.MapControllers();

    app.Run();
}
finally
{
    Log.CloseAndFlush();
}

