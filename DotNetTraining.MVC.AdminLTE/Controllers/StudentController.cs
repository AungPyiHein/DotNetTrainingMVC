using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTraining.MVC.AdminLTE.Controllers
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
            var students = await _db.TblStudents.Where(s => s.DeleteFlag == false).ToListAsync();
            return View(students);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentCode,StudentName,FirstName,LastName,DateOfBirth")] TblStudent tblStudent)
        {
            if (ModelState.IsValid)
            {
                tblStudent.DeleteFlag = false;
                _db.Add(tblStudent);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblStudent);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var tblStudent = await _db.TblStudents.FindAsync(id);
            if (tblStudent == null || tblStudent.DeleteFlag == true) return NotFound();
            return View(tblStudent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,StudentNo,StudentName,FatherName,DateOfBirth,Gender,Address,MobileNo,DeleteFlag")] TblStudent tblStudent)
        {
            if (id != tblStudent.StudentId) return NotFound();
            if (ModelState.IsValid)
            {
                _db.Update(tblStudent);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblStudent);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var tblStudent = await _db.TblStudents.FirstOrDefaultAsync(m => m.StudentId == id && m.DeleteFlag == false);
            if (tblStudent == null) return NotFound();
            return View(tblStudent);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblStudent = await _db.TblStudents.FindAsync(id);
            if (tblStudent != null)
            {
                tblStudent.DeleteFlag = true;
                _db.Update(tblStudent);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
