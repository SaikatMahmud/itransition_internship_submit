using DrawingBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DrawingBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var boards = _db.Boards.AsNoTracking()
                //.Select(b => new Board { BoardId = b.BoardId })
                .OrderByDescending(b => b.BoardId)
                .ToList();
            return View(boards);
        }

        [Route("board/{boardId}")]
        public IActionResult Board(int boardId = 0)
        {
            if (boardId == 0)
            {
                TempData["error"] = "Inavlid board id";
                return RedirectToAction("Index");
            }
            else if(!_db.Boards.Any(b => b.BoardId == boardId))
            {
                TempData["error"] = "Board not found";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [Route("board/create")]
        public JsonResult Create()
        {
            var board = new Board();
            _db.Boards.Add(board);
            if (_db.SaveChanges() > 0)
            {
                return Json(new { success = true, boardId = board.BoardId });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpGet]
        [Route("get-svg/{boardId}")]
        public async Task<FileStreamResult> GetSvg(int boardId)
        {

            var board = await _db.Boards.Where(b => b.BoardId == boardId).AsNoTracking().FirstOrDefaultAsync();
            if (board == null || board.SvgData == null)
            {
                return new FileStreamResult(new MemoryStream(), "image/svg+xml");
            }
            var svgData = board.SvgData;
            var stream = new MemoryStream(svgData);
            return new FileStreamResult(stream, "image/svg+xml");
        }

        [HttpPost]
        [Route("save-svg")]
        public async Task<JsonResult> SaveSVG(int boardId, IFormFile svgBlob)
        {
            if (svgBlob == null || svgBlob.Length == 0)
            {
                return Json(new { success = false, message = "Invalid SVG data" });
            }

            using (var memoryStream = new MemoryStream())
            {
                await svgBlob.CopyToAsync(memoryStream);

                var drawing = new Board
                {
                    BoardId = boardId,
                    SvgData = memoryStream.ToArray(),
                };

                _db.Boards.Update(drawing);
                await _db.SaveChangesAsync();
            }
            return Json(new { success = true });
        }
    }
}
