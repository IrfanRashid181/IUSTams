using AMSProj.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly AppDBContext _context;

    public HomeController(AppDBContext context)
    {
        _context = context;
    }


    public IActionResult Index()
    {
        var campuses = _context.Campuses
            .Include(c => c.Buildings)
            .ToList(); // Load campuses + buildings
        ViewBag.Campuses = campuses;
        return View();
    }

}
