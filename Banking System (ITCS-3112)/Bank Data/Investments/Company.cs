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
        public Company(string name)
        {
            this.name = name;
            this.value = 100;
        }


        public string name { get; }
        public float value { get; set; } // usd
        public float last_value { get; set; }


        public float tick(Random r) => r.Next(-10, 11); // biased :)
    }
}
