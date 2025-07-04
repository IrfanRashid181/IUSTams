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
    public class FloorsController : Controller
    {
        private readonly AppDBContext _context;

        public FloorsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Floors
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Floors.Include(f => f.Building);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Floors/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floor = await _context.Floors
                .Include(f => f.Building)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (floor == null)
            {
                return NotFound();
            }

            return View(floor);
        }

        // GET: Floors/Create
        public IActionResult Create()
        {
            ViewBag.Campuses = _context.Campuses.ToList();
            return View();
        }

        // ✅ Add this method to get buildings for a selected campus
        [HttpGet]
        public JsonResult GetBuildingsByCampus(Guid campusId)
        {
            var buildings = _context.Buildings
                .Where(b => b.CampusID == campusId)
                .Select(b => new { id = b.ID, name = b.Building_Name })
                .ToList();

            return Json(buildings);
        }

        // ✅ Add this method to get floor count for a selected building
        [HttpGet]
        public JsonResult GetFloorCount(Guid buildingId)
        {
            int count = _context.Floors.Count(f => f.BuildingID == buildingId);
            return Json(new { count = count });
        }

        // POST: Floors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FloorNo,BuildingID")] Floor floor)
        {
            if (!ModelState.IsValid)
            {
                floor.ID = Guid.NewGuid();

                // Add the floor to DB
                _context.Floors.Add(floor);

                // 🔼 Increment NoOfFloors in related Building
                var building = await _context.Buildings.FindAsync(floor.BuildingID);
                if (building != null)
                {
                    building.NoOfFloors += 1;
                    _context.Buildings.Update(building);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload campus dropdown
            ViewBag.Campuses = _context.Campuses.ToList();
            return View(floor);
        }




        // GET: Floors/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floor = await _context.Floors.FindAsync(id);
            if (floor == null)
            {
                return NotFound();
            }
            ViewData["BuildingID"] = new SelectList(_context.Buildings, "ID", "Building_Name", floor.BuildingID);
            return View(floor);
        }

        // POST: Floors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,FloorNo,BuildingID")] Floor floor)
        {
            if (id != floor.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(floor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FloorExists(floor.ID))
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
            ViewData["BuildingID"] = new SelectList(_context.Buildings, "ID", "Building_Name", floor.BuildingID);
            return View(floor);
        }

        // GET: Floors/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floor = await _context.Floors
                .Include(f => f.Building)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (floor == null)
            {
                return NotFound();
            }

            return View(floor);
        }

        // POST: Floors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            // Find the floor to delete
            var floor = await _context.Floors.FindAsync(id);

            if (floor == null)
            {
                return NotFound();
            }

            // Find the associated building
            var building = await _context.Buildings.FindAsync(floor.BuildingID);

            if (building != null && building.NoOfFloors > 0)
            {
                // Decrease the floor count
                building.NoOfFloors -= 1;
                _context.Buildings.Update(building);
            }

            // Remove the floor
            _context.Floors.Remove(floor);

            // Save changes to both entities
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool FloorExists(Guid id)
        {
            return _context.Floors.Any(e => e.ID == id);
        }

    }
}
