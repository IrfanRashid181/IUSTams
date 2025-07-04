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
    public class LabsController : Controller
    {
        private readonly AppDBContext _context;

        public LabsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Labs
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Labs.Include(l => l.Department).Include(l => l.Floor);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Labs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lab = await _context.Labs
                .Include(l => l.Department)
                .Include(l => l.Floor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lab == null)
            {
                return NotFound();
            }

            return View(lab);
        }

        // GET: Labs/Create
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


        // POST: Labs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Lab_Name,DepartmentID,FloorID")] Lab lab)
        {
            if (!ModelState.IsValid)
            {
                lab.ID = Guid.NewGuid();
                _context.Labs.Add(lab);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Campuses = new SelectList(_context.Campuses, "ID", "Name");
            ViewBag.Departments = new SelectList(_context.Departments, "ID", "Department_Name");
            return View(lab);
        }


        // GET: Labs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lab = await _context.Labs.FindAsync(id);
            if (lab == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Department_Name", lab.DepartmentID);
            ViewData["FloorID"] = new SelectList(_context.Floors, "ID", "FloorNo", lab.FloorID);
            return View(lab);
        }

        // POST: Labs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Lab_Name,DepartmentID,FloorID")] Lab lab)
        {
            if (id != lab.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(lab);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabExists(lab.ID))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Department_Name", lab.DepartmentID);
            ViewData["FloorID"] = new SelectList(_context.Floors, "ID", "FloorNo", lab.FloorID);
            return View(lab);
        }

        // GET: Labs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lab = await _context.Labs
                .Include(l => l.Department)
                .Include(l => l.Floor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lab == null)
            {
                return NotFound();
            }

            return View(lab);
        }

        // POST: Labs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var lab = await _context.Labs.FindAsync(id);
            if (lab != null)
            {
                _context.Labs.Remove(lab);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabExists(Guid id)
        {
            return _context.Labs.Any(e => e.ID == id);
        }
    }
}
