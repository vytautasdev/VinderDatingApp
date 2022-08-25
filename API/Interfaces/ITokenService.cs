namespace API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Entities.AppUser user);
    }
}