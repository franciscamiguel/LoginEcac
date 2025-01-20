using System.Text;
using System.Text.Json;

namespace LoginEcac.Services;

/// <summary>
/// Serviço responsável por realizar login via gov.br
/// </summary>
/// <param name="httpClient"></param>
public class GovBrLoginService(HttpClient httpClient)
{
    /// <summary>
    /// Realiza login
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="accessToken"></param>
    public async Task LoginAsync(string username, string password, string accessToken)
    {
        const string loginUrl = "https://example-govbr-login-endpoint"; // Substitua pelo endpoint correto.

        var loginData = new { username, password };

        var content = new StringContent(
            JsonSerializer.Serialize(loginData),
            Encoding.UTF8,
            "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, loginUrl) { Content = content };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        // Enviando a requisição
        var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Login via Gov.br realizado com sucesso!");
            return;
        }

        Console.WriteLine($"Erro no login via Gov.br: {response.StatusCode}");
    }
}