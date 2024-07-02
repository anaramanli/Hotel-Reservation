using Hotel.Controllers;
using Hotel.DAL;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController(HotelDBContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var comment = await _context.Comments.ToListAsync();

            var vm = comment.Select(c => new Comment
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                RoomId = c.RoomId,
                UserName = c.UserName,
                IsDeleted = c.IsDeleted,
            }).ToList();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest model)
        {
            var comment = await _context.Comments.FindAsync(model.Id);
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
        [HttpPost]
        public async Task<IActionResult> ChangeVisibility(int commentId)
        {
            if (commentId <= 0)
            {
                return BadRequest("Invalid Reservation ID");
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(r => r.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            comment.IsDeleted = !comment.IsDeleted;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
    
}
