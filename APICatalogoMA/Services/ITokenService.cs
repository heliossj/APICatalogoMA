using APICatalogoMA.Models;

namespace APICatalogoMA.Services;

public interface ITokenService
{
    string GerarToken(string key, string issuer, string audience, User user);
}
