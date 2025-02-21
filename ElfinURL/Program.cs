using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ElfinURL.DB;
using ElfinURL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElfinURL;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ElfinDbContext>(opt => opt.UseInMemoryDatabase("ElfinURL"));
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen( options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ElfinURL API",
                Description = "A URL Shortener API built with ASP.NET Core Minimal API",
                TermsOfService = new Uri("https://yourdomain.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Your Name",
                    Url = new Uri("https://yourdomain.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ElfinURL v1");
                    options.RoutePrefix = string.Empty;
                }
            );
        }

        var shorten = app.MapGroup("/shorten");

        // Get Home/Index
        shorten.MapGet("/", () => "Welcome to ElfinURL!");

        // Get a specific shortened URL by short code
        shorten.MapGet("/{shortCode}", async (string shortCode, [FromServices] ElfinDbContext db) =>
        {
            var url = await db.ShorterURLs.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
            return url != null ? Results.Ok(url) : Results.NotFound();
        });

        // Create a new shortened URL
        shorten.MapPost("/", async (string originalUrl, [FromServices] ElfinDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
                return Results.BadRequest("Original URL is required.");

            // Generate a short code (for example, using part of a GUID)
            var shortCode = Guid.NewGuid().ToString("N").Substring(0, 6);

            var newUrl = new ShorterURL
            {
                OriginalUrl = originalUrl,
                ShortCode = shortCode
            };

            db.ShorterURLs.Add(newUrl);
            await db.SaveChangesAsync();

            return Results.Created($"/shorten/{shortCode}", newUrl);
        });

        // Update a shortened URL (for example, update the OriginalUrl)
        shorten.MapPut("/{id:int}", async (int id, string newOriginalUrl, [FromServices] ElfinDbContext db) =>
        {
            var url = await db.ShorterURLs.FindAsync(id);
            if (url == null)
                return Results.NotFound();

            url.OriginalUrl = newOriginalUrl;
            await db.SaveChangesAsync();

            return Results.Ok(url);
        });

        // Delete a shortened URL by id
        shorten.MapDelete("/{id:int}", async (int id, [FromServices] ElfinDbContext db) =>
        {
            var url = await db.ShorterURLs.FindAsync(id);
            if (url == null)
                return Results.NotFound();

            db.ShorterURLs.Remove(url);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

       
       
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}