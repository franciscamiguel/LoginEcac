using System.Text;
using System.Text.Json;
using System.Web;

namespace LoginEcac.Services;

/// <summary>
/// Classe responsável por gerar o token para envia no header da requisição no momento de fazer o login
/// </summary>
/// <param name="httpClient"></param>
public class TokenService(HttpClient httpClient)
{
    private const string UrlProvider = "https://sso.staging.acesso.gov.br";
    private const string ClientId = "<seu-client-id>";
    private const string ClientSecret = "<seu-client-secret>";
    private const string RedirectUri = "<sua-url-de-retorno>";
    private const string Scopes = "openid email profile govbr_empresa govbr_confiabilidades";

    /// <summary>
    /// Gera token para conseguir fazer login
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetAccessToken()
    {
        Console.WriteLine("URL para autenticação:");
        var authUrl = GetAuthorizationUrl();
        Console.WriteLine(authUrl);

        Console.WriteLine("Cole o código de autorização retornado:");
        var authorizationCode = Console.ReadLine();

        Console.WriteLine("Digite o Code Verifier utilizado:");
        var codeVerifier = Console.ReadLine();

        try
        {
            var accessToken = await ExchangeCodeForTokenAsync(authorizationCode!, codeVerifier!);
            Console.WriteLine($"Access Token obtido com sucesso: {accessToken}");

            return accessToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            throw;
        }
    }
    
    /// <summary>
    /// Constrói os escopos necessários e adiciona na url para geração do token
    /// </summary>
    /// <returns></returns>
    private static string GetAuthorizationUrl()
    {
        var state = GenerateRandomString();
        var nonce = GenerateRandomString();
        var codeChallenge = GenerateCodeChallenge();

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["response_type"] = "code";
        query["client_id"] = ClientId;
        query["scope"] = Scopes;
        query["redirect_uri"] = RedirectUri;
        query["nonce"] = nonce;
        query["state"] = state;
        query["code_challenge"] = codeChallenge;
        query["code_challenge_method"] = "S256";

        return $"{UrlProvider}/authorize?{query}";
    }

    /// <summary>
    /// Gera o token com os escopos necessários
    /// </summary>
    /// <param name="authorizationCode"></param>
    /// <param name="codeVerifier"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async Task<string> ExchangeCodeForTokenAsync(string authorizationCode, string codeVerifier)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", authorizationCode),
            new KeyValuePair<string, string>("redirect_uri", RedirectUri),
            new KeyValuePair<string, string>("code_verifier", codeVerifier)
        });

        var request = new HttpRequestMessage(HttpMethod.Post, $"{UrlProvider}/token")
        {
            Content = content,
            Headers =
            {
                { "Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"))}" }
            }
        };

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Erro ao trocar o código pelo token: {response.StatusCode}");

        var responseBody = await response.Content.ReadAsStringAsync();
        var tokenData = JsonSerializer.Deserialize<TokenResponse>(responseBody);

        return tokenData?.AccessToken ?? throw new Exception("Token não encontrado na resposta.");
    }

    private static string GenerateRandomString()
    {
        var random = new Random();
        var bytes = new byte[32];
        random.NextBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string GenerateCodeChallenge() 
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(GenerateRandomString()));

    private class TokenResponse
    {
        public string AccessToken { get; init; }
        public string IdToken { get; init; }
    }
}
