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
    public class ClassroomFacilitiesController : Controller
    {
        private readonly AppDBContext _context;

        public ClassroomFacilitiesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: ClassroomFacilities
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.ClassroomFacilities.Include(c => c.Classroom).Include(c => c.Facility);
            return View(await appDBContext.ToListAsync());
        }

        // GET: ClassroomFacilities/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomFacility = await _context.ClassroomFacilities
                .Include(c => c.Classroom)
                .Include(c => c.Facility)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (classroomFacility == null)
            {
                return NotFound();
            }

            return View(classroomFacility);
        }

        // GET: ClassroomFacilities/Create
        public IActionResult Create()
        {
            ViewBag.Campuses = new SelectList(_context.Campuses, "ID", "Name");
            ViewBag.Facilities = new SelectList(_context.Facilities, "ID", "Name");
            return View();
        }

        // ✅ Get buildings based on campus
        public JsonResult GetBuildings(Guid campusId)
        {
            var buildings = _context.Buildings
                .Where(b => b.CampusID == campusId)
                .Select(b => new { b.ID, b.Building_Name })
                .ToList();
            return Json(buildings);
        }

        // ✅ Get floors based on building
        public JsonResult GetFloors(Guid buildingId)
        {
            var floors = _context.Floors
                .Where(f => f.BuildingID == buildingId)
                .Select(f => new { f.ID, f.FloorNo })
                .ToList();
            return Json(floors);
        }

        // ✅ Get classroomss based on floor
        public JsonResult GetClassrooms(Guid floorId)
        {
            var classrooms = _context.Classrooms
                .Where(c => c.FloorID == floorId)
                .Select(c => new { c.ID, c.Classroom_Name })
                .ToList();
            return Json(classrooms);
        }

        // ✅ Get facilities based on classroom
        public JsonResult GetFacilities(Guid classroomId)
        {
            var facilities = _context.Facilities
                .Select(f => new { f.ID, f.Name }) // or filter later if needed
                .ToList();
            return Json(facilities);
        }


        // POST: ClassroomFacilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Quantity,FacilityID,ClassroomID")] ClassroomFacility classroomFacility)
        {
            if (!ModelState.IsValid)
            {
                classroomFacility.ID = Guid.NewGuid();
                _context.Add(classroomFacility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdowns in case of error
            ViewBag.Campuses = new SelectList(_context.Campuses, "ID", "Name");
            ViewBag.Facilities = new SelectList(_context.Facilities, "ID", "Name");
            return View(classroomFacility);
        }


        // GET: ClassroomFacilities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomFacility = await _context.ClassroomFacilities.FindAsync(id);
            if (classroomFacility == null)
            {
                return NotFound();
            }
            ViewData["ClassroomID"] = new SelectList(_context.Classrooms, "ID", "Classroom_Name", classroomFacility.ClassroomID);
            ViewData["FacilityID"] = new SelectList(_context.Facilities, "ID", "Name", classroomFacility.FacilityID);
            return View(classroomFacility);
        }

        // POST: ClassroomFacilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Quantity,FacilityID,ClassroomID")] ClassroomFacility classroomFacility)
        {
            if (id != classroomFacility.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(classroomFacility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomFacilityExists(classroomFacility.ID))
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
            ViewData["ClassroomID"] = new SelectList(_context.Classrooms, "ID", "Classroom_Name", classroomFacility.ClassroomID);
            ViewData["FacilityID"] = new SelectList(_context.Facilities, "ID", "Name", classroomFacility.FacilityID);
            return View(classroomFacility);
        }

        // GET: ClassroomFacilities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroomFacility = await _context.ClassroomFacilities
                .Include(c => c.Classroom)
                .Include(c => c.Facility)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (classroomFacility == null)
            {
                return NotFound();
            }

            return View(classroomFacility);
        }

        // POST: ClassroomFacilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var classroomFacility = await _context.ClassroomFacilities.FindAsync(id);
            if (classroomFacility != null)
            {
                _context.ClassroomFacilities.Remove(classroomFacility);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassroomFacilityExists(Guid id)
        {
            return _context.ClassroomFacilities.Any(e => e.ID == id);
        }
    }
}
