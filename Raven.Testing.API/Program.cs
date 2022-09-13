using Raven.Core.Interfaces;
using Raven.Core.Repositories;
using Raven.Data;
using Raven.Services;
using Raven.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

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
            policy.WithOrigins(
                "https://localhost:49185",
                "https://localhost:49187",
                "https://localhost:49189",
                "https://localhost:7099",
                "http://localhost:49185",
                "http://localhost:49187",
                "http://localhost:49189",
                "http://localhost:7099"
                );
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

#region Testing API CRUD Mappings

app.MapPost("/testing/create", async (TestCase tc, RavenDbContext db) =>
{
    db.TestCases.Add(tc);
    await db.SaveChangesAsync();
    return Results.Created($"/testing/{tc.TestCaseId}", tc);
}).WithName("CreateTestCase");

app.MapGet("/testing/{requirementId}", async (Guid requirementId, RavenDbContext db) =>
{
    var cases = await db.TestCases.Where(x => x.RequirementId == requirementId).ToListAsync();
    return cases;
}).WithName("GetTestsByRequirement");

app.MapGet("/testing/{projectId}", async (Guid projectId, RavenDbContext db) =>
{
    var cases = await db.TestCases.Where(x => x.ProjectId == projectId).ToListAsync();
    return cases;
}).WithName("GetTestsByProject");

app.MapGet("/testing/{testCaseId}", async (Guid testCaseId, RavenDbContext db) =>
{
    var foundCase = await db.TestCases.FirstOrDefaultAsync(x => x.TestCaseId == testCaseId);
    if (foundCase != null)
        return foundCase;
    return null;
}).WithName("GetTestCase");

app.MapPost("/testing/update", async (TestCase tc, RavenDbContext db) =>
{
    var foundCase = await db.TestCases.FirstOrDefaultAsync(x => x.TestCaseId == tc.TestCaseId);
    if (foundCase == null)
        return null;

    if (tc.Title == foundCase.Title
        && tc.Info == foundCase.Info)
        return null; //no changes to save

    if (tc.Title != foundCase.Title)
        foundCase.Title = tc.Title;

    if (tc.Info != foundCase.Info)
        foundCase.Info = tc.Info;

    foundCase.UpdatedDate = DateTime.UtcNow;

    db.TestCases.Update(foundCase);
    await db.SaveChangesAsync();
    return foundCase;
}).WithName("UpdateTestCase");

app.MapDelete("/testing/{testCaseId}", async (Guid testCaseId, RavenDbContext db) =>
{
    var foundCase = await db.TestCases.FirstOrDefaultAsync(x => x.TestCaseId == testCaseId);
    if (foundCase == null)
        throw new Exception("No matching record found");
    db.TestCases.Remove(foundCase);
    await db.SaveChangesAsync();
    return testCaseId;
}).WithName("TestCaseDelete");

#endregion
app.UseCors(AllowSpecificOrigins);

app.Run();