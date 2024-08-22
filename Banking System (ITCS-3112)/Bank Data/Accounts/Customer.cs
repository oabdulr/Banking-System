using Banking_System__ITCS_3112_.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Banking_System__ITCS_3112_.Bank_Data.Accounts
{
    public class Customer : Account
    {
        public Customer(int account_number, string first, string last, int pin, account_type type = account_type.customer) : base(account_number, first, last, pin, type)
        {

        }

        public override bool prompt_options(Bank bank)
        {
            Console.WriteLine("1. Activity");
            Console.WriteLine("2. Wire Transfer");
            Console.WriteLine("3. Logout");

            char input = Console.ReadKey().KeyChar;

            switch (input)
            {
                case '1':
                    break;
                case '2':

                    break;
                case '3':
                    return true;
            }

            return false;
        }
    }
}
