using CourseManagement.Services;
using CourseManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace CourseManagement.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesApiController : ControllerBase{
        private readonly ICoursesService coursesService;
        public CoursesApiController(ICoursesService coursesService){
            this.coursesService = coursesService;

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CourseRequest request){
            if(!ModelState.IsValid){
                return BadRequest(ModelState.IsValid);
            }

            var result = await coursesService.Create(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState.IsValid);
            }

            var result = await coursesService.GetById(id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var result = await coursesService.GetAll();
            return Ok(result);
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetAllFilter(string searchString,string sortOrder,string currentFilter,int pageSize ,int? pageIndex)
        {
            int PageSize = 10;
            var result = await coursesService.GetAllFilter(searchString!, sortOrder!, currentFilter!, PageSize ,  pageIndex);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await coursesService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] CourseViewModel request)
        {
            var result = await coursesService.Update(request);
            return Ok(result);
        }
    }
}