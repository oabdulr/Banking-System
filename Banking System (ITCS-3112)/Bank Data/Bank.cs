using Banking_System__ITCS_3112_.Bank_Data.Accounts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking_System__ITCS_3112_.Banks
{

    public enum account_type
    {
        customer,
        employee,
        manager
    };

    public class Bank
    {
        public static int SLEEP_TIME = 3000; // ms
        public static Random RAND = new Random();

        public Bank(string name)
        {
            this.name = name;
        }

        public void display_data()
        {
            Console.WriteLine($"Registered Accounts: {accounts.Count}\nDate: {System.DateTime.Now}\n\n");
        }

        public Account prompt_login()
        {
            Console.Clear();
            Console.WriteLine("Login Page\n");

            Console.WriteLine("(Leave blank if you do not know)");

            while (true)
            {
                try
                {
                    Console.Write("Enter Account Number: ");
                    string in_acc = Console.ReadLine();
                    if (in_acc == "")
                        break;

                    Account acc = query_lookup(Convert.ToInt32(in_acc));
                    if (acc == null)
                        break;
                    else
                        return acc;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nInvalid Account, Number Only\n");
                }
            }


            Console.Write("Enter First Name: ");
            string in_first = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            string in_last = Console.ReadLine();
            int converted_pin = -1;

            while (true)
            {
                try
                {
                    Console.Write("Enter pin: ");
                    string in_pin = Console.ReadLine();
                    converted_pin = Convert.ToInt32(in_pin);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nInvalid Pin, Number Only\n");
                }
            }

            Account account = slow_query_account(in_first, in_last, converted_pin); ;

            if (account is null)
                Console.WriteLine("\nLogin Failed.");
            else
                Console.WriteLine("\nLogged In.");

            Thread.Sleep(SLEEP_TIME);
            return account;
        }

        public void prompt_create_user()
        {
            Console.Clear();
            Console.WriteLine("Account Creation\n");

            Console.Write("Enter First Name (max 24 char): ");
            string in_first = Console.ReadLine();
            Console.Write("Enter Last Name (max 24 char): ");
            string in_last = Console.ReadLine();
            int converted_pin = -1;
            while (true)
            {
                try
                {
                    Console.Write("Enter pin (4 or 5 digets): ");
                    string in_pin = Console.ReadLine();
                    converted_pin = Convert.ToInt32(in_pin);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nInvalid Pin, Number Only\n");
                }
            }

            string return_data = create_user(in_first, in_last, converted_pin);
            switch (return_data)
            {
                case "pin":
                    Console.WriteLine("\nInvalid Pin, stay within xxxx or xxxxx digets.\n");
                    Thread.Sleep(SLEEP_TIME);
                    this.prompt_create_user();
                    break;
                case "invalid_first":
                    Console.WriteLine("\nInvalid First Name, max 24 characters.\n");
                    Thread.Sleep(SLEEP_TIME);
                    this.prompt_create_user();
                    break;
                case "invalid_last":
                    Console.WriteLine("\nInvalid Last Name, max 24 characters.\n");
                    Thread.Sleep(SLEEP_TIME);
                    this.prompt_create_user();
                    break;
                case "repeat":
                    Console.WriteLine("\nAccount Already Exists.\n");
                    Thread.Sleep(SLEEP_TIME);
                    this.prompt_create_user();
                    break;
                default:
                    Console.WriteLine($"\nAccount Created.\nYour account number is {return_data}\nThis is needed for login\n\nPress Any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }

        private string create_user(string first, string last, int pin)
        {
            if (first.Length > 24 || first.Length == 0)
                return "invalid_first";
            if (last.Length > 24 || last.Length == 0)
                return "invalid_last";
            if (pin <= 999 || pin > 99999)
                return "pin";

            if (slow_query_account(first, last, pin) != null)
                return "repeat";

            int account_number = RAND.Next(100000, 999999);
            accounts.Add(account_number, new Account(account_number, first, last, pin));
            return account_number.ToString();
        }

        // public quick
        public Account query_lookup(int account_number) => accounts[account_number];
        // private internal only
        internal Account slow_query_account(string first, string last, int pin)
        {
            // Need a better way
            foreach (KeyValuePair<int, Account> account in accounts)
                if (first == account.Value.get_first() && account.Value.compare_pin(pin) && (last == "" ? true : last == account.Value.get_last()))
                    return account.Value;
            
            return null;
        }

        public bool has_executed(int id) => executed_transactions[id] == null ? false : true;

        public Transaction do_transfer(int from_account_number, int to_account_number, float amount)
        {
            Transaction transaction = new Transaction(from_account_number, to_account_number, amount);
            transaction.number = RAND.Next(10000, 99999);
            transaction.execute(this);
            return transaction;
        }

        // name unchangeable
        public string name { get; }

        private Dictionary<int, Account> accounts = new Dictionary<int, Account>()
        {
            {1, new Customer(1, "John", "Doe", 8821) },
            {2, new Customer(1, "Jane", "Doe", 1223) },
            {3, new Customer(1, "Jonny", "Cam", 1214) }
        };

        private Dictionary<int, Transaction> executed_transactions = new Dictionary<int, Transaction>();
    }
}
