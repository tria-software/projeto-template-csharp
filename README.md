# Tria Software Projeto Template

Este é o template `ProjetoTemplate` criado para aplicações C# baseadas em uma estrutura de projeto organizada em camadas.

## Estrutura do Projeto

A estrutura criada pelo template é a seguinte:

```
ProjetoTemplate/
├── ProjetoTemplate.API/
│ ├── appsettings.Development.json
│ ├── appsettings.json
│ ├── Dockerfile
│ ├── Program.cs
│ ├── ProjetoTemplate.API.csproj/
│ ├── web.config
│ ├── Configuration/
│ ├── Controllers/
│ │ └── Base/
│ ├── Properties/
│ └── wwwroot/
│ ├── Storage/
│ └── TemplateEmail/
│
├── ProjetoTemplate.BL/
│ ├── Authentication/
│ ├── AzureStorage/
│ ├── Excel/
│ ├── FindCep/
│ ├── Jwt/
│ ├── LayoutColumns/
│ ├── Profile/
│ ├── Security/
│ └── SendEmail/
│
├── ProjetoTemplate.Domain/
│ ├── DTO/
│ ├── Helpers/
│ └── Models/
│
├── ProjetoTemplate.Repository/
│ ├── Migrations/
│ └── ModelsConfiguration/
├── .gitattributes
├── .gitignore
├── LICENSE.txt
├── ProjetoTemplate.sln
└── README.md
```

## Descrição dos Módulos

### `.template.config`

Configurações do template do projeto, incluindo o arquivo `template.json`, que define como o projeto deve ser gerado.

## `ProjetoTemplate.API`

Módulo da API.

- **appsettings.Development.json**: Configurações de ambiente de desenvolvimento.
- **appsettings.json**: Configurações gerais da aplicação.
- **Dockerfile**: Arquivo Docker para containerização da aplicação.
- **Program.cs**: Ponto de entrada da aplicação.
- **ProjetoTemplate.API.csproj**: Arquivo de configuração do projeto API.
- **web.config**: Configurações para o servidor web (IIS).

### `Configuration`

Configurações da API.

### `Controllers`

Controladores da API.

#### `Base`

Controladores base.

### `Properties`

Propriedades do projeto.

### `wwwroot`

Arquivos estáticos.

- **Storage**: Diretório de armazenamento temporário.
- **TemplateEmail**: Templates de e-mail HTML.

## `ProjetoTemplate.BL`

Módulo de Lógica de Negócios (Business Logic).

- **Authentication**: Regras de autenticação.
- **AzureStorage**: Integração com o Azure Storage.
- **Excel**: Funções de manipulação de arquivos Excel.
- **FindCep**: Integração para busca de CEPs.
- **Jwt**: Configurações e manipulação de JWTs.
- **LayoutColumns**: Configurações de layout.
- **Profile**: Regras de negócios para perfis de usuários.
- **Security**: Regras de segurança.
- **SendEmail**: Funcionalidade de envio de e-mails.

## `ProjetoTemplate.Domain`

Módulo de Domínio.

- **DTO**: Objetos de Transferência de Dados (DTOs).
- **Helpers**: Classes auxiliares e funções utilitárias.
- **Models**: Modelos de domínio (entidades).

## `ProjetoTemplate.Repository`

Repositório e contexto do banco de dados.

- **Migrations**: Arquivos de migração do banco de dados.
- **ModelsConfiguration**: Configurações dos modelos do banco de dados.

## Arquivos Gerais

- **.gitattributes**: Configurações de atributos do Git.
- **.gitignore**: Arquivo para especificar quais arquivos e pastas devem ser ignorados pelo Git.
- **LICENSE.txt**: Licença do projeto.
- **ProjetoTemplate.sln**: Arquivo de solução do Visual Studio.
- **README.md**: Documento de descrição do projeto.

## Como Rodar e Testar a Criação de Templates

### Pré-requisitos

Antes de rodar o projeto localmente, certifique-se de que você tenha instalado:

