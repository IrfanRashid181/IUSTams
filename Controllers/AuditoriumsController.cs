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
    public class AuditoriumsController : Controller
    {
        private readonly AppDBContext _context;

        public AuditoriumsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Auditoriums
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Auditoriums.Include(a => a.Floor);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Auditoriums/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditorium = await _context.Auditoriums
                .Include(a => a.Floor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (auditorium == null)
            {
                return NotFound();
            }

            return View(auditorium);
        }

        // GET: Auditoriums/Create
        public IActionResult Create()
        {
            ViewBag.Campuses = _context.Campuses.ToList();
            return View();
        }
        // ✅ Add this method to get buildings for a selected campus
        [HttpGet]
        public JsonResult GetBuildingsByCampus(Guid id)
        {
            var buildings = _context.Buildings
                .Where(b => b.CampusID == id)
                .Select(b => new { id = b.ID, name = b.Building_Name })
                .ToList();

            return Json(buildings);
        }
        // ✅ Add this method to get floor count for a selected building
        [HttpGet]
        public JsonResult GetFloorsByBuilding(Guid id)
        {
            var floors = _context.Floors
                .Where(f => f.BuildingID == id)
                .Select(f => new { id = f.ID, floorNo = f.FloorNo })
                .ToList();

            return Json(floors);
        }


        // POST: Auditoriums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Auditorium_Name,FloorID")] Auditorium auditorium)
        {
            if (!ModelState.IsValid)
            {
                auditorium.ID = Guid.NewGuid();
                _context.Add(auditorium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Campuses = _context.Campuses.ToList();
            return View(auditorium);
        }


        // GET: Auditoriums/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditorium = await _context.Auditoriums.FindAsync(id);
            if (auditorium == null)
            {
                return NotFound();
            }
            ViewData["FloorID"] = new SelectList(_context.Floors, "ID", "FloorNo", auditorium.FloorID);
            return View(auditorium);
        }

        // POST: Auditoriums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Auditorium_Name,FloorID")] Auditorium auditorium)
        {
            if (id != auditorium.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auditorium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuditoriumExists(auditorium.ID))
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
            ViewData["FloorID"] = new SelectList(_context.Floors, "ID", "FloorNo", auditorium.FloorID);
            return View(auditorium);
        }

        // GET: Auditoriums/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditorium = await _context.Auditoriums
                .Include(a => a.Floor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (auditorium == null)
            {
                return NotFound();
            }

            return View(auditorium);
        }

        // POST: Auditoriums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var auditorium = await _context.Auditoriums.FindAsync(id);
            if (auditorium != null)
            {
                _context.Auditoriums.Remove(auditorium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuditoriumExists(Guid id)
        {
            return _context.Auditoriums.Any(e => e.ID == id);
        }
    }
}
