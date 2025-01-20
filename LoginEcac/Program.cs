using System.Security.Cryptography.X509Certificates;
using LoginEcac.Services;

Console.WriteLine("Escolha a opção de login:");
Console.WriteLine("1 - Login via Gov.br (usuário e senha)");
Console.WriteLine("2 - Login via Certificado Digital A1");
var choice = Console.ReadLine();

var govBrTokenService = new TokenService(new HttpClient());
var accessToken = await govBrTokenService.GetAccessToken();

switch (choice)
{
    case "1":
    {
        await LoginGovBr();
        break;
    }
    case "2":
    {
        await LoginWithCertificate();
        break;
    }
    default:
        Console.WriteLine("Opção inválida.");
        break;
}

return;

#region Métodos Privados

async Task LoginGovBr()
{
    Console.Write("Digite o usuário: ");
    var username = Console.ReadLine();

    Console.Write("Digite a senha: ");
    var password = Console.ReadLine();

    var govBrService = new GovBrLoginService(new HttpClient());
    await govBrService.LoginAsync(username, password, accessToken);
}

async Task LoginWithCertificate()
{
    Console.Write("Digite ou cole o caminho do certificado A1 (arquivo .pfx): ");
    var certPath = Console.ReadLine();

    Console.Write("Digite a senha do certificado: ");
    var certPassword = Console.ReadLine();

    try
    {
        var certificate = new X509Certificate2(certPath, certPassword);
        var certService = new CertificateLoginService(certificate);
        await certService.LoginAsync(accessToken);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao carregar o certificado: {ex.Message}");
    }
}

#endregion