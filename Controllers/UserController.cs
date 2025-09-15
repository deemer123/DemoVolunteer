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
    public async Task<IActionResult> Register(string firstName, string lastName, string email, string password, string confirmPassword, string gender, string phoneNumber)
    {
        if (password != confirmPassword)
        {
            ModelState.AddModelError("", "Passwords do not match");
            return View();
        }

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
        if (result.Succeeded)
        {
            // await _signInManager.SignInAsync(user, false);
            ViewBag.Message = "สมัครสมาชิกสำเร็จแล้ว! ตอนนี้คุณสามารถเข้าสู่ระบบได้";
            return View();
        }
        foreach (var error in result.Errors)
        {
            // แสดง error ที่ view
            ModelState.AddModelError(string.Empty, error.Description);
        }
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
        {
            return RedirectToAction("Index", "Home");
        }
        // กรณี login ไม่สำเร็จ
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
        // ดึงข้อมูลของ user ที่ login
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "User");
        }
        //สร้าง instance ของ model และส่งไปยัง view
        var model = new UserViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        // ใช้ ViewBag ส่งค่าไปที่ view ก็ได้
        // ViewBag.FirstName = user.FirstName;
        // ViewBag.LastName = user.LastName;

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