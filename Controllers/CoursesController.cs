using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Data;
using CourseManagement.Data.Entities;
using AutoMapper;
using CourseManagement.ViewModels;
using CourseManagement.Service;

namespace CourseManagement.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICoursesService service;
        public CoursesController(ICoursesService service)
        {
            this.service = service;
          
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var products = await service.GetAll();
            return View(products);
            
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await service.GetById(id);
            if(product == null){
                return NotFound();
            }else{
                return View(product);
            }
            
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseRequest request)
        {
            if(ModelState.IsValid){
                await service.Create(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await service.GetById(id);
            if(product== null){
                return NotFound();
            }
            return View(product);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await service.Update(course);
                
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product =await service.GetById(id);
            if(product== null){
                return NotFound();
            }
            return View(product);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
