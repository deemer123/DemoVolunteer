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
        
        // GET: Category/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
        
        
        // public async Task<IActionResult> Index()
        // {
        // var user = await _userManager.FindByIdAsync("f7df1a85-e291-4ee7-9a70-840111923b52");
        // var categorie = await _context.Categories.FindAsync(1);
        // if (user == null)
        // {
        //     return Content("not found user");
        // }
        // if (categorie.CategoryId == null)
        // {
        //     return Content("categoriesID not found");
        // }
        // var post = new Post
        // {
        //     Title = "My Group ทำบุญตักบาต",
        //     OwnerId = user.Id,
        //     CategoryId = categorie.CategoryId,
        //     Description = "loerm 55sade kdsajiowdjskaljds",
        //     Location = "bankok",
        //     MaxParticipants = 20,
        //     AppointmentDate = DateTime.Today,
        //     Score = 5,
        //     Status = "Open",
        // };


        // _context.Posts.Add(post);
        // await _context.SaveChangesAsync();


        // var posts = await _context.Categories
        //     .Include(c => c.Posts)
        //     .Where(c => c.IsActive)
        //     .ToListAsync();
        // var Posts = await _context.
        //     return View();
        // }

        // // GET: Category
        // public async Task<IActionResult> Index()
        // {
        //     var categories = await _context.Categories
        //         .Include(c => c.Products)
        //         .Where(c => c.IsActive)
        //         .OrderBy(c => c.Name)
        //         .ToListAsync();

        //     return View(categories);
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