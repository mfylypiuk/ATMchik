using System;
using System.Collections.Generic;
using System.Text;

namespace ATMchik.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public ulong BankAccountNumber { get; set; }
        public ulong CardNumber { get; set; }
        public string PvvHash { get; set; }
        public string CvvHash { get; set; }
        public decimal Value { get; set; }
        public string Owner { get; set; }
    }
}
