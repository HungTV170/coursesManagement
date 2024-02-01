
namespace CourseManagement.Services{
    public class FileStorageService : IStorageService
    {   
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public FileStorageService(IWebHostEnvironment webHostEnvironment){
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath,USER_CONTENT_FOLDER_NAME);
        }
        public async Task DeleteImageAsync(string fileName)
        {
            string filePath = Path.Combine(_userContentFolder,fileName);
            if(!File.Exists(filePath)){
                await Task.Run(()=> File.Delete(filePath) );
            }
        }

        public string GetUrlFile(string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveImageAsync(string fileName, Stream MediaBinaryStream)
        {
            if(!Directory.Exists(_userContentFolder)){
                Directory.CreateDirectory(_userContentFolder);
            }

            var filePath = Path.Combine(_userContentFolder,fileName);
            using ( var output = new FileStream(filePath,FileMode.Create)){
                await MediaBinaryStream.CopyToAsync(output);
            }
        }
    }
}