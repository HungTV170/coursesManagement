using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Data;
using CourseManagement.Data.Entities;
using CourseManagement.Services;
using CourseManagement.ViewModels;

namespace CourseManagement.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ILessonsService service;
        private readonly ICoursesService coursesService;

        public LessonsController(ILessonsService service,ICoursesService coursesService)
        {
            this.service = service;
             this.coursesService = coursesService;
        }

        // GET: Lessons
        public async Task<IActionResult> Index(string searchString,string currentFilter,string sortOrder,int? PageIndex,int? courseId)
        {
            int PAGESIZE = 2;
            ViewBag.titleOrderParam  = string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("title") ? "title_desc" :"";
            ViewBag.dateOrderParam = string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("date_create") ? "date_create_desc" :"date_create";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder ;
            var Course  = await coursesService.GetAll();
            ViewBag.CourseId = Course.Select(x => new SelectListItem(){
                Text = x.Title,
                Value = x.Id.ToString(),
                Selected = courseId.HasValue && courseId == x.Id
            });
            var LessonList  = await service.GetAllFilter(searchString,sortOrder,currentFilter,PageIndex??1,PAGESIZE,courseId);
            return View(LessonList);
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int Id = id ?? 0;
            var lesson  = await service.GetById(Id);
            return View(lesson);
        }

        // GET: Lessons/Create
        public async Task<IActionResult> Create()
        {
            var CourseId = await coursesService.GetAll();
            ViewData["CourseId"] = new SelectList(CourseId, "Id", "Title");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonRequest lesson)
        {
            if (ModelState.IsValid)
            {
                await service.Create(lesson);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(await coursesService.GetAll(), "Id", "Title");
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }
            int Id = id ?? 0;
            var lesson = await service.GetById(Id);

            ViewData["CourseId"] = new SelectList(await coursesService.GetAll(), "Id", "Title");
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,LessonViewModel lesson)
        {
            if (id != lesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await service.Update(lesson);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(await coursesService.GetAll(), "Id", "Title");
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int Id = id ??0;
            var lesson = await service.GetById(Id);
            

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.Delete(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
