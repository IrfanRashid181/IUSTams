using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMSProj.Data;
using AMSProj.Models;

namespace AMSProj.Controllers
{
    public class ClassroomsController : Controller
    {
        private readonly AppDBContext _context;

        public ClassroomsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Classrooms
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Classrooms.Include(c => c.Department).Include(c => c.Floor);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Classrooms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _context.Classrooms
                .Include(c => c.Department)
                .Include(c => c.Floor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (classroom == null)
            {
                return NotFound();
            }

            return View(classroom);
        }

        // GET: Classrooms/Create
        public IActionResult Create()
        {
            ViewBag.Campuses = new SelectList(_context.Campuses, "ID", "Name");
            ViewBag.Departments = new SelectList(_context.Departments, "ID", "Department_Name");
            return View();
        }


        // ✅ Get buildings based on campus
        [HttpGet]
        public JsonResult GetBuildings(Guid campusId)
        {
            var buildings = _context.Buildings
                .Where(b => b.CampusID == campusId)
                .Select(b => new { b.ID, b.Building_Name })
                .ToList();
            return Json(buildings);
        }


        // ✅ Get floors based on building
        [HttpGet]
        public JsonResult GetFloors(Guid buildingId)
        {
            var floors = _context.Floors
                .Where(f => f.BuildingID == buildingId)
                .Select(f => new { f.ID, f.FloorNo })
                .ToList();
            return Json(floors);
        }

        // ✅ Get departments based on floor
        [HttpGet]
        public JsonResult GetDepartmentsByFloor(Guid floorId)
        {
            var departments = _context.Departments
                .Where(d => d.FloorID == floorId)
                .Select(d => new { id = d.ID, name = d.Department_Name })
                .ToList();

            return Json(departments);
        }

        // POST: Classrooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // ✅ POST: Classrooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Classroom_Name,FloorID,DepartmentID")] Classroom classroom)
        {
            if (!ModelState.IsValid)
            {
                classroom.ID = Guid.NewGuid();
                _context.Classrooms.Add(classroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Campuses = new SelectList(_context.Campuses, "ID", "Name");
            ViewBag.Departments = new SelectList(_context.Departments, "ID", "Department_Name");
            return View(classroom);
        }


        // GET: Classrooms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Department_Name", classroom.DepartmentID);
            ViewData["FloorID"] = new SelectList(_context.Floors, "ID", "FloorNo", classroom.FloorID);
            return View(classroom);
        }

        // POST: Classrooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Classroom_Name,DepartmentID,FloorID")] Classroom classroom)
        {
            if (id != classroom.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(classroom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomExists(classroom.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Department_Name", classroom.DepartmentID);
            ViewData["FloorID"] = new SelectList(_context.Floors, "ID", "FloorNo", classroom.FloorID);
            return View(classroom);
        }

        // GET: Classrooms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _context.Classrooms
                .Include(c => c.Department)
                .Include(c => c.Floor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (classroom == null)
            {
                return NotFound();
            }

            return View(classroom);
        }

        // POST: Classrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom != null)
            {
                _context.Classrooms.Remove(classroom);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassroomExists(Guid id)
        {
            return _context.Classrooms.Any(e => e.ID == id);
        }
    }
}
