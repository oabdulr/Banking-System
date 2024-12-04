using System;
using System.Threading;
using Banking_System__ITCS_3112_.Bank_Data.Investments;
using Banking_System__ITCS_3112_.Banks;

namespace Banking_System__ITCS_3112_
{
    public class Program
    {

        // as much as I hate threading like this, I designed this app without the idea of ever running multiple things at once
        // so I have to make the theads like this or I redesign it all :(
        static void tInvestments(Bank b)
        {
            new Thread(() => {

                while (true)
                {
                    lock (b.tLock)
                    {
                        foreach (Company c in b.get_companies())
                        {
                            c.last_value = c.value;
                            c.value += c.value * (c.tick(Bank.RAND) / 100f);
                        }
                    }

                    Thread.Sleep(2000);
                }
            
            }).Start();
        }


        static void Main(string[] args)
        {
            Bank central_bank = new Bank("Central Bank");
            Account logged_in_account = null;

            tInvestments(central_bank);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Banking System (ITCS-3112)\n");
                Console.WriteLine("System of " + central_bank.name);
                central_bank.display_data();

                if (logged_in_account == null)
                {
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Create Account");
                    char key = Console.ReadKey().KeyChar;
                    switch (key)
                    {
                        case '1':
                            logged_in_account = central_bank.prompt_login();
                            break;
                        case '2':
                            central_bank.prompt_create_user();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Logged In as {logged_in_account.full_name()}");
                    Console.WriteLine($"Account Number: {logged_in_account.get_account_number()}\n");
                    Console.WriteLine($"Current Balance: {logged_in_account.get_balance():C}\n");

                    if (logged_in_account.prompt_options(central_bank))
                        logged_in_account = null;
                }
            }
        }
    }
}
