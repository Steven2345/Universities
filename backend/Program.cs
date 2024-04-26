
//using backend.Domain;
//using backend.Repository;
//var repo = new UniversityRepository();

//for (int i = 0; i < 4300; i++)
/*//{
    repo.AddUniversity(new University(1, "Massachusetts Institute of Technology", "Cambridge, Massachusetts", 100, "The Massachusetts Institute of Technology (MIT) is a private land-grant research university in Cambridge, Massachusetts. Established in 1861, MIT has played a significant role in the development of many areas of modern technology and science."));
    repo.AddUniversity(new University(1, "Harvard University", "Cambridge, Massachusetts", 98.3, "Harvard University is a private Ivy League research university in Cambridge, Massachusetts. Founded in 1636 as Harvard College and named for its first benefactor, the Puritan clergyman John Harvard, it is the oldest institution of higher learning in the United States."));
    repo.AddUniversity(new University(1, "University of California, Berkeley", "Berkeley, California", 90.4, "The University of California, Berkeley is a public land-grant research university in Berkeley, California. Founded in 1868 and named after Anglo-Irish philosopher George Berkeley, it is the state's first land-grant university and the founding campus of the University of California system."));
    repo.AddUniversity(new University(1, "Princeton University", "Princeton, New Jersey", 87, "Princeton University is a private Ivy League research university in Princeton, New Jersey. Founded in 1746 in Elizabeth as the College of New Jersey, Princeton is the fourth-oldest institution of higher education in the United States and one of the nine colonial colleges chartered before the American Revolution."));
    repo.AddUniversity(new University(1, "Vanderbilt University", "Nashville, Tennessee", 38.4, "Vanderbilt University is a private research university in Nashville, Tennessee. Founded in 1873, it was named in honor of shipping and railroad magnate Cornelius Vanderbilt, who provided the school its initial $1 million endowment in the hopes that his gift and the greater work of the university would help to heal the sectional wounds inflicted by the American Civil War."));
    repo.AddUniversity(new University(1, "New York University", "New York, New York", 68, "Vanderbilt University is a private research university in Nashville, Tennessee. Founded in 1873, it was named in honor of shipping and railroad magnate Cornelius Vanderbilt, who provided the school its initial $1 million endowment in the hopes that his gift and the greater work of the university would help to heal the sectional wounds inflicted by the American Civil War."));
    repo.AddUniversity(new University(1, "University of California, Los Angeles", "Los Angeles, California", 78, "Vanderbilt University is a private research university in Nashville, Tennessee. Founded in 1873, it was named in honor of shipping and railroad magnate Cornelius Vanderbilt, who provided the school its initial $1 million endowment in the hopes that his gift and the greater work of the university would help to heal the sectional wounds inflicted by the American Civil War."));
//}*/


/*foreach (University u in repo.GetBatch(0, 5))
{
    Console.WriteLine(u.Name);
}
Console.WriteLine();
foreach (University u in repo.GetBatch(5, 15))
{
    Console.WriteLine(u.Name);
}*/


using backend.Domain;
using backend.Service;

UniversityService universityService = new UniversityService();
FacultyService facultyService = new FacultyService();


var MyAllowSpecificOrigins = "randomStringTheySay";
var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/", () => { Console.WriteLine("hit"); return universityService.GetBatchExtended(0, 50); });
app.MapGet("/{page}", (int page) => universityService.GetBatchExtended(page, 50));
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
    facultyService.AddFaculty(new Faculty(1, fcl.Name, fcl.NoOfStudents, fcl.University)));
app.MapPut("/faculties/edit/{id}", (FacultyNoId fcl, int id) =>
    facultyService.UpdateFaculty(new Faculty(id, fcl.Name, fcl.NoOfStudents, fcl.University)));
app.MapDelete("/faculties/delete/{id}", (int id) =>
    facultyService.DeleteFaculty(id));

app.Run();



/*
app.MapPut("/edit/{id}", (object uni, int id) =>
{
    Console.WriteLine(uni);
    /*
    Console.WriteLine(uni.Name);
    Console.WriteLine(uni.Location);
    Console.WriteLine(uni.Score);
    Console.WriteLine(uni.Description);
    return universityService.UpdateUniversity(new University(id, uni.Name, uni.Location, uni.Score, uni.Description));
*});
*/
