using Microsoft.AspNetCore.Identity;

namespace CsPharma_V4.repositorios
{
    public interface InterfazRoles
    {
        ICollection<IdentityRole> GetRoles();
    }
}
