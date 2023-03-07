using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace CsPharma_V4.repositorios
{
    public class RepoRoles:InterfazRoles
    {
        private readonly LoginContexto _Contexto;

        public RepoRoles(LoginContexto contexto)
        {
            _Contexto = contexto;
        }

        public ICollection<IdentityRole> GetRoles()
        {
            return _Contexto.Roles.ToList();
        }

    }
}
