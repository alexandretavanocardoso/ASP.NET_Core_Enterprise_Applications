using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models.Identidade;
using NSE.WebApp.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : Controller
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
        [Route("autenticacao")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            // Comunicacao API
            var resposta = await _autenticacaoService.Registrar(usuarioRegistro);

            if (false) return View(usuarioRegistro);

            // Realizar Login

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("criarAutenticacao")]
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

            if (false) return View(usuarioLogin);

            // Realizar Login

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("sair")]
        public async Task<ActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
