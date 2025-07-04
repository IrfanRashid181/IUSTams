using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMSProj.Data;
using AMSProj.Models;

namespace AMSProj.Controllers
{
    public class BuildingsController : Controller
    {
        private readonly AppDBContext _context;

        public BuildingsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Buildings
        public async Task<IActionResult> Index()
        {
            var buildingsWithCampus = _context.Buildings.Include(b => b.Campus);
            return View(await buildingsWithCampus.ToListAsync());
        }

        // GET: Buildings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var building = await _context.Buildings
                .Include(b => b.Campus)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (building == null) return NotFound();

            return View(building);
        }

        // GET: Buildings/Create
        public IActionResult Create()
        {
            ViewData["CampusID"] = new SelectList(_context.Campuses, "ID", "Name");
            return View();
        }

        // POST: Buildings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Building_Name,Type,Nearest_Building,NoOfFloors,CampusID")] Building building)
        {
            if (!ModelState.IsValid)
            {
                building.ID = Guid.NewGuid();

                // Get all buildings in the same campus ordered by creation
                var existingBuildings = await _context.Buildings
                    .Where(b => b.CampusID == building.CampusID)
                    .OrderBy(b => b.Building_Name) // or OrderByDescending(b => b.ID) if you rely on insert order
                    .ToListAsync();

                if (existingBuildings.Any())
                {
                    // Set current building's Nearest_Building to the last added
                    var lastBuilding = existingBuildings.Last();
                    building.Nearest_Building = lastBuilding.Building_Name;

                    // Update last building's Nearest_Building to the new one
                    lastBuilding.Nearest_Building = building.Building_Name;
                    _context.Buildings.Update(lastBuilding);
                }
                else
                {
                    // First building — set Nearest_Building as empty
                    building.Nearest_Building = "NULL"; // or string.Empty or null if nullable
                }

                _context.Add(building);

                // Update campus count
                var campus = await _context.Campuses.FirstOrDefaultAsync(c => c.ID == building.CampusID);
                if (campus != null)
                {
                    campus.NoOfBuildings += 1;
                    _context.Campuses.Update(campus);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CampusID"] = new SelectList(_context.Campuses, "ID", "Name", building.CampusID);
            return View(building);
        }


        // GET: Buildings/Edit/5
        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var building = await _context.Buildings.FindAsync(id);
            if (building == null) return NotFound();

            ViewData["CampusID"] = new SelectList(_context.Campuses, "ID", "Name", building.CampusID);

            // Get other buildings in the same campus (excluding current)
            var nearbyBuildings = await _context.Buildings
                .Where(b => b.CampusID == building.CampusID && b.ID != building.ID)
                .ToListAsync();

            ViewBag.NearestBuildings = new SelectList(nearbyBuildings, "Building_Name", "Building_Name", building.Nearest_Building);

            return View(building);
        }


        // POST: Buildings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Building_Name,Type,Nearest_Building,NoOfFloors,CampusID")] Building building)
        {
            if (id != building.ID) return NotFound();

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(building);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists(building.ID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CampusID"] = new SelectList(_context.Campuses, "ID", "Name", building.CampusID);
            return View(building);
        }

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var building = await _context.Buildings
                .Include(b => b.Campus)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (building == null) return NotFound();

            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building != null)
            {
                // Decrement NoOfBuildings in related campus
                var campus = await _context.Campuses.FirstOrDefaultAsync(c => c.ID == building.CampusID);
                if (campus != null && campus.NoOfBuildings > 0)
                {
                    campus.NoOfBuildings -= 1;
                    _context.Campuses.Update(campus);
                }

                _context.Buildings.Remove(building);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ Fix for CS0103: This must be present
        private bool BuildingExists(Guid id)
        {
            return _context.Buildings.Any(e => e.ID == id);
        }
    }
}
