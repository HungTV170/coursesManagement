using AutoMapper;
using CourseManagement.ViewModels;
using CourseManagement.Data;
using CourseManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections;
namespace CourseManagement.Service{
    class CoursesService : ICoursesService
    {
        private readonly IMapper mapper;
        private readonly CourseDbContext context;
        public CoursesService(IMapper mapper,CourseDbContext context){
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PaganitedList<CourseViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString,int pageIndex,int pageSize)
        {
           if(searchString != null){
            pageIndex =1;
           }else{
            searchString = currentFilter;
           }

            var courses = context.Courses.Select(c=>c);

            if(!String.IsNullOrEmpty(searchString)){
                courses = context.Courses.Where(c=>
                c.Author!.Contains(searchString)||
                c.Title!.Contains(searchString) ||
                c.Topic!.Contains(searchString)
            );
            }
            courses = sortOrder switch
    {
        "title_desc" => courses.OrderByDescending(s => s.Title),
        "topic" => courses.OrderBy(s => s.Topic),
        "topic_desc" => courses.OrderByDescending(s => s.Topic),
        "release_date" => courses.OrderBy(s => s.ReleaseDate),
        "release_date_desc" => courses.OrderByDescending(s => s.ReleaseDate),
        _ => courses.OrderBy(s => s.Title),
    };
                return PaganitedList<CourseViewModel>.Create(mapper.Map<IEnumerable<CourseViewModel>>(await courses.ToListAsync()),pageIndex,pageSize);
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