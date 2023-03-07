using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace CsPharma_V4.Pages.Roles
{
    public class AdministradoresModel : PageModel
    {

        [Authorize(Roles = "Administradores")]


        public IActionResult Index()
        {
            return Page();
        }
    }
}
