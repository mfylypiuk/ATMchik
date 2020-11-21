using System;
using System.Collections.Generic;
using System.Text;

namespace ATMchik.Models
{
    public class Card
    {
        public ulong CardNumber { get; set; }
        public ulong BankAccountNumber { get; set; }
        public string CvvHash { get; set; }
        public string Owner { get; set; }
    }
}
