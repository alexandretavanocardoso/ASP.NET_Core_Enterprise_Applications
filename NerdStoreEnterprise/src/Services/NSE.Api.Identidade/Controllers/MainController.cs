using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Api.Identidade.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        // Coleção dos erros
        protected ICollection<string> Erros = new List<string>();

        protected ActionResult CustomResponse(object result = null) 
        {
            // Verifica se tem erro
            if (OperacaoValida())
            {
                return Ok(result);
            }

            // retornando mensagens de erros
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> 
            {
                { "Mesagens:", Erros.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected bool OperacaoValida()
        {
            return !Erros.Any();
        }

        protected void AdicionarErroProcessamento(string erro)
        {
            Erros.Add(erro);
        }

        protected void LimparErroProcessamento(string erro)
        {
            Erros.Clear();
        }
    }
}
