// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CsPharma_V4.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager; // Objeto para gestionar el inicio de sesión
        private readonly ILogger<LogoutModel> _logger; // Objeto para registrar información de log

        public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager; // Inicializa el objeto de gestión de inicio de sesión
            _logger = logger; // Inicializa el objeto de registro de log
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync(); // Cierra la sesión del usuario
            _logger.LogInformation("User logged out."); // Registra en el log que el usuario ha cerrado sesión
            if (returnUrl != null) // Si se proporcionó una URL de retorno
            {
                return LocalRedirect(returnUrl); // Redirige al usuario a esa URL
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage(); // Redirige al usuario a la página principal
            }
        }
    }
}
