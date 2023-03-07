// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace CsPharma_V4.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        // Se definen las dependencias necesarias para el registro de usuarios
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            // Se asignan las dependencias a las variables privadas
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        // Se define la clase InputModel que representará el modelo para la vista del registro de usuarios
        [BindProperty]
        public InputModel Input { get; set; }

        // Se define la propiedad ReturnUrl que se usará para redireccionar al usuario después de iniciar sesión
        public string ReturnUrl { get; set; }

        // Se define la lista de proveedores de autenticación externos
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // Clase InputModel para el modelo de la vista del registro de usuarios
        public class InputModel
        {
            // Anotación para indicar que este campo es requerido
            [Required]
            // Anotación para limitar la longitud de la cadena
            [StringLength(255, ErrorMessage = "El campo del nombre tiene un máximo de 255 caracteres", MinimumLength = 4)]
            // Anotación para especificar el nombre que se usará en las etiquetas HTML
            [Display(Name = "NombreUsuario")]
            public string NombreUsuario { get; set; }

            // Anotación para indicar que este campo es requerido
            [Required]
            // Anotación para limitar la longitud de la cadena
            [StringLength(255, ErrorMessage = "El campo de los apellidos tiene un máximo de 255 caracteres", MinimumLength = 4)]
            // Anotación para especificar el nombre que se usará en las etiquetas HTML
            [Display(Name = "ApellidosUsuario")]
            public string ApellidosUsuario { get; set; }

            // Anotación para indicar que este campo es requerido
            [Required]
            // Anotación para validar que el formato sea de correo electrónico
            [EmailAddress]
            // Anotación para especificar el nombre que se usará en las etiquetas HTML
            [Display(Name = "Email")]
            public string Email { get; set; }

            // Anotación para indicar que este campo es requerido
            [Required]
            // Anotación para limitar la longitud de la cadena
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            // Anotación para indicar que el campo es de tipo contraseña
            [DataType(DataType.Password)]
            // Anotación para especificar el nombre que se usará en las etiquetas HTML
            [Display(Name = "Password")]
            public string Password { get; set; }

            // Anotación para indicar que este campo es de tipo contraseña
            [DataType(DataType.Password)]
            // Anotación para especificar el nombre que se usará en las etiquetas HTML
            [Display(Name = "Confirm password")]
            // Anotación para validar que el campo sea igual al campo Password
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        // Método que se llama al cargar la página de registro
        public async Task OnGetAsync(string returnUrl = null)
        {
            // Se guarda el URL de retorno en la propiedad ReturnUrl
            ReturnUrl = returnUrl;
            // Se obtienen los esquemas de autenticación externa
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        // Método que se llama cuando se envía el formulario de registro
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Si el parámetro returnUrl es nulo, se le asigna un valor predeterminado
            returnUrl ??= Url.Content("~/");
            // Se obtienen los esquemas de autenticación externa
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // Se crea un nuevo usuario
                var user = CreateUser();

                // Se establece el nombre de usuario y correo electrónico del usuario
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {

                    _logger.LogInformation("User created a new account with password.");
                    //cuando accedamos seremos usuarios
                    var role = await _roleManager.RoleExistsAsync("Usuarios");

                    if (!role)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Usuarios"));
                    }

                    await _userManager.AddToRoleAsync(user, "Usuarios");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
