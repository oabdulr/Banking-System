using Banking_System__ITCS_3112_.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking_System__ITCS_3112_.Bank_Data.Accounts
{

    public class Manager : Account
    {
        private Account opened_profile = null;

        public Manager(int account_number, string first, string last, DateTime dob, int pin, account_type type = account_type.customer) : base(account_number, first, last, dob, pin, type)
        {
        }

        // I cannot be asked to make a dynamic menu so I dont have to repeat options for each class 
        public override bool prompt_options(Bank bank)
        {

            if (opened_profile != null)
            {
                Console.WriteLine($"Viewing {opened_profile.full_name()}'s profile");
                Console.WriteLine($"{opened_profile.full_name()}'s Balance: {opened_profile.get_balance():C}\n");
                Console.WriteLine($"1. {opened_profile.full_name()} -> Activity");
                Console.WriteLine($"2. {opened_profile.full_name()} -> Wire Transfer");
                Console.WriteLine($"3. {opened_profile.full_name()} -> Reset Pin");
                Console.WriteLine("4. Close Profile");

                char profile_input = Console.ReadKey().KeyChar;

                switch (profile_input)
                {
                    case '1':
                        opened_profile.prompt_actvity(bank);
                        break;
                    case '2':
                        opened_profile.prompt_wire_transfer(bank);
                        break;
                    case '3':
                        opened_profile.prompt_pin_reset(); 
                        break;
                    case '4':
                        opened_profile = null;
                        break;

                }

                return false;
            }

            Console.WriteLine("1. Activity");
            Console.WriteLine("2. Wire Transfer");
            Console.WriteLine("3. Open Customer Profile");
            Console.WriteLine("4. Open New Account");
            Console.WriteLine("5. Reset Pin");
            Console.WriteLine("6. Logout");

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
                    opened_profile = bank.prompt_manager_view();
                    break;
                case '4':
                    bank.prompt_create_user();
                    break;
                case '5':
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
                case '6':
                    return true;
            }

            return false;
        }
    }
}
