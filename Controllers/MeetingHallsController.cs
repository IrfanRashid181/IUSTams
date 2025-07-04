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
    public class MeetingHallsController : Controller
    {
        private readonly AppDBContext _context;

        public MeetingHallsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: MeetingHalls
        // GET: MeetingHalls
        public IActionResult Index()
        {
            var meetingHalls = _context.MeetingHalls.Include(m => m.Floor).ToList();
            return View(meetingHalls);
        }



        // GET: MeetingHalls/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingHall = await _context.MeetingHalls
                .Include(m => m.Floor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (meetingHall == null)
            {
                return NotFound();
            }

            return View(meetingHall);
        }

        // GET: MeetingHalls/Create
        public IActionResult Create()
        {
            ViewData["Campuses"] = new SelectList(_context.Campuses, "ID", "Name");
            return View();
        }

        // AJAX: Fetch buildings by campus ID
        [HttpGet]
        public JsonResult GetBuildings(Guid campusId)
        {
            var buildings = _context.Buildings
                .Where(b => b.CampusID == campusId)
                .Select(b => new { b.ID, b.Building_Name })
                .ToList();

            return Json(buildings);
        }
        // AJAX: Fetch floors by building ID
        [HttpGet]
        public JsonResult GetFloors(Guid buildingId)
        {
            var floors = _context.Floors
                .Where(f => f.BuildingID == buildingId)
                .Select(f => new { f.ID, f.FloorNo })
                .ToList();

            return Json(floors);
        }
        // POST: MeetingHalls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: MeetingHalls/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MeetingHall meetingHall)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(meetingHall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Campuses"] = new SelectList(_context.Campuses, "ID", "Name");
            return View(meetingHall);
        }

        // GET: MeetingHalls/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingHall = await _context.MeetingHalls.FindAsync(id);
            if (meetingHall == null)
            {
                return NotFound();
            }
            ViewData["FloorID"] = new SelectList(_context.Floors, "ID", "FloorNo", meetingHall.FloorID);
            return View(meetingHall);
        }

        // POST: MeetingHalls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,MeetingHall_Name,FloorID")] MeetingHall meetingHall)
        {
            if (id != meetingHall.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meetingHall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingHallExists(meetingHall.ID))
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
            ViewData["FloorID"] = new SelectList(_context.Floors, "ID", "FloorNo", meetingHall.FloorID);
            return View(meetingHall);
        }

        // GET: MeetingHalls/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingHall = await _context.MeetingHalls
                .Include(m => m.Floor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (meetingHall == null)
            {
                return NotFound();
            }

            return View(meetingHall);
        }

        // POST: MeetingHalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var meetingHall = await _context.MeetingHalls.FindAsync(id);
            if (meetingHall != null)
            {
                _context.MeetingHalls.Remove(meetingHall);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingHallExists(Guid id)
        {
            return _context.MeetingHalls.Any(e => e.ID == id);
        }
    }
}
