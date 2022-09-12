using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Raven.Core.Interfaces;
using Raven.Core.Models;
using Raven.Core.Repositories;
using Raven.Data;
using Raven.Data.Repositories;
using Raven.Services;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RavenDbContext>(options => options.UseSqlServer());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/* minimap controller-like mappings */

app.MapGet("/projects", async (RavenDbContext db) => await db.Projects.ToListAsync());
app.MapGet("/projects/shortlist", async (RavenDbContext db) =>
{
    return from project in await db.Projects.ToListAsync() select new { project.ProjectId, project.ShortCode };
});
app.MapPost("/projects", async (Project proj, RavenDbContext db) =>
{
    db.Projects.Add(proj);
    await db.SaveChangesAsync();

    return Results.Created($"/projects/{proj.ProjectId}", proj);
});

/* do not add any mappings below */

app.Run();