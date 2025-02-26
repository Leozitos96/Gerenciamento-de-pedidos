using System;
using System.Threading;

namespace GerenciamentoDePedidos
{
    class Program
    {

        private static Database database = new Database();
        static void Main(string[] args)
        {
            database.CriarTabelas();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=====================================");
                Console.WriteLine("    *** Menu De Gerenciamento ***       ");
                Console.WriteLine("=====================================");
                Console.WriteLine();
                Console.ResetColor();
                Console.WriteLine(" 1 - Cadastrar Cliente");
                Console.WriteLine(" 2 - Cadastrar Produto");
                Console.WriteLine(" 3 - Atualizar Email do Cliente");
                Console.WriteLine(" 4 - Atualizar Produto");
                Console.WriteLine(" 5 - Criar Pedido");
                Console.WriteLine(" 6 - Adicionar produto em um Pedido");
                Console.WriteLine(" 7 - Listar Itens dos pedidos");
                Console.WriteLine(" 8 - Listar Pedidos");
                Console.WriteLine(" 9 - Listar Clientes");
                Console.WriteLine(" 10 - Listar Produtos");
                Console.WriteLine(" 11 - Deletar Cliente");
                Console.WriteLine(" 12 - Deletar Produto");
                Console.WriteLine(" 0 - Sair");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Escolha uma opção (0-12): ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=====================================");
                Console.ResetColor();

                //Resumindo para vcs, o ForegroundColor vai mudar a cor dos writeline, e o resetColor é obvio né, volta ao normal

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                string opc = Console.ReadLine();

                switch (opc)
                {
                    case "0":
                        Console.WriteLine("\nEncerrando o programa...");
                        Thread.Sleep(2500);
                        return;
                        break;

                    case "1":

                        database.CadastrarCliente();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "2":

                        database.CadastrarProduto();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "3":

                        database.AtualizarEmailCliente();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "4":

                        database.AtualizarProduto();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "5":
                        database.NovoPedido();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "6":
                        database.AdicionarProdutoPedido();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "7":
                        database.ListarItensPedidos();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "8":
                        database.ListarPedidos();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "9":
                        database.ListarClientes();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "10":
                        database.ListarProdutos();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "11":

                        database.RemoverCliente();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;

                    case "12":

                        database.RemoverProduto();
                        Thread.Sleep(2500);
                        Console.Clear();
                        break;


                    default:
                        Console.WriteLine("Opção invalida!");
                        Thread.Sleep(1500);
                        break;
                }
            }
        }
    }

}
