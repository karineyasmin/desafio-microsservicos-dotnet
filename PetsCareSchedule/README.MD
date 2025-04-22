# PetsCareScheduler

## Sobre o Serviço
O PetsCareScheduler é um serviço de agendamento para cuidados com pets. Ele permite que os usuários criem, atualizem, visualizem e excluam agendamentos para diversos tipos de serviços de cuidados com animais de estimação, como consultas veterinárias, banhos e tosas.

## Estrutura do Projeto
```
PetsCareScheduler.sln
PetsCareScheduler.Service
  ├── appsettings.Development.json
  ├── appsettings.json
  ├── PetsCareScheduler.Service.csproj
  ├── PetsCareScheduler.Service.http
  ├── PetsCareScheduler.Service.sln
  ├── Program.cs
  ├── bin/
  ├── Debug/
  ├── Consumers/
  │   ├── PetCreatedConsumer.cs
  │   ├── PetInfoRequestConsumer.cs
  │   └── PetInfoResponseConsumer.cs
  ├── Controllers/
  │   └── ScheduleController.cs
  ├── DTOs/
  │   ├── AppointmentDto.cs
  │   └── ManualAppointmentRequestDTo.cs
  ├── Interfaces/
  ├── Models/
  ├── obj/
  ├── Properties/
  ├── Repositories/
  ├── Services/
  └── Settings/
```

## Configuração do appsettings.json
O arquivo `appsettings.json` contém as configurações necessárias para a aplicação, incluindo as configurações de conexão com o banco de dados MongoDB. Aqui está um exemplo de como configurar o `appsettings.json`:

```json
{
  "RabbitMqSettings": {
    "HostName": "localhost",
    "QueueName": "pets_queue",
    "UserName": "guest",
    "Password": "guest"
  },
  "MongoDbSettings": {
    "ConnectionString": "mongodb://user:password@localhost:27017",
    "DatabaseName": "PetsCareSchedulerDb"
  },
```

Certifique-se de atualizar a string de conexão `MongoDb` com as informações corretas do seu ambiente.

## Passo 2: Restaurar Dependências
Navegue até o diretório do projeto e restaure as dependências:
```
dotnet restore
```

## Passo 3: Configurar o Banco de Dados
Certifique-se de que o MongoDB está instalado e em execução. Atualize as configurações de conexão no arquivo `appsettings.json` conforme necessário.

## Passo 4: Executar a Aplicação
Execute a aplicação:
```
dotnet run
```
A API estará disponível em `http://localhost:5000/index.html`.

## Passo 5: Usar a API
Você pode usar ferramentas como Postman ou cURL para interagir com a API. A documentação Swagger também está disponível em `http://localhost:5000/index.html`.

## Contribuição
Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests.

## Licença
Este projeto está licenciado sob a MIT License.

## Contato
Para mais informações, entre em contato com Karine Ribeiro em karine.ribeiro@gft.com.