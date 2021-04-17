using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Models.Identidade;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl);

            _httpClient = httpClient;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            // retorna dado com formato string especifico
            var loginContent = ObterConteudo(usuarioLogin);

            // Chamada para a api
            var response = await _httpClient.PostAsync("/api/Identidade/Autenticacao", loginContent);

            // Tratando Erros de Response
            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin()
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            // transformando o formato do response em string
            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task<UsuarioRespostaLogin> Registrar(UsuarioRegistro usuarioRegistro)
        {
            // retorna dado com formato string especifico
            var registroContent = ObterConteudo(usuarioRegistro);

            // Chamada para a api
            var response = await _httpClient.PostAsync("/api/Identidade/CriandoAutenticacao", registroContent);

            // Tratando Erros de Response
            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin()
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            // transformando o formato do response em string
            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }
    }
}
