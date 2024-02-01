using CourseManagement.Services;
using CourseManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CourseManagement.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LessonsApiController : ControllerBase{
        private readonly ILessonsService lessonsService;
        public LessonsApiController(ILessonsService lessonsService){
            this.lessonsService = lessonsService;

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] LessonRequest request){
            if(!ModelState.IsValid){
                return BadRequest(ModelState.IsValid);
            }

            var result = await lessonsService.Create(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState.IsValid);
            }

            var result = await lessonsService.GetById(id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var result = await lessonsService.GetAll();
            return Ok(result);
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetAllFilter(string searchString,string sortOrder,string currentFilter,int? PageIndex,int PageSize,int? courseId)
        {
            int pageSize = 10;
            var result = await lessonsService.GetAllFilter(searchString!, sortOrder!, currentFilter!, PageIndex ,  pageSize ,courseId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await lessonsService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] LessonViewModel request)
        {
            var result = await lessonsService.Update(request);
            return Ok(result);
        }
    }
}