using Banking_System__ITCS_3112_.Bank_Data.Accounts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

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
        public static int SLEEP_TIME = 1500; // ms
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

            Account acc_found = null;
            while (true)
            {
                try
                {
                    Console.Write("Enter Account Number: ");
                    string in_acc = Console.ReadLine();
                    if (in_acc == "")
                        break;

                    acc_found = query_lookup(Convert.ToInt32(in_acc));
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nInvalid Account, Numbers Only\n");
                }
            }

            if (acc_found != null)
            {
                DateTime acc_dob = this.prompt_dob();
                if (!acc_found.validate_dob(acc_dob))
                {
                    Console.WriteLine("\nLogin failed.");
                    Thread.Sleep(SLEEP_TIME);
                    return null;
                }

                if (acc_found.needs_reset)
                {
                    if (acc_found.prompt_pin_reset())
                        return acc_found;
                    else
                    {
                        Console.WriteLine("\nLogin failed.");
                        Thread.Sleep(SLEEP_TIME);
                        return null;
                    }
                }
                else
                    while (true)
                    {
                        try
                        {

                            Console.Write("Enter Pin Number: ");
                            string in_pin = Console.ReadLine();
                            if (in_pin == "")
                                break;

                            if (!acc_found.compare_pin(Convert.ToInt32(in_pin)))
                            {
                                Console.WriteLine("\nLogin failed.");
                                Thread.Sleep(SLEEP_TIME);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\nLogged in.");
                                Thread.Sleep(SLEEP_TIME);
                                return acc_found;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("\nInvalid Pin, Numbers Only\n");
                        }
                    }
            }


            Console.Write("\nEnter First Name: ");
            string in_first = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            string in_last = Console.ReadLine();
            DateTime dob = this.prompt_dob();
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
                    Console.WriteLine("\nInvalid Pin, Numbers Only\n");
                }
            }

            Account account = slow_query_account(in_first, in_last, dob, converted_pin); ;

            if (account is null)
                Console.WriteLine("\nLogin Failed.");
            else
                Console.WriteLine("\nLogged In.");

            Thread.Sleep(SLEEP_TIME);
            return account;
        }

        public DateTime prompt_dob()
        {
            DateTime time = DateTime.MinValue;

            // we accept all people, no age restriction, including yet to be born people!
            while (true)
            {
                Console.Write("Please enter you date of birth as mm/dd/yyyy\nDOB: ");
                string date_of_birth = Console.ReadLine();
                string[] dob_arr = date_of_birth.Split('/');
                try
                {
                    int month = Convert.ToInt32(dob_arr[0]);
                    int day = Convert.ToInt32(dob_arr[1]);
                    int year = Convert.ToInt32(dob_arr[2]);
                    time = new DateTime(year, month, day);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid entry\n");
                }
            }

            return time;
        }

        public void prompt_create_user()
        {
            Console.Clear();
            Console.WriteLine("Account Creation\n");

            Console.Write("Enter First Name (max 24 char): ");
            string in_first = Console.ReadLine();
            Console.Write("Enter Last Name (max 24 char): ");
            string in_last = Console.ReadLine();
            DateTime dob = this.prompt_dob();
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
                    Console.WriteLine("\nInvalid Pin, Numbers Only\n");
                }
            }

            string return_data = create_user(in_first, in_last, dob, converted_pin);
            switch (return_data)
            {
                case "invalid_pin":
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
                case "invalid_dob":
                    Console.WriteLine("\nInvalid Date of Birth Name, mm/dd/yy\n");
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

        public void prompt_employee_reset_pin()
        {
            Console.Clear();
            Console.WriteLine("Account Recovery\n");

            Console.Write("Enter First Name (max 24 char): ");
            string in_first = Console.ReadLine();
            Console.Write("Enter Last Name (max 24 char): ");
            string in_last = Console.ReadLine();

            DateTime dob = this.prompt_dob();

            Account located_account = slow_query_account(in_first, in_last, dob);
            if (located_account is null)
            {
                Console.WriteLine("\nInvalid information\nPlease try again later.");
                Thread.Sleep(SLEEP_TIME);
                return;
            }

            Console.WriteLine("\nCheck customers ID to verify pin reset\n1. Continue\n2. Exit");
            if (Console.ReadKey().KeyChar != '1')
            {
                Console.WriteLine("\nPin Reset Failed.\nPlease try again later.");
                Thread.Sleep(SLEEP_TIME);
                return;
            }

            located_account.needs_reset = true;
            Console.WriteLine("\nCustomer will be prompted for a new pin on next login attempt.\nPin Reset Successful.");
            Thread.Sleep(SLEEP_TIME * 2);
        }

        public Account prompt_manager_view()
        {
            Console.Clear();
            Console.WriteLine("Profile Manager\n");

            Console.Write("Enter First Name (max 24 char): ");
            string in_first = Console.ReadLine();
            Console.Write("Enter Last Name (max 24 char): ");
            string in_last = Console.ReadLine();

            DateTime dob = this.prompt_dob();

            Account located_account = slow_query_account(in_first, in_last, dob);
            if (located_account is null)
            {
                Console.WriteLine("\nInvalid information\nPlease try again later.");
                Thread.Sleep(SLEEP_TIME);
            }
            else
            {
                Console.WriteLine("\nLoaded Profile.");
                Thread.Sleep(SLEEP_TIME);
            }

            return located_account;
        }

        private string create_user(string first, string last, DateTime dob, int pin)
        {
            if (first.Length > 24 || first.Length == 0)
                return "invalid_first";
            if (last.Length > 24 || last.Length == 0)
                return "invalid_last";
            if (dob == DateTime.MinValue)
                return "invalid_dob";
            if (pin <= 999 || pin > 99999)
                return "invalid_pin";

            if (slow_query_account(first, last, dob, pin) != null)
                return "repeat";

            int account_number = RAND.Next(100000, 999999);
            accounts.Add(account_number, new Account(account_number, first, last, dob, pin));
            return account_number.ToString();
        }

        // public quick
        public Account query_lookup(int account_number)
        {
            if (accounts.TryGetValue(account_number, out Account ret))
                return ret;
            return null;
        }
        // private internal only
        internal Account slow_query_account(string first, string last, DateTime dob, int pin = 0)
        {
            // Need a better way
            foreach (KeyValuePair<int, Account> account in accounts)
                if (first == account.Value.get_first() && account.Value.validate_dob(dob) && (pin != 0 ? account.Value.compare_pin(pin) : true) && (last == "" ? true : last == account.Value.get_last()))
                    return account.Value;

            return null;
        }

        public bool has_executed(int id) => executed_transactions.ContainsKey(id);
        public void add_executed(Transaction transaction) => executed_transactions[transaction.number] = transaction;

        public Transaction do_transfer(int from_account_number, int to_account_number, float amount)
        {
            Transaction transaction = new Transaction();
            transaction.from_account = from_account_number;
            transaction.to_account = to_account_number;
            transaction.amt = amount;
            transaction.number = RAND.Next(10000, 99999);
            transaction.type = transaction_type.wire_transfer;
            transaction.execute(this);
            return transaction;
        }

        // name unchangeable
        public string name { get; }

        private Dictionary<int, Account> accounts = new Dictionary<int, Account>()
        {
            {1, new Customer(1, "John", "Doe", new DateTime(2024, 8, 25), 8821) },
            {2, new Customer(2, "Jane", "Doe", new DateTime(2024, 8, 24), 1223) },
            {3, new Manager(3, "Jonny", "Cam", new DateTime(2024, 8, 23), 1214) }
        };

        private Dictionary<int, Transaction> executed_transactions = new Dictionary<int, Transaction>();
    }
}
