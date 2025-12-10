using MyApiProject.Services;
using MyApiProject.contracts;
using MyApiProject.Models;


public interface ILogininterface
{
    public Task<RefreshToken?> Authenticate(LoginModel loginModel);

    

    public  Task<RefreshToken> SaveRefreshTokenAsync(int userId, string tokenValue);


   public Task<bool?> ValidateRefreshtokenAsync(int userid, string refreshtoken);

    
}