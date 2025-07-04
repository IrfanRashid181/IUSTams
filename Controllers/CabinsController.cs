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
    public class CabinsController : Controller
    {
        private readonly AppDBContext _context;

        public CabinsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Cabins
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Cabins.Include(c => c.Department).Include(c => c.Floor);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Cabins/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cabin = await _context.Cabins
                .Include(c => c.Department)
                .Include(c => c.Floor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cabin == null)
            {
                return NotFound();
            }

            return View(cabin);
        }

        // GET: Cabins/Create
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
        public JsonResult GetDepartments(Guid floorId)
        {
            var departments = _context.Departments
                .Where(d => d.FloorID == floorId)
                .Select(d => new { d.ID, d.Department_Name })
                .ToList();
            return Json(departments);
        }

        // POST: Cabins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cabin_Name,FloorID,DepartmentID")] Cabin cabin)
        {
            if (!ModelState.IsValid)
            {
                cabin.ID = Guid.NewGuid();
                _context.Cabins.Add(cabin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Campuses = new SelectList(_context.Campuses, "ID", "Name");
            ViewBag.Departments = new SelectList(_context.Departments, "ID", "Department_Name");
            return View(cabin);
        }




        // GET: Cabins/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cabin = await _context.Cabins.FindAsync(id);
            if (cabin == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Department_Name", cabin.DepartmentID);
            ViewData["FloorID"] = new SelectList(_context.Floors, "ID", "FloorNo", cabin.FloorID);
            return View(cabin);
        }

        // POST: Cabins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Cabin_Name,DepartmentID,FloorID")] Cabin cabin)
        {
            if (id != cabin.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(cabin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CabinExists(cabin.ID))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Department_Name", cabin.DepartmentID);
            ViewData["FloorID"] = new SelectList(_context.Floors, "ID", "FloorNo", cabin.FloorID);
            return View(cabin);
        }

        // GET: Cabins/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cabin = await _context.Cabins
                .Include(c => c.Department)
                .Include(c => c.Floor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cabin == null)
            {
                return NotFound();
            }

            return View(cabin);
        }

        // POST: Cabins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cabin = await _context.Cabins.FindAsync(id);
            if (cabin != null)
            {
                _context.Cabins.Remove(cabin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CabinExists(Guid id)
        {
            return _context.Cabins.Any(e => e.ID == id);
        }
    }
}
