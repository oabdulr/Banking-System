using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Banking_System__ITCS_3112_.Banks
{
    public class Transaction
    {
        public Transaction() { }
        public Transaction(int from_account, int to_account, float amt) 
        {
            this.from_account = from_account;
            this.to_account = to_account;
            this.amt = amt;
        }

        public bool execute(Bank bank)
        {
            if (bank.has_executed(this.number)) return false;

            Account from = bank.query_lookup(this.from_account);
            if (from is null) return false;
            if (from.get_balance() <= 0) return false;
            if (from.get_balance() < amt) return false;

            Account to = bank.query_lookup(this.to_account);
            if (to is null) return false;

            from.do_transaction(this);
            to.get_transaction(this);

            return true;
        }

        public int from_account;
        public int to_account;
        public float amt;

        public int number;
    }
}
