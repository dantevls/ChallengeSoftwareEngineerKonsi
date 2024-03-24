namespace Services.KonsiCredit.AuthAppService;

public interface IAuthAppService
{ 
    Task<string?> GetUserToken(string username, string password);
    
}