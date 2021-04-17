using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
    public interface IUser
    {
        string Name { get; }
        Guid ObterUserId();
        string ObterUserEmail();
        string ObterUserToken();
        bool EstaAutenticado();
        bool PossuiRole(string role);
        IEnumerable<Claim> ObterClaims();
        HttpContext ObterHttpContext();
    }

    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AspNetUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Name => _httpContextAccessor.HttpContext.User.Identity.Name;

        public bool EstaAutenticado()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> ObterClaims()
        {
            return _httpContextAccessor.HttpContext.User.Claims;
        }

        public HttpContext ObterHttpContext()
        {
            return _httpContextAccessor.HttpContext;
        }

        public string ObterUserEmail()
        {
            return EstaAutenticado() ? _httpContextAccessor.HttpContext.User.GetUserEmail() : "";
        }

        public Guid ObterUserId()
        {
            return EstaAutenticado() ? Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string ObterUserToken()
        {
            return EstaAutenticado() ? _httpContextAccessor.HttpContext.User.GetUserToken() : "";
        }

        public bool PossuiRole(string role)
        {
            return _httpContextAccessor.HttpContext.User.IsInRole(role);
        }
    }

    public static class ClaimsPrincipalExtensions 
    { 
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null) throw new ArgumentException(nameof(claimsPrincipal));

            // da um find para recuperar o Id -> sub
            var claim = claimsPrincipal.FindFirst("sub");
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null) throw new ArgumentException(nameof(claimsPrincipal));

            // da um find para recuperar o emails
            var claim = claimsPrincipal.FindFirst("email");
            return claim?.Value;
        }

        public static string GetUserToken(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null) throw new ArgumentException(nameof(claimsPrincipal));

            // da um find para recuperar o Token
            var claim = claimsPrincipal.FindFirst("JWT");
            return claim?.Value;
        }
    }
}
