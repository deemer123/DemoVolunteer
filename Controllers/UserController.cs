using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DemoVolunteer.Models;
using DemoVolunteer.Data;
using Microsoft.AspNetCore.Identity;

namespace DemoVolunteer.Controllers;

public class UserController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;


    //DB Manager
    public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    // Register
    [HttpGet]
    public IActionResult Register() => View();
    [HttpPost]
    public async Task<IActionResult> Register(string firstName, string lastName, string email, string password, string gender, string phoneNumber)
    {
        Console.WriteLine("Register Method Post Worked");
        var user = new ApplicationUser
        {
            UserName = email,
            Gender = gender,
            Email = email,
            PhoneNumber = phoneNumber,
            FirstName = firstName,
            LastName = lastName
        };
        var result = await _userManager.CreateAsync(user, password);
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Error!! {error.Description}");
        }
        if (result.Succeeded)
        {
            // await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View();
    }


    // Login
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError("", "Invalid login attempt");
        return View();
    }


    // Logout
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }




    // GET Login User: Edit Profile
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "User");
        }
        var model = new UserViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        // ViewBag.FirstName = user.FirstName;
        // ViewBag.LastName = user.LastName;
        // ViewBag.gender = user.Gender;
        // ViewBag.Email = user.Email;
        // ViewBag.phoneNumber = user.PhoneNumber;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserViewModel model)
    {
        Console.WriteLine($"{model.FirstName}");
        Console.WriteLine($"{model.LastName}");
        Console.WriteLine($"{model.Gender}");
        Console.WriteLine($"{model.PhoneNumber}");
        Console.WriteLine($"{model.Email}");
        Console.WriteLine($"{model.UserName}");
        // Console.WriteLine($"{ModelState.IsValid.ToString()}");
        // if (!ModelState.IsValid) return View(model);
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            // debug ดู errors
            return Json(errors);
        }
     
        

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "User");
        

        // อัปเดตค่าจากฟอร์ม
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Gender = model.Gender;
        user.PhoneNumber = model.PhoneNumber;
        user.UserName = model.UserName;
        user.Email = model.Email;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            Console.WriteLine($"แก้ไข้เรียบร้อย");
            return RedirectToAction("Index", "Home");
        }
        // ถ้ามี error
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }
}