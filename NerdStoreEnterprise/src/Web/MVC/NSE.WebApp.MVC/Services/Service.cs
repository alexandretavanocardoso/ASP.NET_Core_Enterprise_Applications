using NSE.WebApp.MVC.Extensions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    // Serviço para ver erros de Response
    public abstract class Service
    {
        protected StringContent ObterConteudo(object dado) 
        {
            // retorna dado com formato string especifico
            return new StringContent( JsonSerializer.Serialize(dado),
                                      Encoding.UTF8,
                                      "application/json");
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            // Resolvendo problema de CaseSensitive do retorno Json (response)
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool TratarErrosResponse(HttpResponseMessage httpResponseMessage)
        {
            switch ((int)httpResponseMessage.StatusCode)
            {
                case 401: // Não Autorizado
                case 403: // Acesso Negado
                case 404: // Recurso Não encontrado
                case 500: // Erro no servidor
                    throw new CustomHttpRequestException(httpResponseMessage.StatusCode);

                case 400: // Erros no response
                    return false;
            }

            // garante que retornou um codigo de sucesso
            httpResponseMessage.EnsureSuccessStatusCode();

            return true;
        }
    }
}
