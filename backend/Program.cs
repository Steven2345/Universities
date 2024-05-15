using backend.Domain;
using backend.Service;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


//Generator.populateUniversities();
//Generator.populateFaculties();

UniversityService universityService = new UniversityService();
FacultyService facultyService = new FacultyService();


var MyAllowSpecificOrigins = "randomStringTheySay";
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("UniAuthConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
      .AddEntityFrameworkStores<ApplicationDbContext>();

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

// Configure the HTTP request pipeline.

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/", () => { Console.WriteLine("hit"); return universityService.GetBatchExtended(0, 10); });
app.MapGet("/{page}", (int page) => universityService.GetBatchExtended(page, 10));
app.MapGet("/names", () => universityService.GetAllNames());
app.MapGet("/details/{id}", (int id) => universityService.getById(id));
app.MapPost("/add", (UniversityNoId uni) =>
    universityService.AddUniversity(new University(1, uni.Name, uni.Location, uni.Score, uni.Description)));
app.MapPut("/edit/{id}", (UniversityNoId uni, int id) =>
    universityService.UpdateUniversity(new University(id, uni.Name, uni.Location, uni.Score, uni.Description)));
app.MapDelete("/delete/{id}", (int id) =>
    universityService.DeleteUniversity(id));

app.MapGet("/faculties", () => facultyService.GetBatch(0, 50));
app.MapGet("/faculties/{page}", (int page) => facultyService.GetBatch(page, 50));
app.MapGet("/faculties/details/{id}", (int id) => facultyService.getById(id));
app.MapPost("/faculties/add", (FacultyNoId fcl) =>
    facultyService.AddFaculty(new Faculty(1, fcl.Name, fcl.NoOfStudents, fcl.UniversityID)));
app.MapPut("/faculties/edit/{id}", (FacultyNoId fcl, int id) =>
    Console.WriteLine(fcl));
app.MapDelete("/faculties/delete/{id}", (int id) =>
    facultyService.DeleteFaculty(id));

app.Run();



public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    { }
}
