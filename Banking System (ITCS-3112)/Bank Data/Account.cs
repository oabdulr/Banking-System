using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking_System__ITCS_3112_.Banks
{
    public class Account
    {
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

        public bool wire_transfer(int to_account_number, float amount)

        public account_type permissions { get; private set; }

        private string first;
        private string last;
        private int pin;
        private float balance = 0f;

        private float salt = -214.22f;

        private int account_number;

        private List<Transaction> transactions = new List<Transaction>();
    }
}
