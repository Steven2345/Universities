using backend;
using backend.Domain;
using backend.Repository;
using backend.Service;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;



var MyAllowSpecificOrigins = "randomStringTheySay";
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("UniAuthConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
      .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton(new UniversityRepository(builder.Configuration.GetConnectionString("DatabaseConnection")));
builder.Services.AddSingleton<UniversityService>();
builder.Services.AddSingleton(new FacultyRepository(builder.Configuration.GetConnectionString("DatabaseConnection")));
builder.Services.AddSingleton<FacultyService>();

builder.Services.AddSingleton<Generator>();


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
                      });
});

// Add services to the container.

var app = builder.Build();
FacultyService facultyService = app.Services.GetService<FacultyService>() ?? throw new Exception();
UniversityService universityService = app.Services.GetService<UniversityService>() ?? throw new Exception();

//app.Services.GetService<Generator>();  // needs to be uncommented! otherwise, Generator is not initialised
//Generator.populateUniversities();
//Generator.populateFaculties();


// Configure the HTTP request pipeline.

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/", () => { Console.WriteLine("hit"); return universityService.GetBatchExtended(0, 10); });
app.MapGet("/{page}", (int page) => universityService.GetBatchExtended(page, 10));
app.MapGet("/names", () => universityService.GetAllNames())
        .RequireAuthorization();
app.MapGet("/details/{id}", (int id) => universityService.getById(id));
app.MapPost("/add", (UniversityNoId uni, HttpContext context) =>
    universityService.AddUniversity(new University(1, uni.Name, uni.Location, uni.Score, uni.Description,
                                            context.User.FindFirstValue(ClaimTypes.NameIdentifier))))
        .RequireAuthorization();
app.MapPut("/edit/{id}", (UniversityNoId uni, int id, HttpContext context) =>
    universityService.UpdateUniversity(new University(id, uni.Name, uni.Location, uni.Score, uni.Description,
                                               context.User.FindFirstValue(ClaimTypes.NameIdentifier))))
        .RequireAuthorization();
app.MapDelete("/delete/{id}", (int id, HttpContext context) =>
    universityService.DeleteUniversity(id, context.User.FindFirstValue(ClaimTypes.NameIdentifier)))
        .RequireAuthorization();

app.MapGet("/faculties", () => facultyService.GetBatch(0, 50));
app.MapGet("/faculties/{page}", (int page) => facultyService.GetBatch(page, 50));
app.MapGet("/faculties/details/{id}", (int id) => facultyService.getById(id));
app.MapPost("/faculties/add", (FacultyNoId fcl, HttpContext context) =>
    facultyService.AddFaculty(new Faculty(1, fcl.Name, fcl.NoOfStudents, fcl.UniversityID,
                                      context.User.FindFirstValue(ClaimTypes.NameIdentifier))))
        .RequireAuthorization();
app.MapPut("/faculties/edit/{id}", (FacultyNoId fcl, int id, HttpContext context) =>
    facultyService.UpdateFaculty(new Faculty(id, fcl.Name, fcl.NoOfStudents, fcl.UniversityID,
                                         context.User.FindFirstValue(ClaimTypes.NameIdentifier))))
        .RequireAuthorization();
app.MapDelete("/faculties/delete/{id}", (int id, HttpContext context) =>
    facultyService.DeleteFaculty(id, context.User.FindFirstValue(ClaimTypes.NameIdentifier)))
        .RequireAuthorization();

app.MapIdentityApi<IdentityUser>();
app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
})
.RequireAuthorization();

app.Run();



public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    { }
}
