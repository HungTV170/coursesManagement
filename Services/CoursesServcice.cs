using AutoMapper;
using CourseManagement.ViewModels;
using CourseManagement.Data;
using CourseManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace CourseManagement.Service{
    class CoursesService : ICoursesService
    {
        private readonly IMapper mapper;
        private readonly CourseDbContext context;
        public CoursesService(IMapper mapper,CourseDbContext context){
            this.context = context;
            this.mapper = mapper;
        }

        async Task<int> ICoursesService.Create(CourseRequest request)
        {
            context.Add(mapper.Map<Course>(request));
            return await context.SaveChangesAsync();
        }

        async Task<int> ICoursesService.Delete(int id)
        {
            if(context.Courses.Any(c=>c.Id == id)){
                var product =await context.Courses.FirstOrDefaultAsync(c=>c.Id == id);
                context.Remove(product);
            }
            return await context.SaveChangesAsync();
        }

        async Task<IEnumerable<CourseViewModel>> ICoursesService.GetAll()
        {
            var courses =await context.Courses.ToListAsync();
            return mapper.Map<IEnumerable<CourseViewModel>>(courses);
        }

        async Task<CourseViewModel> ICoursesService.GetById(int id)
        {
            var product = await context.Courses.FirstOrDefaultAsync(c=>c.Id == id );
            return mapper.Map<CourseViewModel>(product);
        }

        async Task<int> ICoursesService.Update(CourseViewModel request)
        {
            if(context.Courses.Any(c=>c.Id == request.Id)){
                var product = context.Update(mapper.Map<Course>(request));
                return await context.SaveChangesAsync();
            }else{
                throw new Exception("ko co khoa hoc");
            }
        }
    }
}