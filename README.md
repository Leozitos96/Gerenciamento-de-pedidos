Sistema de Gerenciamento de Pedidos


Sobre o Projeto
Este projeto é um sistema de gerenciamento de pedidos desenvolvido em C#, que utiliza um banco de dados SQLite para armazenar informações sobre clientes, produtos e pedidos. Ele permite a realização de operações básicas de cadastro, leitura, atualização e exclusão (CRUD), garantindo a organização e o controle dos dados.


Tecnologias Utilizadas:

C# – Linguagem principal do sistema.
.NET – Plataforma usada para desenvolvimento.
SQLite – Banco de dados utilizado para armazenar as informações.


Estrutura do Banco de Dados:

O sistema utiliza um banco de dados relacional, organizado em três tabelas principais:

Clientes – Armazena os dados dos clientes.
Produtos – Contém os produtos disponíveis para pedidos.
Pedidos – Registra os pedidos feitos pelos clientes, relacionando-os aos produtos.
Cada tabela está conectada através de chaves estrangeiras, garantindo a integridade dos dados.


Funcionalidades:

Cadastrar novos clientes, produtos e pedidos.
Listar registros armazenados no banco de dados.
Atualizar informações de clientes, produtos e pedidos.
Excluir produtos e clientes.


Como Usar:

Baixe ou clone este repositório ultilizando:
``` bash
$git clone https://github.com/Leozitos96/Gerenciamento-de-pedidos.git
```
Configure o SQLite no seu ambiente ultilizando:
``` bash
$dotnet package add Microsoft.Data.Sqlite
```
Execute o projeto e utilize as opções disponíveis no menu para gerenciar os pedidos, ultilizando:
``` bash
$dotnet run
```

``` 
Feito por: Ricardo, Leonardo e Daniel Sampaio
```
