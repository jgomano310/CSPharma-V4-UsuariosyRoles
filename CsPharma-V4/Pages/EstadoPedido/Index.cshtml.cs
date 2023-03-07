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
    public class IndexModel : PageModel
    {
        private readonly DAL.Models.CsPharmaV4Context _context;

        public IndexModel(DAL.Models.CsPharmaV4Context context)
        {
            _context = context;
        }

        public IList<TdcTchEstadoPedido> TdcTchEstadoPedido { get; set; } = default!;
        public string filtrado { get; set; }

        // Metodo para cargar los datos de la pagina
        public async Task OnGetAsync(string filtrado)
        {
            // Consulta inicial con todas las relaciones
            IQueryable<TdcTchEstadoPedido> query = _context.TdcTchEstadoPedidos
                   .Include(t => t.CodEstadoDevolucionNavigation)
                   .Include(t => t.CodEstadoEnvioNavigation)
                   .Include(t => t.CodEstadoPagoNavigation)
                   .Include(t => t.CodLineaNavigation);

            // Si se proporciona un filtro, se aplica a la consulta
            if (!string.IsNullOrEmpty(filtrado))
            {
                query = query.Where(t => t.CodEstadoDevolucionNavigation.DesEstadoDevolucion.Contains(filtrado)
                    || t.CodEstadoEnvioNavigation.DesEstadoEnvio.Contains(filtrado)
                    || t.CodEstadoPagoNavigation.DesEstadoPago.Contains(filtrado)
                    || t.CodLinea.Contains(filtrado) || t.CodPedido.Contains(filtrado));
            }

            // Se ejecuta la consulta y se almacenan los resultados
            TdcTchEstadoPedido = await query.ToListAsync();
            filtrado = filtrado;
        }
    }
}
