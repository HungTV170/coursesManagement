using CourseManagement.ViewModels;

namespace CourseManagement.Service{
    public interface ICoursesService{
        Task<int> Delete(int id);
        Task<int> Update(CourseViewModel courseViewModel);
        Task<int> Create(CourseRequest courseViewModel);
        Task<IEnumerable<CourseViewModel>> GetAll();
        Task<CourseViewModel> GetById(int id);
    }
}