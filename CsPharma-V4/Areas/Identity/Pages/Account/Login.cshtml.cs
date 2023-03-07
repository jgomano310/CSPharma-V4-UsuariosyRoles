// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CsPharma_V4.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager; // Sign in manager para autenticación de usuario
        private readonly ILogger<LoginModel> _logger; // Logger para registro de eventos

        // Constructor que inyecta SignInManager y ILogger
        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }


        [BindProperty] // Atributo que indica que InputModel será binded a la propiedad "Input" de la página
        public InputModel Input { get; set; }


        public IList<AuthenticationScheme> ExternalLogins { get; set; } // Lista de esquemas de autenticación externa

        public string ReturnUrl { get; set; } // URL de retorno después de la autenticación

        [TempData] // Atributo que indica que ErrorMessage será guardado en TempData
        public string ErrorMessage { get; set; }


        public class InputModel // Modelo de datos para los campos de entrada en la página
        {

            [Required] // Atributo que indica que el campo es requerido
            [EmailAddress] // Atributo que indica que el campo debe ser una dirección de correo electrónico
            public string Email { get; set; }


            [Required] // Atributo que indica que el campo es requerido
            [DataType(DataType.Password)] // Atributo que indica que el campo es de tipo contraseña
            public string Password { get; set; }


            [Display(Name = "Remember me?")] // Atributo que establece el nombre de la etiqueta del campo en la página
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage)) // Si hay un mensaje de error en TempData, lo añade al modelo de estado de la página
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/"); // Si la URL de retorno es nula, la establece a la raíz del sitio

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme); // Cierra cualquier sesión de autenticación externa

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(); // Obtiene una lista de esquemas de autenticación externa disponibles

            ReturnUrl = returnUrl; // Establece la URL de retorno
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/"); // Si la URL de retorno es nula, la establece a la raíz del sitio

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(); // Obtiene una lista de esquemas de autenticación externa disponibles

            if (ModelState.IsValid) // Si el modelo de datos es válido
            {

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false); // Intenta autenticar al usuario con las credenciales proporcionadas
                if (result.Succeeded) // Si la autenticación fue exitosa
                {
                    _logger.LogInformation("User logged in."); // Registra el evento de inicio de sesión
                    return LocalRedirect(returnUrl); // Redirige al usuario a la URL de retorno

                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
