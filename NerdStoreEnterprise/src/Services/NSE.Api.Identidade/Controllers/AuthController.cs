using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Api.Identidade.Extension;
using NSE.Api.Identidade.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Api.Identidade.Controllers
{
    [Route("api/Identidade")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager; // Login
        private readonly UserManager<IdentityUser> _userManager; // Administra usuario
        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _userManager = userManager;
        }

        [HttpPost("CriandoAutenticacao")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            // Usuario do Identity
            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            // Criando Usuario
            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);
        
            // Gera o Token se for sucesso
            if(result.Succeeded)
            {
                // Logando usuario
                // await _signInManager.SignInAsync(user, false);
                return CustomResponse(await GerarJWT(usuarioRegistro.Email));
            }

            // Verifica se tem erro
            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();

        }

        [HttpPost("Autenticacao")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

                                                                 //email              //senha             
            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, 
                                                                 false, true); // persistente  //bloqueado

            if (result.Succeeded)
            {
                return CustomResponse(await GerarJWT(usuarioLogin.Email));
            }

            // Verifica se usuario estao bloqueado
            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou Senha Incorretas");
            return CustomResponse();

        }

        private async Task<UsuarioRespostaLogin> GerarJWT(string email)
        {
            // obter usuario
            var user = await _userManager.FindByEmailAsync(email);

            // Lista de Claims
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await ObterClaimUsuario(claims, user);

            // Token codificado com base na chave
            var encodeToken = CodificarToken(identityClaims);

            return ObterRespostaToken(encodeToken, user, claims);

        }

        private async Task<ClaimsIdentity> ObterClaimUsuario(ICollection<Claim> claims, IdentityUser user)
        {
            // Lista de Roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Adicionando na lista de claims
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));  // Id Token
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString())); // Token expirar
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)); // Token Emitido
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;

        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            // Gerando manipulador do token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            // Gerando Token
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);

        }

        private UsuarioRespostaLogin ObterRespostaToken(string encodeToken, IdentityUser user, IEnumerable<Claim> claims)
        {
            return new UsuarioRespostaLogin()
            {
                AccessToken = encodeToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = new UsuarioToken()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim() { Type = c.Type, Value = c.Value })
                }
            };
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
