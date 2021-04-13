using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Api.Identidade.Extension
{
    public class AppSettings
    {
        public string Secret { get; set; } // chave de criptografia
        public int ExpiracaoHoras { get; set; } // horas da validade
        public string Emissor { get; set; }
        public string ValidoEm { get; set; } // urls q token é valido
    }
}
