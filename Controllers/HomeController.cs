using Microsoft.AspNetCore.Mvc;
using TicTacToeMVC.Helpers;

namespace TicTacToeMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MinimaxAI _ai;

        public HomeController()
        {
            _ai = new MinimaxAI();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAIMove([FromBody] int[,] board)
        {
            int[] move = _ai.BestMove(board);
            return Json(new { row = move[0], col = move[1] });
        }
    }
}
