using System;
using System.Collections.Generic;
using System.Threading;

namespace Task_1
{
    class Client
    {
        public int Id;
        public int Age;
        public decimal Balance;
        public string FirstName;
    }

    class Program
    {
        static int clientId = 0;
        static List<Client> clients = new List<Client>();

        static int updatedClientId = 0;
        static decimal lastBalance = 0;
        static decimal newBalance = 0;
        static bool isUpdated = false;
        static int InputId()
        {
            Console.Write("Input ID: ");
            int Id = int.Parse(Console.ReadLine());
            return Id;
        }
        static Client FindById(int Id)
        {
            Client resClient = null;
            if (clients.Count > 0)
            {
                foreach (Client client in clients)
                {
                    if (client.Id == Id)
                    {
                        resClient = client;
                        break;
                    }
                }
            }
            return resClient;
        }
        static void Insert()
        {
            Client client = new Client();
            Console.Write("Имя: ");
            string firstName = Console.ReadLine();
            Console.Write("Возраст: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Баланс: ");
            decimal balance = decimal.Parse(Console.ReadLine().Replace('.', ','));
            client.Id = clientId;
            client.FirstName = firstName;
            client.Age = age;
            client.Balance = balance;
            clients.Add(client);
            clientId++;
        }
        static void Update(object Id)
        {
            try
            {
                Console.Write("Баланс: ");
                decimal balance = decimal.Parse(Console.ReadLine().Replace('.', ','));

                Client client = FindById((int)Id);
                Client updatedClient = new Client();

                isUpdated = true;
                updatedClientId = (int)Id;
                lastBalance = client.Balance;
                newBalance = balance;

                updatedClient.Id = client.Id;
                updatedClient.FirstName = client.FirstName;
                updatedClient.Age = client.Age;
                updatedClient.Balance = balance;
                clients.RemoveAt((int)Id);
                clients.Add(updatedClient);
            }
            catch (Exception) { Console.WriteLine("ERROR, TRY AGAIN, TRY TO INPUT CORRECT CLIENT ID"); return; }
        }
        static void Delete(object Id)
        {
            try
            {
                clients.RemoveAt((int)Id);
            }
            catch (Exception) { Console.WriteLine("ERROR, TRY AGAIN, TRY TO INPUT CORRECT CLIENT ID"); return; }
        }
        static void Select(object Id)
        {
            try
            {
                if ((int)Id == -1)
                {
                    foreach (Client item in clients)
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine($"ID: {item.Id}");
                        Console.WriteLine($"Name: {item.FirstName}");
                        Console.WriteLine($"Age: {item.Age}");
                        Console.WriteLine($"Balance: {item.Balance}");
                        Console.WriteLine("\n");
                    }
                }
                else
                {
                    Client selectedClient = FindById((int)Id);
                    Console.WriteLine("\n");
                    Console.WriteLine($"ID: {selectedClient.Id}");
                    Console.WriteLine($"Name: {selectedClient.FirstName}");
                    Console.WriteLine($"Age: {selectedClient.Age}");
                    Console.WriteLine($"Balance: {selectedClient.Balance}");
                    Console.WriteLine("\n");
                }
            }
            catch (Exception) { Console.WriteLine("ERROR, TRY AGAIN, TRY TO INPUT CORRECT CLIENT ID"); return; }
        }
        static void UI()
        {
            string menu = "\n1.\tInsert Client\n2.\tUpdate Client\n3.\tDelete Client\n4.\tSelect Client\n0.\tExit\n";
            Console.WriteLine(menu);
            string cmd = string.Empty;
            while (cmd != "0")
            {
                cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "1":
                        Thread threadInsert = new Thread(new ThreadStart(Insert));
                        threadInsert.Start();
                        threadInsert.Join();
                        break;
                    case "2":
                        {
                            int Id = InputId();
                            Thread threadUpdate = new Thread(new ParameterizedThreadStart(Update));
                            threadUpdate.Start(Id);
                            threadUpdate.Join();
                        }
                        break;
                    case "3":
                        {
                            int Id = InputId();
                            Thread threadDelete = new Thread(new ParameterizedThreadStart(Delete));
                            threadDelete.Start(Id);
                            threadDelete.Join();
                        }
                        break;
                    case "4":
                        {
                            Console.WriteLine("Input -1 To Select All Clients");
                            int Id = InputId();
                            Thread threadSelect = new Thread(new ParameterizedThreadStart(Select));
                            threadSelect.Start(Id);
                            threadSelect.Join();
                        }
                        break;
                }
                Console.WriteLine(menu);
            }
        }

        static void TimerEvent(object x)
        {
            if (isUpdated)
            {
                decimal difference = lastBalance - newBalance;
                if (difference != 0)
                {
                    if (lastBalance > newBalance)
                    {
                        char differenceChr = '-';
                        if (difference > 0)
                            differenceChr = '-';
                        else if (difference < 0)
                        {
                            difference *= (-1);
                            differenceChr = '+';
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Updated ID: {updatedClientId}, Last Balance {lastBalance}, New Balance {newBalance}, Difference {differenceChr}{difference}");
                        Console.ResetColor();
                    }
                    else if (lastBalance < newBalance)
                    {
                        char differenceChr = '-';
                        if (difference > 0)
                            differenceChr = '-';
                        else if (difference < 0)
                        {
                            difference *= (-1);
                            differenceChr = '+';
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Updated ID: {updatedClientId}, Last Balance {lastBalance}, New Balance {newBalance}, Difference {differenceChr}{difference}");
                        Console.ResetColor();
                    }
                }
                isUpdated = false;
            }
        }
        static void TimerFunctionallity()
        {
            TimerCallback timerCallback = new TimerCallback(TimerEvent);
            Timer t = new Timer(TimerEvent, 0, 0, 1000);
        }
        static void Main(string[] args)
        {
            TimerFunctionallity();
            UI();
        }
    }
}