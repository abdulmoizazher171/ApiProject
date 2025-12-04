using MyApiProject.Services;
using MyApiProject.contracts;

public interface ILogininterface
{
    public AuthResponse Authenticate(LoginModel loginModel);

    

    public RefreshToken CreateRefreshToken(string userId);


    public  Task AddTokenAsync(RefreshToken token);

    public Task<RefreshToken?> ValidateRefreshTokenAsync(string tokenString);
}