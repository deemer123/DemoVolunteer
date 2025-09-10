using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DemoVolunteer.Models;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using DemoVolunteer.Data;

namespace DemoVolunteer.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;       // ใช้ _context สำหรับ query/add/remove database
        _userManager = userManager; // ใช้ _userManager ดึงข้อมูล user        
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.FullName = user.FullName;
            ViewBag.Gender = user.Gender;
            ViewBag.PhoneNumber = user.PhoneNumber;
            ViewBag.Email = user.Email;
        }
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
