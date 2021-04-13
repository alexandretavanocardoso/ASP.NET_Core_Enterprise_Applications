using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Api.Identidade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Api.Identidade.Controllers
{
    [ApiController]
    [Route("api/Identidade")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager; // Login
        private readonly UserManager<IdentityUser> _userManager; // Administra usuario

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("Registrando")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return BadRequest();

            // Usuario do Identity
            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            // Criando Usuario
            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);
        
            if(result.Succeeded)
            {
                // Logando usuario
                await _signInManager.SignInAsync(user, false);
                return Ok();
            }

            return BadRequest();

        }

        [HttpPost("Logando")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return BadRequest();

                                                                 //email              //senha             
            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, 
                                                                 false, true); // persistente  //bloqueado

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();

        }
    }
}
