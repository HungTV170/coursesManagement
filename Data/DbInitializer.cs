using CourseManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Data;

static class DbInitializer{

    internal static void Seed(IServiceProvider _serviceprovider){
        using(var db = new CourseDbContext(_serviceprovider.GetRequiredService<DbContextOptions<CourseDbContext>>())){
            if(db.Courses.Any() ){
                return;
            }

            db.AddRange(
                new Course
                    {
                        Title = "ASP.NET Core MVC",
                        Topic = ".NET Programming",
                        ReleaseDate = DateTime.Today,
                        Author = "vnLab"
                    },
                    new Course
                    {
                        Title = "ASP.NET Core API",
                        Topic = ".NET Programming",
                        ReleaseDate = DateTime.Today,
                        Author = "vnLab"
                    },
                    new Course
                    {
                        Title = "Java Spring Boot",
                        Topic = "Java Programming",
                        ReleaseDate = DateTime.Today,
                        Author = "vnLab"
                    },
                    new Course
                    {
                        Title = "Laravel - The PHP Framework",
                        Topic = "PHP Programming",
                        ReleaseDate = DateTime.Today,
                        Author = "vnLab"
                    },
                    new Course
                    {
                        Title = "Angular Tutorial For Beginner",
                        Topic = "Angular Programming",
                        ReleaseDate = DateTime.Today,
                        Author = "vnLab"
                    }
            );

            db.SaveChanges();
        }
    }
}

