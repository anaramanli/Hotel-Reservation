using Hotel.DAL;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class CommentsController : Controller
    {
        private readonly HotelDBContext _context;

        public CommentsController(HotelDBContext context)
        {
            _context = context;
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int roomId, string content)
        {
            if (roomId < 1)
            {
                return Json(new { success = false, error = "Invalid room ID." });
            }

            if (string.IsNullOrEmpty(content))
            {
                return Json(new { success = false, error = "Comment content cannot be empty." });
            }

            var comment = new Comment
            {
                RoomId = roomId,
                Content = content,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                UserName = User.Identity.Name,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                if (comment.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) || User.IsInRole("Admin"))
                {
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = "You do not have permission to delete this comment." });
            }
            return Json(new { success = false, error = "Comment not found." });
        }
    }
}
