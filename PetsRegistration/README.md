# PetsRegistration Microservice

## Visão Geral

O microserviço PetsRegistration é responsável pelo registro e gerenciamento de informações de animais de estimação. Ele é composto por três principais componentes:

1. **AuthAndIdentity**: Gerencia a autenticação e identidade dos usuários.
2. **ExternalPetsApi**: Integra com APIs externas para obter informações adicionais sobre os animais de estimação.
3. **PetsRegistration.Api**: Fornece endpoints para o registro e gerenciamento de animais de estimação.

## Estrutura do Projeto

A estrutura do projeto é a seguinte:

```
PetsRegistration.sln
AuthAndIdentity/
	AuthAndIdentity.csproj
	bin/
		Debug/
	DTOS/
	Interfaces/
	Models/
	obj/
	Repositories/
	Services/
	Settings/
ExternalPetsApi/
	appsettings.json
	ExternalPetsAPi.csproj
	bin/
	Dtos/
	Interfaces/
	obj/
	Properties/
	Services/
	Settings/
PetsRegistration.Api/
	appsettings.Development.json
	appsettings.json
	PetsRegistration.Api.csproj
	PetsRegistration.Api.http
	Program.cs
	bin/
	Consumers/
	Controllers/
	Dtos/
	Interfaces/
	Models/
	obj/
	Properties/
	...
```

## Componentes

### AuthAndIdentity

Este componente gerencia a autenticação e identidade dos usuários. Ele contém:

- **DTOS**: Objetos de transferência de dados.
- **Interfaces**: Interfaces para os serviços.
- **Models**: Modelos de dados.
- **Repositories**: Repositórios para acesso a dados.
- **Services**: Serviços de autenticação e identidade.
- **Settings**: Configurações específicas do componente.

### ExternalPetsApi

Este componente integra com APIs externas para obter informações adicionais sobre os animais de estimação. Ele contém:

- **Dtos**: Objetos de transferência de dados.
- **Interfaces**: Interfaces para os serviços.
- **Properties**: Propriedades do projeto.
- **Services**: Serviços de integração com APIs externas.
- **Settings**: Configurações específicas do componente.

### PetsRegistration.Api

Este componente fornece endpoints para o registro e gerenciamento de animais de estimação. Ele contém:

- **Consumers**: Consumidores de mensagens.
- **Controllers**: Controladores de API.
- **Dtos**: Objetos de transferência de dados.
- **Interfaces**: Interfaces para os serviços.
- **Models**: Modelos de dados.
- **Properties**: Propriedades do projeto.

## Configuração

### AuthAndIdentity

Para configurar o componente AuthAndIdentity, edite o arquivo `appsettings.json` localizado no diretório `Settings`.

### ExternalPetsApi

Para configurar o componente ExternalPetsApi, edite o arquivo `appsettings.json` localizado no diretório raiz do componente.

### PetsRegistration.Api

Para configurar o componente PetsRegistration.Api, edite os arquivos `appsettings.json` e `appsettings.Development.json` localizados no diretório raiz do componente.

#### Configuração da Chave da API

Adicione a chave da API no arquivo `appsettings.json` do `PetsRegistration.Api`:

```json
{
  "ExternalPetsApiKey": "sua_chave_api_aqui"
}
```

## Executando o Projeto

Para executar o projeto, siga os passos abaixo:

1. Abra o arquivo de solução `PetsRegistration.sln` no Visual Studio.
2. Restaure os pacotes NuGet.
3. Compile a solução.
4. Execute o projeto `PetsRegistration.Api`.
5. O endereço para acessar o serviço é `http://localhost:5264/index.html`.

## Uso do Token JWT

Para acessar os endpoints protegidos, você precisa autenticar-se e obter um token JWT. Siga os passos abaixo:

1. Faça uma requisição POST para o endpoint `/api/Auth/login` com o corpo da requisição contendo o `username` e `password`.
2. O endpoint retornará um token JWT.
3. No Swagger, clique no ícone de cadeado no canto superior direito.
4. Insira o token JWT no campo de autorização, precedido pela palavra `Bearer`. Por exemplo: `Bearer seu_token_jwt`.

## Contribuindo

Se você deseja contribuir para este projeto, por favor, siga as diretrizes de contribuição:

1. Faça um fork do repositório.
2. Crie uma nova branch para a sua feature ou correção de bug.
3. Faça commit das suas alterações.
4. Envie um pull request.

## Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

## Contato

Para mais informações, entre em contato com a equipe de desenvolvimento.

---

Este README fornece uma visão geral do microserviço PetsRegistration, incluindo sua estrutura, componentes, configuração e instruções de execução.