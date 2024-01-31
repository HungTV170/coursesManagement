using CourseManagement.Data;
using CourseManagement.Data.Entities;
using CourseManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICoursesService,CoursesService>();
builder.Services.AddTransient<ILessonsService,LessonsService>();
//File Storage
builder.Services.AddTransient<IStorageService, FileStorageService>();
//automapper config
builder.Services.AddAutoMapper(typeof(Program));
//swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Course Management", Version = "v1" });
});


//Setup identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<CourseDbContext>()
    .AddDefaultTokenProviders();
//DbInitializer
builder.Services.AddTransient<DbInitializer>();

builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CourseDbContext") ?? throw new InvalidOperationException("Connection string 'CourseDbContext' not found.")));
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CourseDbContext>();
    db.Database.Migrate();
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Seeding data...");
        var dbInitializer = serviceProvider.GetService<DbInitializer>();
        if (dbInitializer != null)
            dbInitializer.Seed()
                         .Wait();
    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Course Management V1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