- [.NET SDK](https://dotnet.microsoft.com/download) (versão compatível com o projeto)

### Passos para Testar a Criação de Templates (Projeto Local)

1. **Clone o repositório**:

   No terminal, execute o seguinte comando para clonar o repositório localmente:

   ```bash
   git clone https://github.com/tria-software/projeto-template-csharp.git
   ```

2. **Entre no diretório do projeto**:

   ```bash
   cd projeto-template-csharp
   ```

3. **Instalar o Template no seu Ambiente Local**:

   Para instalar o template localmente, execute o seguinte comando no diretório raiz do projeto:

   ```bash
   dotnet new -i .
   ```

   Isso instalará o template que está definido no arquivo `template.json` e fará com que ele fique disponível para uso localmente.

4. **Criar um Novo Projeto Usando o Template**:

   Após a instalação, você pode criar um novo projeto baseado no template. Execute o seguinte comando em qualquer diretório onde você deseja criar o novo projeto:

   ```bash
   dotnet new projetotemplate -n MeuNovoProjeto
   ```

   Isso criará um novo projeto chamado `MeuNovoProjeto` usando o template que você configurou.

5. **Testar o Novo Projeto**:

   Navegue até o diretório do novo projeto e compile para verificar se tudo está funcionando corretamente:

   ```bash
   cd MeuNovoProjeto
   dotnet build
   ```

6. **Rodar o Novo Projeto**:

   Após garantir que o novo projeto foi criado corretamente, execute-o:

   ```bash
   dotnet run
   ```

   Isso iniciará o novo projeto gerado a partir do template e o servidor estará rodando localmente.

### Passos para Testar a Criação de Templates (Pacote NuGet)

1. **Instalar o Template no seu Ambiente Local**:

   Para instalar o template localmente, execute o seguinte comando no diretório raiz do projeto:

   ```bash
   dotnet new -i TriaSoftware.ProjetoTemplate.Teste
   ```

   Isso instalará o template que está definido no arquivo `template.json` e fará com que ele fique disponível para uso localmente.

2. **Criar um Novo Projeto Usando o Template**:

   Após a instalação, você pode criar um novo projeto baseado no template. Execute o seguinte comando em qualquer diretório onde você deseja criar o novo projeto:

   ```bash
   dotnet new projetotemplate -n MeuNovoProjeto
   ```

   Isso criará um novo projeto chamado `MeuNovoProjeto` usando o template que você configurou.

3. **Testar o Novo Projeto**:

   Navegue até o diretório do novo projeto e compile para verificar se tudo está funcionando corretamente:

   ```bash
   cd MeuNovoProjeto
   dotnet build
   ```

4. **Rodar o Novo Projeto**:

   Após garantir que o novo projeto foi criado corretamente, execute-o:

   ```bash
   dotnet run
   ```

   Isso iniciará o novo projeto gerado a partir do template e o servidor estará rodando localmente.

### Passos para Testar a Criação de Entidades

1. **Criar uma Entidade Projeto Usando o Template**:

   Após a instalação, você pode criar uma nova entidade baseado no template. Execute o seguinte comando dentro da pasta do projeto criado com o template:

   ```bash
   dotnet new projetotemplate -n MeuProjeto -E MinhaNovaEntidade
   ```

   Isso criará uma nova entidade chamada `MeuNovoProjeto` usando o template que você configurou.

## Como Criar Entidades Usando o Template

O template permite a criação de entidades personalizadas (como `EntityName`) com controladores, serviços, modelos, DTOs e configurações do modelo, de acordo com os parâmetros definidos.

### Parâmetros Disponíveis para Criação de Entidades

- **`EntityName`**: Nome da entidade que você deseja criar (substituirá "EntityName" no código gerado).
- **`IncludeController`**: Incluir o controlador da entidade (padrão: `true`).
- **`IncludeService`**: Incluir o serviço da entidade (padrão: `true`).
- **`IncludeModel`**: Incluir o modelo da entidade (padrão: `true`).
- **`IncludeDTO`**: Incluir os DTOs da entidade (padrão: `true`).
- **`IncludeModelsConfig`**: Incluir a configuração dos modelos para o Entity Framework (padrão: `true`).

### Passos para Criar uma Nova Entidade

1. **Criar uma nova entidade**:

   Para criar uma nova entidade chamada, por exemplo, `Product`, execute o seguinte comando:

   ```bash
   dotnet new projetotemplate -e Product
   ```

   Este comando gera a entidade `Product` com base no template.

2. **Incluir arquivos opcionais**:

   Se desejar incluir ou excluir arquivos específicos, você pode ajustar os parâmetros:

   - Para **incluir/excluir o controlador**, use o parâmetro `-c`:

     ```bash
     dotnet new projetotemplate -e Product -c false
     ```

     (Neste caso, o controlador não será gerado.)

   - Para **incluir/excluir o serviço**, use o parâmetro `-s`:

     ```bash
     dotnet new projetotemplate -e Product -s false
     ```

     (Neste caso, o serviço não será gerado.)

   - Para **incluir/excluir o modelo**, use o parâmetro `-m`:

     ```bash
     dotnet new projetotemplate -e Product -m false
     ```

   - Para **incluir/excluir os DTOs**, use o parâmetro `-d`:

     ```bash
     dotnet new projetotemplate -e Product -d false
     ```

   - Para **incluir/excluir a configuração do modelo no Entity Framework**, use o parâmetro `-mc`:
     ```bash
     dotnet new projetotemplate -e Product -mc false
     ```

3. **Registrando a nova entidade no projeto**:

   - **Serviço (`Service`)**:
     Se você escolheu incluir o serviço, o template automaticamente gera o código necessário. No entanto, você ainda precisa registrar o serviço no `IocConfig.cs`. O template fornece uma ação pós-criação que adiciona o seguinte código no arquivo `IocConfig.cs`:

     ```csharp
     services.AddTransient<IProductBO, ProductBO>();
     ```

     Caso não seja automaticamente adicionado, você pode adicionar manualmente esse código abaixo do comentário `// Registro de BOs (Business Objects)`.

   - **Modelos do Entity Framework (`DbContext`)**:
     Se você incluiu a configuração do modelo, o `DbSet` da nova entidade será adicionado ao `ProjetoTemplateDbContext.cs`:

     ```csharp
     public virtual DbSet<Product> Product { get; set; }
     ```

     Isso será adicionado abaixo do comentário `// Add DbSet for each entity`. Caso o template não consiga adicionar automaticamente, insira manualmente esse código.

4. **Testar a nova entidade**:

   Após a geração da entidade, compile e rode o projeto:

   ```bash
   dotnet build
   dotnet run
   ```

   Sua entidade `Product` estará disponível, e o projeto estará rodando com todas as funcionalidades configuradas.

### Exemplo Completo

Para gerar um projeto chamado `MeuNovoProjeto` e incluir uma nova entidade chamada `Product`, com todos os arquivos opcionais (controlador, serviço, modelo, DTO e configuração do modelo), você pode usar o seguinte comando:

```bash
dotnet new projetotemplate -n MeuNovoProjeto -e Product
```

Se desejar excluir o controlador, por exemplo, você pode ajustar o comando assim:

```bash
dotnet new projetotemplate -n MeuNovoProjeto -e Product -c false
```

Isso criará o projeto com a entidade `Product`, mas sem gerar o controlador.

---

Essa documentação serve como um guia rápido para criar e configurar projetos e entidades usando o `Bug Office Project Template`.

### Limpar o Template Localmente (Projeto Local)

Se você quiser remover o template localmente, execute o seguinte comando:

```bash
dotnet new -u .
```

Isso desinstalará o template do ambiente local.

### Limpar o Template Localmente (Pacote NuGet)

Se você quiser remover o template localmente, execute o seguinte comando:

```bash
dotnet new -u TriaSoftware.ProjetoTemplate.Teste
```

Isso desinstalará o template do ambiente local.
