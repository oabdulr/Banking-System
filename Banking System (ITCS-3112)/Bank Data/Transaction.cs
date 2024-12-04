using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking_System__ITCS_3112_.Banks
{
    public enum transaction_type
    {
        purchase,
        wire_transfer,
        other
    }

    public class Transaction
    {
        public Transaction() { }
        public Transaction(int from_account, int to_account, int number, float amt, float rate = 1) 
        {
            this.from_account = from_account;
            this.to_account = to_account;
            this.number = number;
            this.amt = amt;
            this.rate = rate;
        }

        public bool execute(Bank bank)
        {
            if (this.amt <= 0 && rate > 0) return false;
            if (this.to_account == this.from_account) return false;
            if (bank.has_executed(this.number)) return false;

            Account from = bank.query_lookup(this.from_account);
            if (from is null) return false;
            if (from.get_balance() <= 0) return false;
            if (from.get_balance() < amt) return false;

            Account to = bank.query_lookup(this.to_account);
            if (to is null) return false;


            if (rate > 0)
            {
                if (!from.do_transaction(this)) return false;
                if (!to.get_transaction(this)) return false;
            }
            else
            {
                // skuffed stocks
                int bk = this.from_account;

                this.from_account = this.to_account;
                this.to_account = bk;

                if (!from.get_transaction(this)) return false;
                if (!to.do_transaction(this)) return false;

                this.to_account = this.from_account;
                this.from_account = bk;
            }

            bank.add_executed(this);
            this.passed = true;
            this.executed_date = DateTime.Now;
            return this.passed;
        }

        public int from_account;
        public int to_account;
        public float amt;
        public float rate = 1; // 1 usd

        public int number;

        public bool passed = false;
        public transaction_type type = transaction_type.other;
        public DateTime created_date = DateTime.Now;
        public DateTime executed_date = DateTime.MinValue;
    }
}
