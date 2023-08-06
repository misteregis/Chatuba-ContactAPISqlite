using Microsoft.AspNetCore.Mvc;

namespace ContactAPISqlite.Controllers
{
    public class DefaultController : ControllerBase
    {
        [HttpGet("/")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public RedirectResult RedirectToSwaggerUi()
        {
            return Redirect("/swagger/index.html");
        }
    }
}