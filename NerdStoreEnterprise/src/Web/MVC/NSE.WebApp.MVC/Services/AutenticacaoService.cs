using NSE.WebApp.MVC.Models.Identidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            // retorna dado com formato string especifico
            var loginContent = new StringContent(
                JsonSerializer.Serialize(usuarioLogin), 
                Encoding.UTF8,
                "application/json");

            // Chamada para a api
            var response = await _httpClient.PostAsync("https://localhost:44396/api/Identidade/Autenticacao", loginContent);

            // Resolvendo problema de CaseSensitive do retorno Json
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // transformando o formato do response em string
            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<UsuarioRespostaLogin> Registrar(UsuarioRegistro usuarioRegistro)
        {
            // retorna dado com formato string especifico
            var registroContent = new StringContent(
                JsonSerializer.Serialize(usuarioRegistro),
                Encoding.UTF8,
                "application/json");

            // Chamada para a api
            var response = await _httpClient.PostAsync("https://localhost:44396/api/Identidade/CriandoAutenticacao", registroContent);

            // transformando o formato do response em string
            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync());
        }
    }
}
