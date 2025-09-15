using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoVolunteer.Data;
using DemoVolunteer.Models;
using Microsoft.AspNetCore.Identity;

namespace MyMvcProject.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // การสร้างหน้า Create Post
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.categories = categories;
            //ส่งข้อมูล Categories ไปที่ View ผ่าน ViewBag
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // รับข้อมูลจาก Form และ newPost ลงใน DB
        [HttpPost]
        public async Task<IActionResult> Create(Post model, IFormFile imageFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (model == null || imageFile == null || user == null)
            {
                return Content("Model is null");
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                // ตั้งชื่อไฟล์เอง เช่น ใช้ชื่อจาก Model หรือเวลาปัจจุบัน
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var customFileName = $"{user.Id}_" + timestamp + Path.GetExtension(imageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", customFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                // กำหนด ImgURL ใน Model เป็น path สำหรับแสดงภาพ
                model.ImgURL = "/img/" + customFileName;
            }
            // บันทึก model ลง DB หรือทำงานต่อ
            var post = new Post
            {
                Title = model.Title,
                OwnerId = user.Id,
                CategoryId = model.CategoryId,
                Description = model.Description,
                Location = model.Location,
                MaxParticipants = model.MaxParticipants,
                AppointmentDate = model.AppointmentDate,
                TimeStart = model.TimeStart,
                TimeEnd = model.TimeEnd,
                Score = model.Score,
                Status = "Open",
                ImgURL = model.ImgURL,
                CreatedAt = DateTime.Now
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return Redirect("/Home/Index");
        }

        // แสดงรายละเอียดของ post
        [HttpGet]
        public async Task<IActionResult> Detail(int postId)
        {
            var post = await _context.Posts
            .Include(p => p.Owner)
            .Include(p => p.Category)
            .Include(p => p.Joins)
            .FirstOrDefaultAsync(p => p.PostId == postId);
            return View(post);
        }

        // แก้ไขข้อมูล post
        [HttpGet]
        public async Task<IActionResult> Edit(int postId)
        {
            return Content("");
        }

        // ขอเข้าร่วมกิจกรรม (ยังไม่ได้ลอง Test!!)
        [HttpPost]
        public async Task<IActionResult> Join(int postId)
        {
            // ตรวจสอบว่า user ได้ login เข้ามาไหม?
            if (!(User?.Identity != null && User.Identity.IsAuthenticated))
            {
                //ไม่ได้ Login
                ModelState.AddModelError("", "Login First");
                return RedirectToAction("Login", "User");
            }
            // user ได้ login และดึง user model
            var user = await _userManager.GetUserAsync(User);
            // ดึงรายการเดียวตาม PostId
            var post = await _context.Posts
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.PostId == postId);
            // ตรวจสอบว่า userID Login ตรง กับ postOwnerID รึเปล่า?
            if (post.Owner.Id == user.Id)
            {
                ModelState.AddModelError("", "You Can't Join Your Post");
                return View();
            }
            // สร้าง new Join model ลงบน DB
            var join = new Join
            {
                UserId = user.Id,
                PostId = postId
            };
            return View();
        }

        //หน้าจัดการโพสของฉัน
        public async Task<IActionResult> Manager()
        {
            // ตรวจสอบว่า user ได้ login เข้ามาไหม?
            if (!(User?.Identity != null && User.Identity.IsAuthenticated))
            {
                //ไม่ได้ Login
                ModelState.AddModelError("", "Login First");
                return RedirectToAction("Login", "User");
            }
            var user = await _userManager.GetUserAsync(User);
            var posts = await _context.Posts
                .Include(p => p.Joins)
                .Where(p => p.Owner.Id == user.Id)
                .ToListAsync();
            return View(posts);
        }





        //-------------------------------------------------------------------------------
        // [HttpGet]
        // public async Task<IActionResult> GetPostByUserid(int id, string category)
        // {
        //     var user = await _userManager.GetUserAsync(User);
        //     var categories = await _context.Posts
        //         .Include(c => c.Owner)
        //         .Where(c => c.OwnerId == user.Id)
        //         .ToListAsync();
        //     // ใช้ค่าที่รับมา
        //     ViewBag.ProductId = id;
        //     ViewBag.Category = category;
        //     return View();
        // }


        // // GET: Category/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var category = await _context.Categories
        //         .Include(c => c.Products.Where(p => p.IsActive))
        //         .FirstOrDefaultAsync(m => m.Id == id);

        //     if (category == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(category);
        // }

        // // POST: Category/Create
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Name,Description,IsActive")] Category category)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         category.CreatedDate = DateTime.Now;
        //         _context.Add(category);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(category);
        // }

        // // GET: Category/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var category = await _context.Categories.FindAsync(id);
        //     if (category == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(category);
        // }



        // // POST: Category/Edit/5
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedDate,IsActive")] Category category)
        // {
        //     if (id != category.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(category);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!CategoryExists(category.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(category);
        // }

        // // GET: Category/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var category = await _context.Categories
        //         .Include(c => c.Products)
        //         .FirstOrDefaultAsync(m => m.Id == id);

        //     if (category == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(category);
        // }

        // // POST: Category/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var category = await _context.Categories.FindAsync(id);
        //     if (category != null)
        //     {
        //         // ตรวจสอบว่ามีสินค้าในหมวดหมู่นี้หรือไม่
        //         var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
        //         if (hasProducts)
        //         {
        //             TempData["Error"] = "ไม่สามารถลบหมวดหมู่ที่มีสินค้าอยู่ได้";
        //             return RedirectToAction(nameof(Index));
        //         }

        //         _context.Categories.Remove(category);
        //         await _context.SaveChangesAsync();
        //     }

        //     return RedirectToAction(nameof(Index));
        // }

        // private bool CategoryExists(int id)
        // {
        //     return _context.Categories.Any(e => e.Id == id);
        // }
    }
}