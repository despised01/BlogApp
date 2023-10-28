using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("Errors/{id?}")]
        public async Task<IActionResult> ErrorsRedirect(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                switch (statusCode)
                {
                    case 400: return RedirectToPage("/Errors/ResourceIsNotFoundPage");
                    case 401: return RedirectToPage("/Errors/AccessIsDeniedPage");
                    default: return RedirectToPage("/Errors/SomethingWrongPage");
                }
            }
            return RedirectToPage("/Error");
        }
        [Route("MakeError")]
        public IActionResult MakeError()
        {
            return StatusCode(402);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
