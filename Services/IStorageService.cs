namespace CourseManagement.Services{
    public interface IStorageService{
        string GetUrlFile(string fileName);
        Task SaveImageAsync(string fileName,Stream MediaBinaryStream);
        Task DeleteImageAsync(string fileName);
    }
}