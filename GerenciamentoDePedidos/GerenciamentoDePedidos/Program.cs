using System;
using System.Threading;

namespace GerenciamentoDePedidos
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=====================================");
                Console.WriteLine("    *** Menu De Gerenciamento ***       ");
                Console.WriteLine("=====================================");
                Console.WriteLine();
                Console.ResetColor();
                Console.WriteLine(" 1 - Cadastrar Cliente");
                Console.WriteLine(" 2 - Cadastrar Produto");
                Console.WriteLine(" 3 - Criar Pedido");
                Console.WriteLine(" 4 - Listar Pedidos");
                Console.WriteLine(" 5 - Deletar Cliente");
                Console.WriteLine(" 6 - Deletar Produto");
                Console.WriteLine(" 7 - Sair");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Escolha uma opção (1-7): ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=====================================");
                Console.ResetColor();

                //Resumindo para vcs, o ForegroundColor vai mudar a cor dos writeline, e o resetColor é obvio né, volta ao normal

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                string opc = Console.ReadLine();

                switch (opc)
                {
                    case "7":
                        Console.WriteLine("\nEncerrando o programa...");
                        Thread.Sleep(2500);
                        return;
                        break;

                    case "1":

                        Thread.Sleep(2500);
                        return;
                        break;

                    case "2":

                        Thread.Sleep(2500);
                        return;
                        break;

                    case "3":

                        Thread.Sleep(2500);
                        return;
                        break;

                    case "4":

                        Thread.Sleep(2500);
                        return;
                        break;

                    case "5":

                        Thread.Sleep(2500);
                        return;
                        break;

                    case "6":

                        Thread.Sleep(2500);
                        return;
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
