using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace CsPharma_V4.Pages.EstadoPedido
{

    [Authorize(Roles = "Administradores, Empleados")] // Solo los usuarios con los roles de "Administradores" o "Empleados" pueden acceder a esta página.
    public class EditModel : PageModel
    {
        private readonly DAL.Models.CsPharmaV4Context _context;

        public EditModel(DAL.Models.CsPharmaV4Context context)
        {
            _context = context;
        }

        [BindProperty]
        public TdcTchEstadoPedido TdcTchEstadoPedido { get; set; } = default!; // Propiedad que se va a enlazar con el formulario de edición.

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TdcTchEstadoPedidos == null) // Si no se proporciona un ID o no hay pedidos de estado TdcTchEstadoPedido en la base de datos, devuelve una respuesta 404.
            {
                return NotFound();
            }

            var tdctchestadopedido = await _context.TdcTchEstadoPedidos.FirstOrDefaultAsync(m => m.Id == id);
            if (tdctchestadopedido == null) // Si no se puede encontrar un pedido de estado TdcTchEstadoPedido con el ID proporcionado, devuelve una respuesta 404.
            {
                return NotFound();
            }
            TdcTchEstadoPedido = tdctchestadopedido;
            ViewData["CodEstadoDevolucion"] = new SelectList(_context.TdcCatEstadosDevolucionPedidos, "CodEstadoDevolucion", "CodEstadoDevolucion");
            ViewData["CodEstadoEnvio"] = new SelectList(_context.TdcCatEstadosEnvioPedidos, "CodEstadoEnvio", "CodEstadoEnvio");
            ViewData["CodEstadoPago"] = new SelectList(_context.TdcCatEstadosPagoPedidos, "CodEstadoPago", "CodEstadoPago");
            ViewData["CodLinea"] = new SelectList(_context.TdcCatLineasDistribucions, "CodLinea", "CodLinea");
            return Page(); // Devuelve la página de edición con los datos del pedido de estado TdcTchEstadoPedido con el ID proporcionado.
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) // Si el modelo no es válido (por ejemplo, si falta algún campo requerido), devuelve la misma página de edición para que el usuario pueda corregir los errores.
            {
                return Page();
            }

            _context.Attach(TdcTchEstadoPedido).State = EntityState.Modified; // Adjunta la entidad TdcTchEstadoPedido a la base de datos y establece su estado como "modificado" para que se guarde en la base de datos.

            try
            {
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
            }
            catch (DbUpdateConcurrencyException) // Si se produce un error de concurrencia (es decir, si otro usuario ha modificado el mismo registro al mismo tiempo), devuelve una respuesta 404.
            {
                if (!TdcTchEstadoPedidoExists(TdcTchEstadoPedido.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index"); // Devuelve al usuario a la página principal de
        }

        private bool TdcTchEstadoPedidoExists(int id)
        {
          return _context.TdcTchEstadoPedidos.Any(e => e.Id == id);
        }
    }
}
