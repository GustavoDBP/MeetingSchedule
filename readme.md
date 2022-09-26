
# Clima Tempo Simples

Este projeto tem por objetivo organizar o agendamento de salas de reuniões.
O projeto ainda não está finalizado, porém já se encontra em um estado funcional.

## Execução do sistema

### API
Na pasta raíz do projeto da API (a mesma que contém a solução), atualize o banco de dados SQL Server com o seguinte comando:

```power shell
  dotnet ef database update
```

O projeto já possui um Migration preparado com um inicializador de dados, portanto o banco será criado e populado automaticamente.

Para adicionar/alterar dados do banco de dados, pode-se acessá-lo manualmente, ou editar o arquivo "ClimaTempoSimplesBuilderExtensions.cs", localizado em
``` path
  ...ClimaTempo\DAL\ClimaTempoSimplesBuilderExtensions.cs
```

O método Seed é responsável por popular o banco de dados inicialmente.

Após a alteração, é necessário criar um novo Migration com o comando:

``` Power Shell
  dotnet ef migrations add [MigrationName]
```

E, novamente, é necessário atualizar o banco de dados com o seguinte comando:
```Power Shell
  dotnet ef database update
```

Para iniciar o servidor da aplicação, é necessário executar o comando "build" no projeto. Para isso, abra-o no visual studio (ou em alguma IDE compatível) e pressione Ctrl+Shift+B. Também é possível executar a mesma ação clicando com o botão direito do mouse em cima do projeto ouda solução e clicando em "Build"ou "Build Solution".

Após isso, você pode iniciar o aplicativo pela própria IDE, pressionando Ctrl+F5, ou clicando no botão de iniciar, localizado na barra superior.
Outra forma de iniciar o sistema é acessando a pasta de arquivos binários gerada na ação anterior e executando o arquivo ClimaTempo.exe.

### Front-end

Na pasta "...\Front-end\meeting-schedule", inicie o servidor React.js com o seguinte comando:
```
    npm run start
```

Após isso, é possível acessar o sistema no endereço padrão "http://localhost:3000/"
## Telas
A principal tela do sistema é a visão geral do mês. Nela, o usuário consegue visualizar todos os agendamentos de forma simples e intuitiva, além de ter um fácil acesso às principais ações do sistema.

### Funcionalidades
    - Agendar uma sala
    - Editar um agendamento
    - Remover um agendamento

### A fazer
    - Tratamento de erro
    - Autenticação (em andamento)
    - Visualização dos horários conflitantes
    - Testes Unitários
    - GraphQL
    - Configurar a conteinerização com o Docker
