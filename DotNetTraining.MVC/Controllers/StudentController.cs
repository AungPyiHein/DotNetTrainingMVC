using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetTraining.MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _db;

        public StudentController(SchoolDbContext db)
        {
           _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var student = await _db.TblStudents.AsNoTracking().OrderByDescending(x=>x.StudentId).ToListAsync();
            return View(student);
        }

        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Save (TblStudent student)
        {
            await _db.TblStudents.AddAsync(student);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
       
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            
            var student = await _db.TblStudents
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StudentId == id && !x.DeleteFlag);

            if (student is null)
            {
                return RedirectToAction("Index");
            }
            return View(student);
        }

       
        [HttpPost]
        public async Task<IActionResult> Update(int id, TblStudent student)
        {
            var item = await _db.TblStudents.FirstOrDefaultAsync(x => x.StudentId == id);

            if (item is null)
            {
                return RedirectToAction("Index");
            }

            
            item.StudentCode = student.StudentCode;
            item.FirstName = student.FirstName;
            item.LastName = student.LastName;
            item.DateOfBirth = student.DateOfBirth;

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.TblStudents.FirstOrDefaultAsync(x => x.StudentId == id);

            if (item is null)
            {
                return RedirectToAction("Index");
            }


         
            _db.TblStudents.Remove(item);   
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
