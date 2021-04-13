using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Api.Identidade.Models
{
    public class UsuarioRegistro
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        [EmailAddress(ErrorMessage = "Campo de e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres"), MinLength(6)]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "Senhas diferentes")]
        public string ConfirmaSenha { get; set; }
    }

    public class UsuarioLogin
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        [EmailAddress(ErrorMessage = "Campo de e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres"), MinLength(6)]
        public string Senha { get; set; }
    }
}
