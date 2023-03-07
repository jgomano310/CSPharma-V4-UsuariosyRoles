using CsPharma_V4.Areas.Identity.Data;

namespace CsPharma_V4.repositorios
{
    public class RepoUsuario : InterfazUsuario
    {
        private readonly LoginContexto _contexto;

        public RepoUsuario(LoginContexto contexto)
        {
            _contexto = contexto;
        }

        public ICollection<User> GetUsers()
        {
            return _contexto.Users.ToList();
        }

        public User GetUser(string id)
        {
            return _contexto.Users.FirstOrDefault(u => u.Id == id);
        }

        public User actualizar(User user)
        {
            _contexto.Update(user);
            _contexto.SaveChanges();

            return user;
        }
    }
}
