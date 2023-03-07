namespace CsPharma_V4.repositorios
{
    public class UnidadDeTrabajoRepo:UnidadDeTrabajo
    {
        public InterfazUsuario User { get; }

        public InterfazRoles Role { get; }

        public UnidadDeTrabajoRepo(InterfazUsuario user, InterfazRoles role)
        {
            User = user;
            Role = role;
        }
    }
}
