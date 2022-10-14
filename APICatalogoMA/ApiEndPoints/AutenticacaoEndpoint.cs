using APICatalogoMA.Models;
using APICatalogoMA.Services;
using Microsoft.AspNetCore.Authorization;

namespace APICatalogoMA.ApiEndPoints
{
    public static class AutenticacaoEndpoint
    {
        public static void MapAutenticacaoEndpoints(this WebApplication app)
        {
            app.MapPost("/login", [AllowAnonymous] (User user, ITokenService tokenService) =>
            {
                if (user == null)
                    return Results.BadRequest("Login inválido");

                if (user.UserName == "helio" && user.Password == "teste123")
                {
                    var tokenString = tokenService.GerarToken(app.Configuration["Jwt:key"],
                        app.Configuration["Jwt:Issuer"],
                        app.Configuration["Jwt:Audience"],
                        user);
                    return Results.Ok(new { token = tokenString });
                }
                else
                {
                    return Results.BadRequest("Login inválido");
                }
            }).Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("Login")
            .WithTags("Autenticacao");
        }
    }
}
