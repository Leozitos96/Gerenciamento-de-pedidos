using System;
using System.Collections.Generic;
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
        public Database()
        {

        }

        public static void CriarTabelas()
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

        public static void CadastrarCliente()
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
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
        }


        public static void ListarClientes()
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT Id, Nome, Email FROM Clientes;";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Nome: {reader["Nome"]}, Email: {reader["Email"]}");
                    }
                }
            }
        }

        public static void AtualizarEmailCliente()
        {
            Console.Write("ID do Cliente: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Novo Email: ");
            string email = Console.ReadLine();

            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "UPDATE Clientes SET Email = @Email WHERE Id = @Id;";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Email do cliente atualizado com sucesso!");
        }

        public static void RemoverCliente()
        {
            Console.Write("ID do Cliente: ");
            int id = int.Parse(Console.ReadLine());

            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "DELETE FROM Clientes WHERE Id = @Id;";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Cliente removido com sucesso!");
        }

        public static void CadastrarProduto()
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Preço: ");
            double preco = double.Parse(Console.ReadLine());
            Console.Write("Estoque: ");
            int estoque = int.Parse(Console.ReadLine());

            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "INSERT INTO Produtos (Nome, Preco, Estoque) VALUES (@Nome, @Preco, @Estoque);";
                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Preco", preco);
                cmd.Parameters.AddWithValue("@Estoque", estoque);
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Produto cadastrado com sucesso!");
        }

        public static void ListarProdutos()
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT Id, Nome, Preco, Estoque FROM Produtos;";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Nome: {reader["Nome"]}, Preço: {reader["Preco"]}, Estoque: {reader["Estoque"]}");
                    }
                }
            }
        }

        public static void AtualizarProduto()
        {
            Console.Write("ID do Produto: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Novo Preço: ");
            double preco = double.Parse(Console.ReadLine());
            Console.Write("Novo Estoque: ");
            int estoque = int.Parse(Console.ReadLine());

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
        }

        public static void RemoverProduto()
        {
            Console.Write("ID do Produto: ");
            int id = int.Parse(Console.ReadLine());
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "DELETE FROM Produtos WHERE Id = @Id;";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Produto removido com sucesso!");
        }

        private static int ClientesCadastrados(int cadastrados)
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT * Clientes;";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       cadastrados++;
                    }
                }
            }
            return cadastrados;
        }



        private static int ProdutosCadastrados(int cadastrados)
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT * FROM Produtos;";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cadastrados++;
                    }
                }
            }
            return cadastrados;
        }

        private static int QuantidadeEstoqueProduto(int idProduto)
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT Quantidade FROM Produtos WHERE Id = @IdProduto;";
                cmd.Parameters.AddWithValue("@IdProduto", idProduto);

                object resultado = cmd.ExecuteScalar();
                return resultado != DBNull.Value ? Convert.ToInt32(resultado) : 0;
            }
        }

        private static double PrecoEstoqueProduto(int idProduto)
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


        private static int PedidosCadastrados(int cadastrados)
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT * FROM Pedidos;";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cadastrados++;
                    }
                }
            }
            return cadastrados;
        }


        public static void NovoPedido()
        {
            ListarClientes();
            Console.Write("Digite o id do cliente que esta fazendo o pedido: ");
            int cliente = Int32.Parse(Console.ReadLine());
            int cadastrados = 0;
            ClientesCadastrados(cadastrados);
            if (!(cliente <= 0 || cliente > cadastrados)) {
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
            }
            else
            {
                Console.WriteLine("Cliente inexistente!");
            }
        }
        public static void ListarPedidos()
        {
            using (var conexao = new SqliteConnection("Data Source=database.db"))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT Id, Clienteid, Datapedido, Clientes.Nome FROM Pedidos INNER JOIN Clientes ON Clientes.id = Pedidos.Clienteid;";
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Lista de pedidos");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, ID_CLIENTE: {reader["Clienteid"]}, CLIENTE: {reader["Clientes.Nome"]}, Data_DO_PEDIDO: {reader["Datapedido"]}");
                    }
                }
            }
        }

        public static void AdicionarProdutoPedido()
        {
            int pedidosCadastrados = 0;
            PedidosCadastrados(pedidosCadastrados);
            int produtosCadastrados = 0;
            ProdutosCadastrados(produtosCadastrados);
            ListarPedidos();
            Console.WriteLine("Digite o id do pedido: ");
            int pedidoid = Int32.Parse(Console.ReadLine());
            if(!(pedidoid < 0 || pedidoid > pedidosCadastrados))
            {
                ListarProdutos();
                Console.Write("Digite o id do produto a ser adicionado: ");
                int produtoid = Int32.Parse(Console.ReadLine());
                if(!(produtoid < 0 || produtoid > produtosCadastrados))
                {
                    int estoque = QuantidadeEstoqueProduto(produtoid);
                    Console.Write("Selecione uma quantidade: ");
                    int quantidade = int.Parse(Console.ReadLine());
                    if(!(quantidade > estoque || quantidade < estoque))
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
                            cmd.Parameters.AddWithValue("@Pedidoid", produtoid);
                            cmd.Parameters.AddWithValue("@Quantidade", quantidade);
                            cmd.Parameters.AddWithValue("@Precototal", precototal);
                            cmd.ExecuteNonQuery();
                        }
                        Console.WriteLine("Produto cadastrado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Quantidade invalida!");
                    }
                }
                else
                {
                    Console.WriteLine("Produto inexistente!");
                }
                
            }
            else
            {
                Console.WriteLine("Pedido inexistente!");
            }
        }

        public static void ListarItensPedidos()
        {
            ListarPedidos();
            Console.WriteLine("Digite o id do pedido: ");
            int idpedido = Int32.Parse(Console.ReadLine());
            int pedidos = 0;
            PedidosCadastrados(pedidos);
            if (!(idpedido < 0 || idpedido > pedidos))
            {
                using (var conexao = new SqliteConnection("Data Source=database.db"))
                {
                    conexao.Open();
                    var cmd = conexao.CreateCommand();
                    cmd.CommandText = "SELECT Id, Pedidoid, Produtoid, Quantidade, Precototal, Produto.Nome FROM Itenspedido INNER JOIN Produtos ON Produtos.id = Itenspedido.Produtoid WHERE Itenspedido.Pedidoid = @Idpedido;";
                    cmd.Parameters.AddWithValue("@Idpedido", idpedido);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Lista de Produtos do Pedido");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["Id"]}, ID_PEDIDO: {reader["Pedidoid"]}, PRODUTO: {reader["Produto.Nome"]}, QUANTIDADE: {reader["Quantidade"]}, PRECO_TOTAL: {reader["Precototal"]}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Pedido inexistente!");
            }

        }

    }
}
