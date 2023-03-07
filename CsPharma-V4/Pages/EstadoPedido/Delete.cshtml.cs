using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace CsPharma_V4.Pages.EstadoPedido
{

    [Authorize(Roles = "Administradores, Empleados")]
    public class DeleteModel : PageModel
    {
        private readonly DAL.Models.CsPharmaV4Context _context;

        public DeleteModel(DAL.Models.CsPharmaV4Context context)
        {
            _context = context;
        }

        // Se establece la propiedad TdcTchEstadoPedido como BindProperty, para que pueda ser utilizada 
        // en las páginas Razor con el atributo BindProperty en lugar de usar el modelo tradicional.
        // Esto es necesario para simplificar el proceso de enlace de datos en las páginas.
        [BindProperty]
        public TdcTchEstadoPedido TdcTchEstadoPedido { get; set; }

        // Se maneja la solicitud GET que se realiza al acceder a la página Delete.
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TdcTchEstadoPedidos == null)
            {
                // Si el id es nulo o la lista de TdcTchEstadoPedidos está vacía, se devuelve NotFound.
                return NotFound();
            }

            // Se busca el TdcTchEstadoPedido con el id proporcionado.
            var tdctchestadopedido = await _context.TdcTchEstadoPedidos.FirstOrDefaultAsync(m => m.Id == id);

            if (tdctchestadopedido == null)
            {
                // Si el TdcTchEstadoPedido no existe, se devuelve NotFound.
                return NotFound();
            }
            else
            {
                // Si el TdcTchEstadoPedido existe, se establece en la propiedad TdcTchEstadoPedido para su uso posterior.
                TdcTchEstadoPedido = tdctchestadopedido;
            }

            // Se devuelve la página Delete con el TdcTchEstadoPedido a eliminar.
            return Page();
        }

        // Se maneja la solicitud POST que se realiza al eliminar un TdcTchEstadoPedido.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.TdcTchEstadoPedidos == null)
            {
                // Si el id es nulo o la lista de TdcTchEstadoPedidos está vacía, se devuelve NotFound.
                return NotFound();
            }

            // Se busca el TdcTchEstadoPedido con el id proporcionado.
            var tdctchestadopedido = await _context.TdcTchEstadoPedidos.FindAsync(id);

            if (tdctchestadopedido != null)
            {
                // Si el TdcTchEstadoPedido existe, se establece en la propiedad TdcTchEstadoPedido para su eliminación.
                TdcTchEstadoPedido = tdctchestadopedido;
                _context.TdcTchEstadoPedidos.Remove(TdcTchEstadoPedido);
                await _context.SaveChangesAsync();
            }

            // Se redirecciona a la página Index.
            return RedirectToPage("./Index");
        }
    }
}
