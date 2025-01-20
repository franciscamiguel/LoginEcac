# Login e-CAC

## Descrição

Este projeto é um sistema simples desenvolvido em **C#** e **.NET** que realiza login no e-CAC utilizando:
1. Login via **Gov.br** (usuário e senha).
2. Login via **Certificado Digital A1**.

---

## Pré-requisitos

**1. Solicitação de Credenciais:**

- O sistema requer a obtenção de credenciais aprovadas pelo governo para integração com o Gov.br. Este processo pode levar **72 horas ou mais**.
- Siga os passos abaixo para configurar as credenciais:
   1. Acesse o [link de solicitação](https://acesso.gov.br/roteiro-tecnico/solicitacaocredencial.html).
   2. Clique em "Iniciar" e entre com sua conta Gov.br.
   3. Preencha as informações obrigatórias do cadastro e do serviço.
   4. Submeta a solicitação e aguarde a aprovação, que pode levar até 72 horas.

**2. Credenciais de Teste:**
- Após aprovação:
   1. Acesse o [sistema de acompanhamento](https://acesso.gov.br/roteiro-tecnico/solicitacaocredencial.html).
   2. Localize o protocolo aprovado e preencha as informações técnicas necessárias.
   3. Baixe, assine digitalmente e envie o documento solicitado.
   4. A credencial será anexada ao protocolo em até 72 horas.

**3. Informações Necessárias:**
- **Client ID** e **Client Secret** fornecidos pelo Gov.br.
- Certificado Digital A1 válido (arquivo `.pfx`) com senha.
- URL de redirecionamento registrada.

**4. Dependências do Sistema:**
- **SDK do .NET 9 ou superior** instalado.

---

## Configuração

1. Clone o repositório:
   ```bash
   git clone https://github.com/seuprojeto/LoginEcac.git
   cd LoginEcac
   
2. Configure as variáveis no arquivo `TokenService.cs`:
   ```csharp
   private const string ClientId = "<seu-client-id>";
   private const string ClientSecret = "<seu-client-secret>";
   private const string RedirectUri = "<sua-url-de-retorno>";
   ```
3. Instale as dependências do projeto:
   ```bash
   dotnet restore

---

## Execução

1. Execute o projeto
   ```bash
   dotnet watch run

2. Escolha a opção desejada no menu:
   - Opção 1: Login via Gov.br (usuário e senha).
   - Opção 2: Login via Certificado Digital A1.

3. Siga as instruções apresentadas no console.

---

## Funcionalidades
- Login via Gov.br:
  1. Obtém o token de acesso.
  2. Realiza autenticação e exibe o status do login.

- Login via Certificado Digital A1:
  1. Utiliza o certificado para autenticar no e-CAC via requisição HTTPS segura.
  2. Retorna a resposta do servidor para validação.

---

## Estrutura do Projeto

- `TokenService.cs`: Responsável por autenticar com o Gov.br e obter o token de acesso.
- `GovBrLoginService.cs`: Realiza login utilizando username, senha e token.
- `CertificateLoginService.cs`: Realiza login utilizando o certificado digital A1.

---

## Observações

1. **Segurança**: Nunca exponha `ClientSecret` e `AccessToken` em logs ou interfaces públicas.
2. **Validade do Token**: Certifique-se de que o token obtido ainda está válido antes de usá-lo em requisições.

