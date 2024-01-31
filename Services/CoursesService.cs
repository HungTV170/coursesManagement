using AutoMapper;
using CourseManagement.Data;
using CourseManagement.Data.Entities;
using CourseManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Services{
    public class CoursesService : ICoursesService
    {
        private readonly CourseDbContext context;
        private readonly IMapper mapper;

        public CoursesService(CourseDbContext context,IMapper mapper){
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<int> Create(CourseRequest request)
        {
            context.Courses.Add(mapper.Map<Course>(request));
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var item =await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if(item !=null){
                context.Courses.Remove(item);
            }
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseViewModel>> GetAll()
        {
            var items = await context.Courses.ToListAsync();
            return mapper.Map<IEnumerable<CourseViewModel>>(items);
        }

        public async Task<PaginatedList<CourseViewModel>> GetAllFilter(string searchString, string sortOrder, string currentFilter, int pageSize, int? pageIndex)
        {
            if(searchString != null){
                pageSize = 1;
            }else{
                searchString = currentFilter;
            }

            var courses = context.Courses.Select(c =>c);
            if(!String.IsNullOrEmpty(searchString)){
                courses  = courses.Where( c => 
                    c.Title!.Contains(searchString)||
                    c.Author!.Contains(searchString)||
                    c.Topic!.Contains(searchString));
            }

            switch(sortOrder){
                case "title_desc":
                    courses  = courses.OrderByDescending( c => c.Title);
                    break;
                case "topic_desc":
                    courses  = courses.OrderByDescending( c => c.Topic);
                    break;
                case "release_date_desc":
                    courses  = courses.OrderByDescending( c => c.ReleaseDate);
                    break;
                case "topic":
                    courses  = courses.OrderBy( c => c.Topic);
                    break;
                case "release_date":
                    courses  = courses.OrderBy( c => c.ReleaseDate);
                    break;
                default:
                    courses  = courses.OrderBy( c => c.Title);
                    break;
            }

            return PaginatedList<CourseViewModel>.Create(mapper.Map<IEnumerable<CourseViewModel>>(await courses.ToListAsync()),pageIndex??1,pageSize);

        }

        public async Task<CourseViewModel> GetById(int id)
        {
            var product = await context.Courses.FirstOrDefaultAsync(c=>c.Id== id);
            return mapper.Map<CourseViewModel>(product);
        }

        public async Task<int> Update(CourseViewModel request)
        {
            
            if(context.Courses.Any(e => e.Id == request.Id)){
                context.Courses.Update(mapper.Map<Course>(request));
            }

            return await context.SaveChangesAsync();
            
        }

        
    }


}