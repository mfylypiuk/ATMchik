using System;
using System.IO;
using ATMchik.Handlers;
using ATMchik.Models;
using Newtonsoft.Json;

namespace ATMchik_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            while (true)
            {
                Console.WriteLine("[INFO] Enter your card...");

                var inputLine = Console.ReadLine();
                //var inputLine = "9291315421382894";

                if (inputLine == "system")
                {
                    Console.Write("Enter new client name: ");
                    string newClientName = Console.ReadLine();
                    BankAccountHandler.CreateBankAccount(newClientName);
                }
                else
                {
                    Session session = new Session();

                    try
                    {
                        var cardInfo = File.ReadAllText("cards\\" + inputLine + ".crd");
                        session.Card = JsonConvert.DeserializeObject<Card>(cardInfo);
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ERROR] Card is incorrect");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }

                    Console.WriteLine("[INFO] Enter PIN code");

                    string enteredPin = Console.ReadLine();
                    //string enteredPin = "6633";

                    if (uint.TryParse(enteredPin, out uint pin))
                    {
                        session.Pin = pin;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[INFO] PIN code is incorrect");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    try
                    {
                        if (session.Authenticate())
                        {
                            while (true)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("Enter operation type (get/put/check/exit): ");
                                Console.ForegroundColor = ConsoleColor.White;

                                string operationType = Console.ReadLine();

                                if (operationType == "get")
                                {
                                    Console.Write("Enter value to get money: ");

                                    if (!decimal.TryParse(Console.ReadLine(), out decimal value))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Value is incorrect");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (session.GetMoney(value))
                                            {
                                                Console.ForegroundColor = ConsoleColor.Cyan;
                                                Console.WriteLine("[!] -" + value + "$");
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("[ERROR] " + ex.Message);
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                    }
                                }
                                else if (operationType == "put")
                                {
                                    Console.Write("Enter value to put money: ");

                                    if (!decimal.TryParse(Console.ReadLine(), out decimal value))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Value is incorrect");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (session.PutMoney(value))
                                            {
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine("[!] +" + value + "$");
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("[ERROR] " + ex.Message);
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                    }
                                }
                                else if (operationType == "check")
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("[!] " + session.CheckValue() + "$");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                else if (operationType == "exit")
                                {
                                    break;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("[ERROR] Operation type is incorrect");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
    }
}
