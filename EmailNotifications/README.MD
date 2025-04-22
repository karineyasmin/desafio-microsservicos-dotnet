# Email Notifications

## Sobre o Projeto

Este projeto é uma aplicação para enviar notificações por email. Ele permite que os usuários recebam um email após o agendamento no serviço de agendamentos de cuidados para o pet.

## Como Utilizar

### Pré-requisitos

- Conta de email configurada para envio (ex: Gmail, Outlook)
- Serviço PetsCareScheduler rodando.

### Instalação

1. Clone o repositório:
   
2. Navegue até o diretório do projeto:
    ```bash
    cd email-notifications
    ```

3. Configure o email remetente no arquivo `appsettings.json`:
    ```json
    {
      "EmailSettings": {
       "SmtpServer": "smtp.gmail.com",
        "Port": "587",
        "Username": "seuemail@gmail.com",
        "Password": "sua senha"
      }
    }
    ```

4. Após executar o serviço de agendamentos, execute o serviço de email com:
    ```bash
    dotnet run
    ```
   **Não é necessário abrir página em localhost.**


### Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -am 'Adicionei uma nova feature'`)
4. Faça push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

### Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
