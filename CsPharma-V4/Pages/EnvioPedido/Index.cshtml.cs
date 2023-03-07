using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Models;

namespace CsPharma_V4.Pages.EnvioPedido
{
    public class IndexModel : PageModel
    {
        private readonly DAL.Models.CsPharmaV4Context _context;

        public IndexModel(DAL.Models.CsPharmaV4Context context)
        {
            _context = context;
        }

        public IList<TdcCatEstadosEnvioPedido> TdcCatEstadosEnvioPedido { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.TdcCatEstadosEnvioPedidos != null)
            {
                TdcCatEstadosEnvioPedido = await _context.TdcCatEstadosEnvioPedidos.ToListAsync();
            }
        }
    }
}
