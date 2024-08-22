using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banking_System__ITCS_3112_.Banks;

namespace Banking_System__ITCS_3112_
{
    public class Program
    {
        static void Main(string[] args)
        {
            Bank central_bank = new Bank("Central Bank");
            Account logged_in_account = null;

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
