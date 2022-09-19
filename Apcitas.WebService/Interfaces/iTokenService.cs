using Apcitas.WebService.Entities;

namespace Apcitas.WebService.Interfaces;

public interface iTokenService
{
    public string CreateToken(AppUser user);
}
