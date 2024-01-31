using CourseManagement.ViewModels;

namespace CourseManagement.Services;
public interface ILessonsService{
    Task<LessonViewModel> GetById(int Id);
    Task<IEnumerable<LessonViewModel>> GetAll();
    Task<int> Update(LessonViewModel lessonViewModel);
    Task<int> Delete(int Id);
    Task<int> Create(LessonRequest lessonRequest);
    Task<PaginatedList<LessonViewModel>> GetAllFilter(string searchString,string sortOrder,string currentFilter,int? PageIndex,int PageSize,int? courseId);
   
}