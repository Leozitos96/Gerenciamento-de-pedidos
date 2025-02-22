using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                cmd.CommandText = "INSERT INTO Clientes (Nome, Email) VALUES (@Nome, @Email)";
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
                cmd.CommandText = "SELECT Id, Nome, Email FROM Clientes";
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
                cmd.CommandText = "UPDATE Clientes SET Email = @Email WHERE Id = @Id";
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
                cmd.CommandText = "DELETE FROM Clientes WHERE Id = @Id";
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
                cmd.CommandText = "INSERT INTO Produtos (Nome, Preco, Estoque) VALUES (@Nome, @Preco, @Estoque)";
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
                cmd.CommandText = "SELECT Id, Nome, Preco, Estoque FROM Produtos";
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
                cmd.CommandText = "UPDATE Produtos SET Preco = @Preco, Estoque = @Estoque WHERE Id = @Id";
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
                cmd.CommandText = "DELETE FROM Produtos WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Produto removido com sucesso!");
        }
    }
}
