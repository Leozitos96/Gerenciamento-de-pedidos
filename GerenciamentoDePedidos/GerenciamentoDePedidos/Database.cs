using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace GerenciamentoDePedidos
{
    public class Database
    {
        public void CriarTabelas()
        {
            string conectar = "Data Source=database.db";
            using (var conexao = new SqliteConnection(conectar))
            {
                conexao.Open();
                string table = @"CREATE TABLE IF NOT EXISTS Clientes(
                                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                 Nome TEXT NOT NULL,
                                 Email TEXT UNIQUE NOT NULL);

                             CREATE TABLE IF NOT EXISTS Produtos(
                                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                 Nome TEXT NOT NULL,
                                 Preco REAL NOT NULL,
                                 Estoque INTEGER NOT NULL);

                             CREATE TABLE IF NOT EXISTS Pedidos(
                                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                 Clienteid INTEGER NOT NULL,
                                 Datapedido TEXT NOT NULL,
                                 FOREIGN KEY (Clienteid) REFERENCES Clientes(Id));

                             CREATE TABLE IF NOT EXISTS Itenspedido(
                                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                 Pedidoid INTEGER NOT NULL,
                                 Produtoid INTEGER NOT NULL,
                                 Quantidade INTEGER NOT NULL,
                                 Precototal REAL NOT NULL,
                                 FOREIGN KEY (Pedidoid) REFERENCES Pedidos(Id),
                                 FOREIGN KEY (Produtoid) REFERENCES Produtos(Id));
                             ";
                var comando = conexao.CreateCommand();
                comando.CommandText = table;
                comando.ExecuteNonQuery();
            }
        }
        private bool EmailValido(string email)
        {
            var emailValido = new EmailAddressAttribute();
            return emailValido.IsValid(email);
        }
        private bool IdClienteValido(int id)
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Clientes WHERE Id = @Id;";
                cmd.Parameters.AddWithValue("@Id", id);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        private bool IdPedidoValido(int id)
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Pedidos WHERE Id = @Id;";
                cmd.Parameters.AddWithValue("@Id", id);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        private bool ClientesNoBanco()
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Clientes;";
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        private bool ProdutosNoBanco()
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Produtos;";
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        private bool PedidosNoBanco()
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Pedidos;";
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        private bool IdProdutoValido(int id)
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Produtos WHERE Id = @Id;";
                cmd.Parameters.AddWithValue("@Id", id);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        public void CadastrarCliente()
        {
            while (true)
            {
                Console.Write("Digite o nome do cliente: ");
                string nome = Console.ReadLine();
                Console.Write("Digite o email do cliente: ");
                string email = Console.ReadLine();
                if (!(string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email))) {
                    if (EmailValido(email))
                    {
                        Cliente cliente = new Cliente(nome, email);
                        using (var conexao = new SqliteConnection("Data Source=database.db"))
                        {
                            conexao.Open();
                            var cmd = conexao.CreateCommand();
                            cmd.CommandText = "INSERT INTO Clientes (Nome, Email) VALUES (@Nome, @Email);";
                            cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                            cmd.Parameters.AddWithValue("@Email", cliente.Email);
                            cmd.ExecuteNonQuery();
                        }
                        Console.WriteLine($"Cliente {cliente.Nome} cadastrado com sucesso!");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Digite um Email valido!");
                    }
                }
                else
                {
                    Console.WriteLine("Campos obrigatorios!");
                }
            }
        }
        public void ListarClientes()
        {
            if (ClientesNoBanco())
            {
                using (var conexao = new SqliteConnection("Data Source=database.db"))
                {
                    conexao.Open();
                    var cmd = conexao.CreateCommand();
                    cmd.CommandText = "SELECT Id, Nome, Email FROM Clientes;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("Nenhum cliente existente!");
                        }
                        else
                        {
                            Console.WriteLine("Lista de clientes:");
                            while (reader.Read())
                            {
                                Console.WriteLine($"ID: {reader["Id"]}, Nome: {reader["Nome"]}, Email: {reader["Email"]}");
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Nenhum cliente cadastrado!");
            }
        }
        public void AtualizarEmailCliente()
        {
            if (ClientesNoBanco())
            {
                while (true)
                {
                    ListarClientes();
                    Console.Write("Digite o Id do Cliente: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Novo Email: ");
                    string email = Console.ReadLine();
                    if (IdClienteValido(id))
                    {
                        if (EmailValido(email))
                        {
                            using (var conexao = new SqliteConnection("Data Source=database.db"))
                            {
                                conexao.Open();
                                var cmd = conexao.CreateCommand();
                                cmd.CommandText = "UPDATE Clientes SET Email = @Email WHERE Id = @Id;";
                                cmd.Parameters.AddWithValue("@Id", id);
                                cmd.Parameters.AddWithValue("@Email", email);
                                cmd.ExecuteNonQuery();
                            }
                            Console.WriteLine("Email atualizado com sucesso!");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Digite um Email valido!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Digite um Id valido!");
                    }
                }
            }
            else
            {
                Console.WriteLine("Nenhum cliente cadastrado!");
            }
            
        }
        public void RemoverCliente()
        {
            if (ClientesNoBanco())
            {
                while (true)
                {
                    ListarClientes();
                    Console.Write("Digite o ID do Cliente: ");
                    int id = int.Parse(Console.ReadLine());
                    if (IdClienteValido(id))
                    {
                        using (var conexao = new SqliteConnection("Data Source=database.db"))
                        {
                            conexao.Open();
                            var cmd = conexao.CreateCommand();
                            cmd.CommandText = "DELETE FROM Itenspedido WHERE Pedidoid IN (SELECT Id FROM Pedidos WHERE Clienteid = @Clienteid);";
                            cmd.Parameters.AddWithValue("@Clienteid", id);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "DELETE FROM Pedidos WHERE Clienteid = @Clienteid;";
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "DELETE FROM Clientes WHERE Id = @Clienteid;";
                            cmd.ExecuteNonQuery();
                        }
                        Console.WriteLine("Cliente removido com sucesso!");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Digite um Id valido!");
                    }

                }
            }
            else
            {
                Console.WriteLine("Nenhum cliente cadasrado!");
            }
        }
        public void CadastrarProduto()
        {
            while (true) {
                Console.Write("Digite o nome do produto: ");
                string nome = Console.ReadLine();
                Console.Write("Digite o preço do produto: ");
                double preco = double.Parse(Console.ReadLine());
                Console.Write("Digite a quantidade de estoque do produto: ");
                int estoque = int.Parse(Console.ReadLine());

                if (!(string.IsNullOrEmpty(nome) || preco < 0 || estoque < 0))
                {
                    Produto produto = new Produto(nome, preco, estoque);
                    using (var conexao = new SqliteConnection("Data Source=database.db"))
                    {
                        conexao.Open();
                        var cmd = conexao.CreateCommand();
                        cmd.CommandText = "INSERT INTO Produtos (Nome, Preco, Estoque) VALUES (@Nome, @Preco, @Estoque);";
                        cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                        cmd.Parameters.AddWithValue("@Preco", produto.Preco);
                        cmd.Parameters.AddWithValue("@Estoque", produto.Estoque);
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine($"Produto {produto.Nome} com sucesso!");
                    return;
                }
                else
                {
                    Console.WriteLine("Campos obrigatorios!");
                }
            }
        }
        public void ListarProdutos()
        {
            if (ProdutosNoBanco())
            {
                Console.WriteLine("Nenhum produto cadastrado!");
            }
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT Id, Nome, Preco, Estoque FROM Produtos;";
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Nenhum produto existente!");
                    }
                    else
                    {
                        Console.WriteLine("Lista de produtos:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["Id"]}, Nome: {reader["Nome"]}, Preço: {reader["Preco"]}, Estoque: {reader["Estoque"]}");
                        }
                    }
                }
            }
        }
        public void AtualizarProduto()
        {
            if (ProdutosNoBanco()) 
            {
                while (true)
                {
                    ListarProdutos();
                    Console.Write("Digite o Id do Produto: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Digite o novo preço: ");
                    double preco = double.Parse(Console.ReadLine());
                    Console.Write("Digite a nova quantidade em estoque: ");
                    int estoque = int.Parse(Console.ReadLine());
                    if (IdProdutoValido(id))
                    {
                        if (!(preco < 0 || estoque < 0))
                        {
                            using (var conexao = new SqliteConnection("Data Source=database.db"))
                            {
                                conexao.Open();
                                var cmd = conexao.CreateCommand();
                                cmd.CommandText = "UPDATE Produtos SET Preco = @Preco, Estoque = @Estoque WHERE Id = @Id;";
                                cmd.Parameters.AddWithValue("@Id", id);
                                cmd.Parameters.AddWithValue("@Preco", preco);
                                cmd.Parameters.AddWithValue("@Estoque", estoque);
                                cmd.ExecuteNonQuery();
                            }
                            Console.WriteLine("Produto atualizado com sucesso!");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Digite um preço ou quantidade validas!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Digite um Id valido!");
                    }
                }
            }
            else
            {
                Console.WriteLine("Nenhum produto adicionado!");
            }
        }
        public void RemoverProduto()
        {
            if (ProdutosNoBanco())
            {
                while (true)
                {
                    ListarProdutos();
                    Console.Write("Digite o Id do produto: ");
                    int id = int.Parse(Console.ReadLine());
                    if (IdProdutoValido(id))
                    {
                        using (var conexao = new SqliteConnection("Data Source=database.db"))
                        {
                            conexao.Open();
                            var cmd = conexao.CreateCommand();
                            cmd.CommandText = "DELETE FROM Itenspedido WHERE Produtoid = @Produtoid;";
                            cmd.Parameters.AddWithValue("@Produtoid", id);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "DELETE FROM Produtos WHERE Id = @Id;";
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.ExecuteNonQuery();
                        }
                        Console.WriteLine("Produto removido com sucesso!");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Digite um Id valido!");
                    }
                }
            }
            else
            {
                Console.WriteLine("Nenhum produto adicionado!");
            }
        }
        private int QuantidadeEstoqueProduto(int idProduto)
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT Estoque FROM Produtos WHERE Id = @IdProduto;";
                cmd.Parameters.AddWithValue("@IdProduto", idProduto);

                object resultado = cmd.ExecuteScalar();
                return resultado != DBNull.Value ? Convert.ToInt32(resultado) : 0;
            }
        }
        private double PrecoEstoqueProduto(int idProduto)
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT Preco FROM Produtos WHERE Id = @IdProduto;";
                cmd.Parameters.AddWithValue("@IdProduto", idProduto);

                object resultado = cmd.ExecuteScalar();
                return resultado != DBNull.Value ? Convert.ToDouble(resultado) : 0;
            }
        }
        public void NovoPedido()
        {
            if (ClientesNoBanco())
            {
                while (true)
                {
                    ListarClientes();
                    Console.Write("Digite o id do cliente que esta fazendo o pedido: ");
                    int cliente = int.Parse(Console.ReadLine());
                    if (IdClienteValido(cliente))
                    {
                        DateTime data = DateTime.UtcNow;
                        using (var conexao = new SqliteConnection("Data Source=database.db"))
                        {
                            conexao.Open();
                            var cmd = conexao.CreateCommand();
                            cmd.CommandText = "INSERT INTO Pedidos (Clienteid, Datapedido) VALUES (@Cliente, @Datapedido);";
                            cmd.Parameters.AddWithValue("@Cliente", cliente);
                            cmd.Parameters.AddWithValue("@Datapedido", data);
                            cmd.ExecuteNonQuery();
                        }
                        Console.WriteLine("Novo pedido criado!");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Digite um Id valido!");
                    }
                }
            }
            else
            {
                Console.WriteLine("Nenhum cliente cadastrado!");
            }
        }
        public void ListarPedidos()
        {
            if (PedidosNoBanco())
            {
                using (var conexao = new SqliteConnection("Data Source=database.db"))
                {
                    conexao.Open();
                    var cmd = conexao.CreateCommand();
                    cmd.CommandText = "SELECT Pedidos.Id AS Pedidoid, Clienteid, Datapedido, Clientes.Nome FROM Pedidos INNER JOIN Clientes ON Clientes.id = Pedidos.Clienteid;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Lista de pedidos");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["Pedidoid"]}, ID_CLIENTE: {reader["Clienteid"]}, CLIENTE: {reader["Nome"]}, Data_DO_PEDIDO: {reader["Datapedido"]}");
                        }
                    }
                }
            }
            else
            {
              Console.WriteLine("Nenhum pedido cadastrado!");
            }
        }
        public void RetirarEstoque(int quantidade, int produtoid)
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE Produtos SET Estoque = Estoque - @Quantidade WHERE Id = @Produtoid;";
                cmd.Parameters.AddWithValue("@Quantidade", quantidade);
                cmd.Parameters.AddWithValue("@Produtoid", produtoid);
                cmd.ExecuteNonQuery();
            }
        }
        public void AdicionarProdutoPedido()
        {
            if (PedidosNoBanco() && ClientesNoBanco() && ProdutosNoBanco()) {
                while (true)
                {
                    ListarPedidos();
                    Console.WriteLine("Digite o id do pedido: ");
                    int pedidoid = Int32.Parse(Console.ReadLine());
                    if (IdPedidoValido(pedidoid))
                    {
                        ListarProdutos();
                        Console.Write("Digite o id do produto a ser adicionado: ");
                        int produtoid = Int32.Parse(Console.ReadLine());
                        if (IdProdutoValido(produtoid))
                        {
                            int estoque = QuantidadeEstoqueProduto(produtoid);
                            Console.Write("Selecione uma quantidade: ");
                            int quantidade = int.Parse(Console.ReadLine());
                            if (estoque > 0)
                            {
                                if (quantidade > 0)
                                {
                                    double precototal = 0;
                                    for (int i = 0; i < quantidade; i++)
                                    {
                                        precototal += PrecoEstoqueProduto(produtoid);
                                    }
                                    using (var conexao = new SqliteConnection("Data Source=database.db"))
                                    {
                                        conexao.Open();
                                        var cmd = conexao.CreateCommand();
                                        cmd.CommandText = "INSERT INTO Itenspedido (Pedidoid, Produtoid, Quantidade, Precototal) VALUES (@Pedidoid, @Produtoid, @Quantidade, @Precototal);";
                                        cmd.Parameters.AddWithValue("@Pedidoid", pedidoid);
                                        cmd.Parameters.AddWithValue("@Produtoid", produtoid);
                                        cmd.Parameters.AddWithValue("@Quantidade", quantidade);
                                        cmd.Parameters.AddWithValue("@Precototal", precototal);
                                        cmd.ExecuteNonQuery();
                                        RetirarEstoque(quantidade, produtoid);
                                        Console.WriteLine("Produto adicionado ao pedido, atualizando o estoque!");
                                        return;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Digite uma quantidade valida!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Produto sem estoque, adicione outro produto no pedido!");
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Digite um Id do produto valido!");

                        }
                    }
                    else
                    {
                        Console.WriteLine("Digite um Id do pedido valido!");
                    }
                }
            }
            else
            {
                Console.WriteLine("Pedidos, clientes ou produtos inexistentes!");
            }
            
        }

        public void ListarItensPedidos()
        {
            ListarPedidos();
            Console.WriteLine("Digite o id do pedido: ");
            int idpedido = Int32.Parse(Console.ReadLine());
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT Itenspedido.Id AS Itensid, Pedidoid, Produtoid, Quantidade, Precototal, Produtos.Nome FROM Itenspedido INNER JOIN Produtos ON Produtos.id = Itenspedido.Produtoid WHERE Itenspedido.Pedidoid = @Idpedido;";
                cmd.Parameters.AddWithValue("@Idpedido", idpedido);
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Lista de Produtos do Pedido");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Itensid"]}, ID_PEDIDO: {reader["Pedidoid"]}, PRODUTO: {reader["Nome"]}, QUANTIDADE: {reader["Quantidade"]}, PRECO_TOTAL: {reader["Precototal"]}");

                    }
                }
            }

        }

    }
}
