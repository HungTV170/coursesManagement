using System.Net.Http.Headers;
using AutoMapper;
using CourseManagement.Data;
using CourseManagement.Data.Entities;
using CourseManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Services;
public class LessonsService : ILessonsService
{
    private readonly IMapper mapper;
    private readonly CourseDbContext context;

    private readonly IStorageService storageService;

    private const string USER_CONTENT_FOLDER_NAME = "user-content";
    public LessonsService(IMapper mapper,CourseDbContext context,IStorageService storageService){
            this.storageService  = storageService;
            this.context = context;
            this.mapper = mapper;
    }

    private async Task<string> SaveFile(IFormFile formFile){
        var originalFileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName!.Trim('"');
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        await storageService.SaveImageAsync(fileName,formFile.OpenReadStream());
        return $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";

    }

    public async Task<int>  Create(LessonRequest lessonRequest)
    {
        var lesson = mapper.Map<Lesson>(lessonRequest);
        

        if(lessonRequest.Image != null){
            lesson.ImagePath = await SaveFile(lessonRequest.Image );
        }

        context.Lessons.Add(lesson);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Delete(int Id)
    {
        
        var lesson =  context.Lessons.FirstOrDefault(c=>c.Id == Id);
        if(lesson != null){
           if (!string.IsNullOrEmpty(lesson.ImagePath))
                await storageService.DeleteImageAsync(lesson.ImagePath.Replace("/" + USER_CONTENT_FOLDER_NAME + "/", ""));
            context.Lessons.Remove(lesson);
        }
        return await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<LessonViewModel>> GetAll()
    {
        return mapper.Map<IEnumerable<LessonViewModel>>(await context.Lessons.ToListAsync());
    }

    public async Task<PaginatedList<LessonViewModel>> GetAllFilter(string searchString, string sortOrder, string currentFilter, int? PageIndex, int PageSize,int? courseId)
    {
        if(searchString != null ){
            PageIndex = 1;
        }else{
            searchString = currentFilter;
        }

        var lessons = context.Lessons.Select( c=> c);
        if(!String.IsNullOrEmpty(searchString)){
            lessons = lessons.Where( c=> 
                c.Title!.Contains(searchString)||
                c.Introduction!.Contains(searchString)
                );
        };
        if(courseId != null){
            lessons = lessons.Where(c => c.CourseId == courseId);
        }
        lessons = sortOrder switch
        {
            "title_desc" => lessons.OrderByDescending(c => c.Title),
            "date_create" => lessons.OrderBy(c => c.DateCreated),
            "date_create_desc" => lessons.OrderByDescending(c => c.DateCreated),
            _ => lessons.OrderBy(c => c.Title),
        };
        return PaginatedList<LessonViewModel>.Create(mapper.Map<IEnumerable<LessonViewModel>>(await lessons.ToListAsync()),PageIndex ?? 1,PageSize);
    }

    public async Task<LessonViewModel> GetById(int Id)
    {
        var lesson =await  context.Lessons.FirstOrDefaultAsync(c=>c.Id == Id);
        if(lesson == null){
            throw new Exception("Don't Have This Lesson");
        }
        return mapper.Map<LessonViewModel>(lesson);
    }

    public async Task<int> Update(LessonViewModel lessonViewModel)
    {
        
        if(!ExitsLesson(lessonViewModel)){
            throw new Exception("Lesson does not exist");
           
        }
        if(lessonViewModel.Image != null ){
            if(!String.IsNullOrEmpty(lessonViewModel.ImagePath)){
                await storageService.DeleteImageAsync(lessonViewModel.ImagePath.Replace("/" + USER_CONTENT_FOLDER_NAME + "/", ""));
                
            }
            lessonViewModel.ImagePath = await SaveFile(lessonViewModel.Image);
        }
        context.Lessons.Update( mapper.Map<Lesson>(lessonViewModel));
        return await context.SaveChangesAsync();
    }

    private bool ExitsLesson(LessonViewModel lessonViewModel){
        return context.Lessons.Any(c => c.Id == lessonViewModel.Id);
    }
}