using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace CsPharma_V4.Pages.EstadoPedido
{
    //autoriza a estos roles para que puedan entrar
    [Authorize(Roles = "Administradores, Empleados")]
    public class CreateModel : PageModel
    {
        private readonly DAL.Models.CsPharmaV4Context _context;

        public CreateModel(DAL.Models.CsPharmaV4Context context)
        {
            _context = context;
        }

        // Método que se ejecuta al cargar la página
        public IActionResult OnGet()
        {
            // Se obtienen los datos necesarios para las listas desplegables en la vista
            ViewData["CodEstadoDevolucion"] = new SelectList(_context.TdcCatEstadosDevolucionPedidos, "CodEstadoDevolucion", "CodEstadoDevolucion");
            ViewData["CodEstadoEnvio"] = new SelectList(_context.TdcCatEstadosEnvioPedidos, "CodEstadoEnvio", "CodEstadoEnvio");
            ViewData["CodEstadoPago"] = new SelectList(_context.TdcCatEstadosPagoPedidos, "CodEstadoPago", "CodEstadoPago");
            ViewData["CodLinea"] = new SelectList(_context.TdcCatLineasDistribucions, "CodLinea", "CodLinea");

            // Se muestra la vista
            return Page();
        }

        // Propiedad que almacena los datos que se envían al servidor
        [BindProperty]
        public TdcTchEstadoPedido TdcTchEstadoPedido { get; set; }

        // Método que se ejecuta al enviar el formulario
        
        public async Task<IActionResult> OnPostAsync()
        {
            // Se agregan los datos ingresados por el usuario a la base de datos
            _context.TdcTchEstadoPedidos.Add(TdcTchEstadoPedido);
            await _context.SaveChangesAsync();

            // Se redirecciona al usuario a la página principal
            return RedirectToPage("./Index");
        }
    }
}
