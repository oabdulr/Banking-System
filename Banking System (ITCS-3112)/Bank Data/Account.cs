using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking_System__ITCS_3112_.Banks
{
    public class Account
    {
        public static int SLEEP_TIME = 1000; // ms

        public Account(int account_number, string first, string last, int pin, account_type type = account_type.customer)
        {
            this.account_number = account_number; ;
            this.first = first;
            this.last = last;
            this.pin = pin;

            this.permissions = type;
        }

        public string get_first() { return this.first; }
        public string get_last() { return this.last; }
        public string full_name() { return $"{this.get_first()} {this.get_last()}"; }
        public int get_account_number() { return this.account_number; }
        public float get_balance() { return this.balance; }

        public bool do_transaction(Transaction transaction)
        {
            if (transaction is null) return false;
            if (transaction.from_account != account_number) return false;
            if (transaction.amt > this.balance) return false;

            this.balance -= transaction.amt;
            Console.WriteLine($"do - {this.balance} {this.pin}");

            return true;
        }

        public bool get_transaction(Transaction transaction)
        {
            if (transaction is null) return false;
            Console.WriteLine($"{transaction.to_account}, {this.account_number}");
            if (transaction.to_account != this.account_number) return false;

            this.balance += transaction.amt;
            Console.WriteLine($"get + {this.balance} {this.pin}");
            return true;
        }

        // Kinda useless lol
        public double get_pin_hash()
        {
            return Math.Pow((float)pin * salt, 8);
        }

        public double hash_pin(int pin)
        {
            return Math.Pow((float)pin * salt, 8);
        }

        // pin == this.pin with extra steps
        public bool compare_pin(int pin)
        {
            return hash_pin(pin) == get_pin_hash();
        }

        public virtual bool prompt_options(Bank bank) { Console.ReadKey(); return false; }
        public void prompt_wire_transfer(Bank bank) 
        {
            Console.Clear();
            Console.WriteLine("Wire Transfer Menu\n");

            Console.Write("Enter Transfer Account: ");
            string in_account = Console.ReadLine();
            int converted_acc = -1;
            while (true)
            {
                try
                {
                    converted_acc = Convert.ToInt32(in_account);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nInvalid Account, Numbers Only\n");
                }
            }

            Account located_account = bank.query_lookup(converted_acc);
            if (located_account is null)
            {
                Console.WriteLine("\nInvalid Account\n");
                Thread.Sleep(SLEEP_TIME);
                return;
            }

            Console.Write("Enter Amount $");
            string in_amt = Console.ReadLine();
            float converted_amt = -1;
            while (true)
            {
                try
                {
                    converted_amt = float.Parse(in_amt, NumberStyles.Currency); // Thanks stack overflow
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nInvalid Amount, Numbers Only\n");
                }
            }

            if (converted_amt > this.balance)
            {
                Console.WriteLine("\nInsufficent Funds.\n");
                Thread.Sleep(SLEEP_TIME);
                return;
            }


            Transaction transaction = this.wire_transfer(converted_acc, converted_amt, bank);
            if (!transaction.passed)
            {
                Console.WriteLine("\nTransaction Failed.\n");
                Thread.Sleep(SLEEP_TIME);
            }else
            {
                Console.WriteLine("\nTransaction Succeeded.\n");
                Thread.Sleep(SLEEP_TIME);
            }
        }

        public Transaction wire_transfer(int to_account_number, float amount, Bank bank) => bank.do_transfer(this.account_number, to_account_number, amount);
        public account_type permissions { get; private set; }

        private string first;
        private string last;
        private int pin;
        private float balance = 100000f;

        private float salt = -214.22f;

        private int account_number;

        private List<Transaction> transactions = new List<Transaction>();
    }
}
