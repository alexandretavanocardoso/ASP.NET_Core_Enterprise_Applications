using NSE.WebApp.MVC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    // Serviço para ver erros de Response
    public abstract class Service
    {
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
