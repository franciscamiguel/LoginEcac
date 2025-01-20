using System.Security.Cryptography.X509Certificates;

namespace LoginEcac.Services;

/// <summary>
/// Serviço responsável por realizar login via certificado
/// </summary>
public class CertificateLoginService
{
    private readonly HttpClient _httpClient;

    public CertificateLoginService(X509Certificate2 certificate)
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(certificate);

        _httpClient = new HttpClient(handler);
    }

    /// <summary>
    /// Realiza login
    /// </summary>
    /// <param name="accessToken"></param>
    public async Task LoginAsync(string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.staging.acesso.gov.br");
        Console.WriteLine($"Iniciando login no e-CAC em: {request.RequestUri?.AbsoluteUri}");
        
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Login via Certificado Digital A1 realizado com sucesso!");
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Resposta do servidor:");
            Console.WriteLine(responseContent);
            return;
        }

        Console.WriteLine($"Erro no login via Certificado Digital A1: {response.StatusCode}");
    }
}
