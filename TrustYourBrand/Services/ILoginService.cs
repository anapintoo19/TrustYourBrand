using TrustYourBrand.Models;

public interface ILoginService
{
    Task<LoginResult> LoginAsync(LoginModel model);
    Task<bool> CheckUserExists(string phoneNumber);
    Task<ChangePinResult> ChangePinAsync(int userId, string newPin, string confirmNewPin);
}

