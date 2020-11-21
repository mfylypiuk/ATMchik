using ATMchik.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATMchik.Models
{
    public class Session : IDisposable
    {
        private ApplicationContext database;
        private BankAccount bankAccount;
        public Card Card { get; set; }
        public uint? Pin { get; set; }
        public bool Authenticated { get; private set; }

        public Session()
        {
            database = new ApplicationContext();
            Pin = null;
        }

        private bool IsCvvCorrect()
        {
            return database.BankAccounts.Any(bankAccount => bankAccount.CvvHash == Card.CvvHash);
        }

        private bool IsPinCorrect()
        {
            string pvvHash = VerificationValuesHandler.GetPvvHash(Pin.Value);
            return database.BankAccounts.Any(bankAccount => bankAccount.BankAccountNumber == Card.BankAccountNumber && bankAccount.PvvHash == pvvHash);
        }

        public bool Authenticate()
        {
            if (Card == null)
            {
                throw new ArgumentNullException("Card is undefined");
            }

            if (Pin == null)
            {
                throw new ArgumentNullException("PIN code is undefined");
            }

            if (!IsCvvCorrect())
            {
                throw new Exception("Card is incorrect");
            }

            if (!IsPinCorrect())
            {
                throw new Exception("PIN code is incorrect");
            }

            Authenticated = true;
            bankAccount = database.BankAccounts.SingleOrDefault(bankAccount => bankAccount.BankAccountNumber == Card.BankAccountNumber);
            
            return true;
        }

        public bool GetMoney(decimal value)
        {
            if (!Authenticated)
            {
                throw new Exception("Forbidden");
            }

            if (value <= 0)
            {
                throw new Exception("Value is incorrect");
            }

            try
            {
                database.ChangeValue(bankAccount, 0 - value);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public bool PutMoney(decimal value)
        {
            if (!Authenticated)
            {
                throw new Exception("Forbidden");
            }

            if (value <= 0)
            {
                throw new Exception("Value is incorrect");
            }

            try
            {
                database.ChangeValue(bankAccount, value);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public decimal CheckValue()
        {
            return bankAccount.Value;
        }

        public void Dispose()
        {
            database.DisposeAsync();
        }
    }
}
