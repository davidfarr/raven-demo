using Microsoft.EntityFrameworkCore;
using Raven.Core.Models;
using Raven.Data;

var AllowSpecificOrigins = "_allowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RavenDbContext>(options => options.UseSqlServer());

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Project API CRUD Mappings

app.MapPost("/projects/create", async (Project proj, RavenDbContext db) =>
{
    proj.CreatedDate = DateTime.UtcNow;
    proj.Title = proj.Title.ToUpper();
    db.Projects.Add(proj);
    await db.SaveChangesAsync();
    return Results.Created($"/projects/{proj.ProjectId}", proj);
}).WithName("CreateProject");

app.MapGet("/projects", async (RavenDbContext db) =>
    await db.Projects.ToListAsync())
    .WithName("GetAllProjects");

app.MapGet("/projects/{projectId}", async (Guid projectId, RavenDbContext db) =>
{
    var foundProject = await db.Projects.FirstAsync(x => x.ProjectId == projectId);
    if (foundProject != null)
        return foundProject;
    return null;
}).WithName("GetProject");

app.MapGet("/projects/shortlist", async (RavenDbContext db) =>
{
    return from project in await db.Projects.ToListAsync() select new { project.ProjectId, project.ShortCode };
}).WithName("GetShortlist");

app.MapPost("/projects/update", async (Project proj, RavenDbContext db) =>
{
    var foundProject = await db.Projects.FirstOrDefaultAsync(x => x.ProjectId == proj.ProjectId);
    if (foundProject == null)
        return null;

    if (proj.Title == foundProject.Title
        && proj.Info == foundProject.Info
        && proj.ShortCode == foundProject.Info)
        return null; //no changes to save

    if (proj.Title != foundProject.Title)
        foundProject.Title = proj.Title.ToUpper();

    if (proj.Info != foundProject.Info)
        foundProject.Info = proj.Info;

    if (proj.ShortCode != foundProject.ShortCode)
        foundProject.ShortCode = proj.ShortCode;

    foundProject.UpdatedDate = DateTime.UtcNow;

    db.Projects.Update(foundProject);
    await db.SaveChangesAsync();
    return foundProject;
}).WithName("UpdateProject");

app.MapDelete("/projects/{projectId}", async (Guid projectId, RavenDbContext db) =>
{
    var foundProject = await db.Projects.FirstOrDefaultAsync(x => x.ProjectId == projectId);
    if (foundProject == null)
        throw new Exception("No matching record found");
    db.Projects.Remove(foundProject);
    await db.SaveChangesAsync();
    return projectId;
}).WithName("DeleteProject");

#endregion
app.UseCors(AllowSpecificOrigins);

app.Run();