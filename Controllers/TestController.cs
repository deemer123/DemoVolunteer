using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoVolunteer.Data;
using DemoVolunteer.Models;
using Microsoft.AspNetCore.Identity;

namespace MyMvcProject.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //ตัวอย่างการเรียกรายการจาก Table Posts ทั้งหมด
        // var posts = await _context.Posts.ToListAsync(); 

        //ดึงเฉพาะรายการที่ IsActive = true
        // var activePosts = await _context.Posts 
        //     .Where(p => p.IsActive)
        //     .ToListAsync();

        //ดึงพร้อม Owner (ApplicationUser)
        // var postsWithOwner = await _context.Posts 
        //     .Include(p => p.Owner)
        //     .ToListAsync()

        //ดึงพร้อม Category และ Joins
        // var postsFull = await _context.Posts  
        //     .Include(p => p.Category)
        //     .Include(p => p.Owner)
        //     .Include(p => p.Joins)
        //     .ToListAsync();

        // ดึงรายการเดียวตาม PostId
        // var post = await _context.Posts  
        //     .Include(p => p.Owner)
        //     .Include(p => p.Category)
        //     .FirstOrDefaultAsync(p => p.PostId == id);


        // เพิ่มข้อมูล
        // var group = new Group { Name = "My Group", OwnerId = userId };
        // _context.Groups.Add(group);
        // await _context.SaveChangesAsync();

        // ดึงข้อมูล
        // var groups = await _context.Groups
        //     .Include(g => g.Members)
        //     .ThenInclude(m => m.User)
        //     .ToListAsync();

        // แก้ไข
        // var group = await _context.Groups.FindAsync(groupId);
        // group.Name = "New Name";
        // await _context.SaveChangesAsync();

        // ลบ
        // var group = await _context.Groups.FindAsync(groupId);
        // _context.Groups.Remove(group);
        // await _context.SaveChangesAsync();

        // _context = ApplicationDbContext
        // ต้อง await _context.SaveChangesAsync() ทุกครั้งที่แก้ไขข้อมูล


        // Try Test
        public async Task<IActionResult> Create()
        {
            return Content($"");
        }

        public async Task<IActionResult> Read()
        {
            return Content($"");
        }

        public async Task<IActionResult> Update()
        {
            return Content($"");
        }
        
         public async Task<IActionResult> Delete()
        {
            return Content($"");
        }
    }

}