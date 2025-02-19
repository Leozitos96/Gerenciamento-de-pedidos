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
            CriarTabelas();
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


    }
}
