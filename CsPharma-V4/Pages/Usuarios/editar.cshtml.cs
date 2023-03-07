using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Modelo;
using CsPharma_V4.repositorios;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CsPharma_V4.Pages.Usuarios
{
    public class editarModel : PageModel
    {
        private readonly UnidadDeTrabajo _unidad;
        private readonly SignInManager<User> _signInManager;

        // Constructor que recibe la Unidad de Trabajo y el SignInManager para autenticación.
        public editarModel(UnidadDeTrabajo unitOfWork, SignInManager<User> signInManager)
        {
            _unidad = unitOfWork;
            _signInManager = signInManager;
        }

        // Propiedad BindProperty para el modelo EditUser.
        [BindProperty]
        public EditUser UserModel { get; set; } = default!;

        // Método para la solicitud GET de la página, recibe el ID del usuario.
        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Obtiene el usuario y los roles asociados a él.
            var user = _unidad.User.GetUser(id);
            var roles = _unidad.Role.GetRoles();

            // Obtiene los roles asignados al usuario actual.
            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            // Crea una lista de SelectListItem para cada rol, indicando si está seleccionado o no.
            var roleItems = roles.Select(role =>
                new SelectListItem(
                    role.Name,
                    role.Id,
                    userRoles.Any(ur => ur.Contains(role.Name))
                )
            ).ToList();

            // Crea un objeto EditUser con el usuario y los roles asociados y lo establece como UserModel.
            var userModel = new EditUser
            {
                User = user,
                Roles = roleItems
            };

            UserModel = userModel;

            // Retorna la página.
            return Page();
        }

        // Método para la solicitud POST de la página, recibe el modelo EditUser.
        public async Task<IActionResult> OnPostAsync(EditUser UserModel)
        {
            // Obtiene el usuario desde la Unidad de Trabajo utilizando el ID del usuario en el modelo EditUser.
            var user = _unidad.User.GetUser(UserModel.User.Id);

            // Si el usuario no existe, retorna un error 404.
            if (user == null)
            {
                return NotFound();
            }

            // Obtiene los roles asignados al usuario actual.
            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);

            // Crea listas para almacenar los roles a agregar y quitar.
            var rolesToAdd = new List<string>();
            var rolesToRemove = new List<string>();

            foreach (var role in UserModel.Roles)
            {
                // Busca el rol en los roles asignados al usuario actual.
                var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Text);

                // Si el rol está seleccionado en el modelo EditUser y no está asignado al usuario, lo agrega a la lista de roles a agregar.
                if (role.Selected)
                {
                    if (assignedInDb == null)
                    {
                        rolesToAdd.Add(role.Text);
                    }
                }
                // Si el rol no está seleccionado en el modelo EditUser y está asignado al usuario, lo agrega a la lista de roles a quitar.
                else
                {
                    if (assignedInDb != null)
                    {
                        rolesToRemove.Add(role.Text);
                    }
                }
            }

            // Agrega los roles a agregar al usuario utilizando el UserManager del SignInManager.
            if (rolesToAdd.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }

            // Quita los roles a quitar del usuario utilizando el UserManager del SignInManager.
            if (rolesToRemove.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            // Actualiza el nombre de usuario, correo electrónico y número de teléfono del usuario.
            user.UserName = UserModel.User.UserName;
            user.Email = UserModel.User.Email;
            user.PhoneNumber = UserModel.User.PhoneNumber;

            // Actualiza el usuario en la Unidad de Trabajo.
            _unidad.User.actualizar(user);

            // Redirige a la página Index.
            return RedirectToPage("./Index");
        }
    }
}
