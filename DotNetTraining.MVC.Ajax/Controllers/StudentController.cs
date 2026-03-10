using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTraining.Mvc.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _db;

        public StudentController(SchoolDbContext db)
        {
            _db = db;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await _db.TblStudents
                                   .AsNoTracking()
                                   .Where(x => !x.DeleteFlag) 
                                   .OrderByDescending(x => x.StudentId)
                                   .ToListAsync();
            return Json(students);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Save(TblStudent student)
        {
            
            await _db.TblStudents.AddAsync(student);
            var result = await _db.SaveChangesAsync();

            var response = new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Saving Successful." : "Saving Failed."
            };
            return Json(response);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _db.TblStudents.AsNoTracking().FirstOrDefaultAsync(x => x.StudentId == id);
            if (student is null)
            {
                return Json(new { IsSuccess = false, Message = "Student not found." });
            }
            return Json(student);
        }

        
        public IActionResult Edit(int id)
        {
            ViewBag.StudentId = id;
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Update(int id, TblStudent student)
        {
            var item = await _db.TblStudents.FirstOrDefaultAsync(x => x.StudentId == id);
            if (item is null)
            {
                return Json(new { IsSuccess = false, Message = "Student not found." });
            }

            item.StudentCode = student.StudentCode;
            item.FirstName = student.FirstName;
            item.LastName = student.LastName;
            item.DateOfBirth = student.DateOfBirth;

            var result = await _db.SaveChangesAsync();
            var response = new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Updating Successful." : "Updating Failed."
            };
            return Json(response);
        }

       
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.TblStudents.FirstOrDefaultAsync(x => x.StudentId == id);
            if (item is null)
            {
                return Json(new { IsSuccess = false, Message = "Student not found." });
            }

            item.DeleteFlag = true; 
            var result = await _db.SaveChangesAsync();

            var response = new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Deleting Successful." : "Deleting Failed."
            };
            return Json(response);
        }
    }
}