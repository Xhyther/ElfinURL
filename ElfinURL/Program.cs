using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ElfinURL;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<DbContext>(opt => opt.UseInMemoryDatabase("ElfinURL"));
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
        shorten.MapGet("/", () => "Index / Home Page");
        shorten.MapGet("/{id}", () => "Specfic Link with ID");
        shorten.MapPost("/{longURL}", () => "Add New Link");
        shorten.MapPut("/{id}", () => "Update Link");
        shorten.MapDelete("/{id}", () => "Delete Link");

       
       
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}