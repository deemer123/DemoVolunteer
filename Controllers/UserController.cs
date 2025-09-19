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
            // แสดงในข้อความแจ้งเตือนใน pop up ที่ Redirect ไป
            TempData["PopupMessage"] = "สมัครสมาชิกสำเร็จ!";
            TempData["PopupType"] = "success"; // success, error, inf
            return RedirectToAction("Index", "Home");
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
            // แสดงในข้อความแจ้งเตือนใน pop up ที่ Redirect ไป
            TempData["PopupMessage"] = "เข้าสู่ระบบสำเร็จ!";
            TempData["PopupType"] = "success"; // success, error, inf
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
        // แสดงในข้อความแจ้งเตือนใน pop up ที่ Redirect ไป
            TempData["PopupMessage"] = "ออกจากระบบเรียบร้อย!";
            TempData["PopupType"] = "success"; // success, error, inf
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
            // แสดงในข้อความแจ้งเตือนใน pop up ที่ Redirect ไป
            TempData["PopupMessage"] = "แก้ไขข้อมูลสำเร็จ!";
            TempData["PopupType"] = "success"; // success, error, inf
            return View();
        }
        // ถ้ามี error
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View(model);
    }

}