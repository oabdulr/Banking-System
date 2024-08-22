using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking_System__ITCS_3112_.Banks
{
    public class Transaction
    {
        public Transaction() { }
        public Transaction(int from_account, int to_account, float amt, int number) 
        {
            this.from_account = from_account;
            this.to_account = to_account;
            this.amt = amt;
            this.number = number;
        }

        private int from_account;
        private int to_account;
        private float amt;

        public int number;
    }
}
