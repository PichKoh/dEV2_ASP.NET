using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Startup.cs
using Microsoft.Extensions.Configuration;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // ... інші методи ...

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // ... інші налаштування ...

        app.Run(async (context) =>
        {
            // Отримайте дані з конфігураційного файлу
            var name = Configuration["PersonalInfo:Name"];
            var surname = Configuration["PersonalInfo:Surname"];
            var dateOfBirth = Configuration["PersonalInfo:DateOfBirth"];
            var favoriteMovie = Configuration["PersonalInfo:FavoriteMovie"];

            // Виведіть дані у вікно браузера
            await context.Response.WriteAsync($"Name: {name}\nSurname: {surname}\nDate of Birth: {dateOfBirth}\nFavorite Movie: {favoriteMovie}");
        });
    }
}

