using Microsoft.EntityFrameworkCore;
using Raven.Data;
using Raven.Core.Models;

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

#region Design API CRUD Mappings

app.MapPost("/design/create", async (Requirement rq, RavenDbContext db) =>
{
    rq.CreatedDate = DateTime.UtcNow;
    db.Requirements.Add(rq);
    await db.SaveChangesAsync();
    return Results.Created($"/design/{rq.RequirementId}", rq);
}).WithName("CreateRequirement");

app.MapGet("/design/requirement/{requirementId}", async (Guid requirementId, RavenDbContext db) =>
{
    var foundReq = await db.Requirements.FirstOrDefaultAsync(x => x.RequirementId == requirementId);
    if (foundReq != null)
        return foundReq;
    return null;
}).WithName("GetRequirement");

app.MapGet("/design/project/{projectId}", async (Guid projectId, RavenDbContext db) =>
{
    var foundReqs = await db.Requirements.Where(x => x.ProjectId == projectId).ToListAsync();
    return foundReqs;
}).WithName("GetRequirementsByProject");

app.MapPost("/design/update", async (Requirement rq, RavenDbContext db) =>
{
    var foundReq = await db.Requirements.FirstOrDefaultAsync(x => x.RequirementId == rq.RequirementId);
    if (foundReq == null)
        return null;

    if (rq.Title == foundReq.Title
        && rq.Info == foundReq.Info
        && rq.VersionIntroduced == foundReq.VersionIntroduced)
        return null; //no changes to save

    if (rq.Title != foundReq.Title)
        foundReq.Title = rq.Title;

    if (rq.Info != foundReq.Info)
        foundReq.Info = rq.Info;

    if (rq.VersionIntroduced != foundReq.VersionIntroduced)
        foundReq.VersionIntroduced = rq.VersionIntroduced;

    foundReq.UpdatedDate = DateTime.UtcNow;

    db.Requirements.Update(foundReq);
    await db.SaveChangesAsync();
    return foundReq;
}).WithName("UpdateRequirement");

app.MapDelete("/design/delete/{requirementId}", async (Guid requirementId, RavenDbContext db) =>
{
    var foundReq = await db.Requirements.FirstOrDefaultAsync(x => x.RequirementId == requirementId);
    if (foundReq == null)
        throw new Exception("No matching record found");
    db.Requirements.Remove(foundReq);
    await db.SaveChangesAsync();
    return requirementId;
}).WithName("DeleteRequirement");

#endregion
app.UseCors(AllowSpecificOrigins);
app.Run();