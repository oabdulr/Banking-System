using Banking_System__ITCS_3112_.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking_System__ITCS_3112_.Bank_Data.Investments
{
    public class Company
    {
        // basic investment stuff
        // not going to add any complex stuff, just a random stock price
        public Company(string name, Account account)
        {
            this.name = name;
            this.value = 100;
            this.account = account;
        }

        public List<Transaction> get_vests(Account a)
        => investments.TryGetValue(a, out var trans) ? trans : null;

        public List<Transaction> get_vests(Bank b, int account)
        => investments.TryGetValue(b.query_lookup(account), out var trans) ? trans : null;

        public float get_totalvest(Bank b, int account)
        {
            List<Transaction> vests = get_vests(b, account);
            if (vests == null)
                return 0;
            float total = 0;
            foreach (Transaction t in vests)
                total += t.amt;
            return total;
        }


        public float get_totalvest(Account a)
        {
            List<Transaction> vests = get_vests(a);
            if (vests == null)
                return 0;
            float total = 0;
            foreach (Transaction t in vests)
                total += t.amt;
            return total;
        }

        public void invest(Bank b, Transaction t)
        {
            if (!t.passed)
                return;

            if (investments.TryGetValue(b.query_lookup(t.from_account), out var _))
                investments[b.query_lookup(t.from_account)].Add(t);
            else
                investments.Add(b.query_lookup(t.from_account), new List<Transaction>() { t });
        }


        private Dictionary<Account, List<Transaction>> investments = new Dictionary<Account, List<Transaction>>();

        public string name { get; }
        public float value { get; set; } // usd
        public float last_value { get; set; }
        public Account account { get; }


        public float tick(Random r) => r.Next(-10, 11); // biased :)
    }
}
