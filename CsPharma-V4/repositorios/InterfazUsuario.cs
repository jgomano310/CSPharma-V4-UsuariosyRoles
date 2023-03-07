using CsPharma_V4.Areas.Identity.Data;

namespace CsPharma_V4.repositorios
{
    public interface InterfazUsuario
    {
        ICollection<User> GetUsers();
        User GetUser(string id);
        User actualizar(User user);

    }
}
