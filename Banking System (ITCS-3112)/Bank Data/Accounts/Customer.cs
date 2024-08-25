using Banking_System__ITCS_3112_.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking_System__ITCS_3112_.Bank_Data.Accounts
{
    public class Customer : Account
    {
        public Customer(int account_number, string first, string last, DateTime dob, int pin, account_type type = account_type.customer) : base(account_number, first, last, dob, pin, type)
        {

        }

        public override bool prompt_options(Bank bank)
        {
            Console.WriteLine("1. Activity");
            Console.WriteLine("2. Wire Transfer");
            Console.WriteLine("3. Reset Pin");
            Console.WriteLine("4. Logout");

            char input = Console.ReadKey().KeyChar;

            switch (input)
            {
                case '1':
                    this.prompt_actvity(bank);
                    break;
                case '2':
                    this.prompt_wire_transfer(bank);
                    break;
                case '3':
                    Console.Clear();
                    Console.Write("Enter Current Pin: ");
                    string in_pin = Console.ReadLine();
                    try
                    {
                        if (this.compare_pin(Int32.Parse(in_pin)))
                            this.prompt_pin_reset();
                        else
                        {
                            Console.WriteLine("Invalid Pin.");
                            Thread.Sleep(SLEEP_TIME);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Pin.");
                        Thread.Sleep(SLEEP_TIME);
                    }
                    break;
                case '4':
                    return true;
            }

            return false;
        }
    }
}
