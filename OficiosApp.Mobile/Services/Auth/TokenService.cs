// Services/ITokenStorage.cs
public interface ITokenStorage
{
    Task SetTokenAsync(string token);
    Task<string?> GetTokenAsync();
    void RemoveToken();
}

// Services/TokenStorage.cs
public class TokenStorage : ITokenStorage
{
    private const string TokenKey = "auth_token";

    public async Task SetTokenAsync(string token) =>
        await SecureStorage.Default.SetAsync(TokenKey, token);

    public async Task<string?> GetTokenAsync() =>
        await SecureStorage.Default.GetAsync(TokenKey);

    public void RemoveToken() =>
        SecureStorage.Default.Remove(TokenKey);
}