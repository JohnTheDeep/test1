using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WebApplication1.Models.Jwt;
using AuthOpt = WebApplication1.Configuartion.AuthenticationConfiguration;
namespace WebApplication1.JwtAuthorization
{
    public class JwtTokenAuthorization
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtTokenAuthorization> _logger;

        public JwtTokenAuthorization(RequestDelegate next, ILogger<JwtTokenAuthorization> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachAccountToContext(context, token);

            await _next(context);
        }
        private void attachAccountToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOpt.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOpt.AUDIENCE,
                    IssuerSigningKey = AuthOpt.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(AuthOpt.LIFE_TIME)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                context.Items["User"] = new JwtPerson("", "");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to auth...");
            }
        }
    }
}
