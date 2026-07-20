using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using my_books.Data;
using my_books.Data.Services;
using my_books.Exceptions;
using ApplicationUser = my_books.Data.Models.ApplicationUser;
using Serilog;
using System.Text;
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

    //Token Validation Parameters
    var tokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"])),

        ValidateIssuer = true,
        ValidIssuer = configuration["JWT:Issuer"],

        ValidateAudience = true,
        ValidAudience = configuration["JWT:Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero

    };

    builder.Services.AddSingleton(tokenValidationParameters);

    //Add Identity
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    //Add Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    //Add JWT Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = tokenValidationParameters;
    });

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

    //Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    //Exception Handling
    app.ConfigureBuildInExceptionHandler(loggerFactory);
    //app.ConfigureCustomExceptionHandler();

    app.MapControllers();

    AppDbInitialiser.SeedRoles(app).Wait();

    app.Run();
}
finally
{
    Log.CloseAndFlush();
}

