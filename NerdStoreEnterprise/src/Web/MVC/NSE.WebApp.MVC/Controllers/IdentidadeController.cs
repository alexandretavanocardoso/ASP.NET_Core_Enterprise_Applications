using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models.Identidade;
using NSE.WebApp.MVC.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet]
        [Route("criarAutenticacao")]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [Route("criarAutenticacao")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            // Comunicacao API
            var resposta = await _autenticacaoService.Registrar(usuarioRegistro);

            // Validando resposta
            if (ReponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

            // realizando o login
            await RealizarLogin(resposta);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("autenticacao")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("autenticacao")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return View(usuarioLogin);

            // Comunicacao API
            var resposta = await _autenticacaoService.Login(usuarioLogin);

            // Validando resposta
            if (ReponsePossuiErros(resposta.ResponseResult)) return View(usuarioLogin);

            // realizando o login
            await RealizarLogin(resposta);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("sair")]
        public async Task<ActionResult> Logout()
        {
            // SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme) = zera cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task RealizarLogin(UsuarioRespostaLogin resposta) 
        {
            // Obtendo token formatado em JwtSecurityToken
            var token = ObterTokenFormatado(resposta.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", resposta.AccessToken)); // Armazenando token na claim em "JWT"
            claims.AddRange(token.Claims); // Adicionando minha lista de claims que venho formatada em outra lsita

            // Gera claims dentro do Cookie, em formatado de cookies
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Propriedades do cookie
            var authProperties = new AuthenticationProperties() { 
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60), // O Cookie expira depois de quantos minutos
                IsPersistent = true // Se é persistente
            };

            // Logando 
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, // Formato Cookie
                new ClaimsPrincipal(claimsIdentity), // Claims usuario
                authProperties // propriedades do cookie
            );
        }

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}
