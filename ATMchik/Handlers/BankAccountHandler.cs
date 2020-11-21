using ATMchik.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ATMchik.Handlers
{
    public static class BankAccountHandler
    {
        public static BankAccount CreateBankAccount(string owner, bool createCard = true)
        {
            var pvvCode = VerificationValuesHandler.GeneratePvvCode();
            var pvvHash = VerificationValuesHandler.GetPvvHash(pvvCode);
            var cardNumber = VerificationValuesHandler.GenerateCardNumber();
            var bankAccountNumber = VerificationValuesHandler.GenerateBankAccountNumber();
            var cvvHash = VerificationValuesHandler.GetCvvHash(cardNumber, bankAccountNumber);

            var bankAccount = new BankAccount()
            {
                BankAccountNumber = bankAccountNumber,
                CardNumber = cardNumber,
                PvvHash = pvvHash,
                CvvHash = cvvHash,
                Owner = owner,
                Value = 0
            };

            using ApplicationContext db = new ApplicationContext();
            db.AddBankAccountAsync(bankAccount);

            if (createCard)
            {
                CreateCard(cardNumber, bankAccountNumber, pvvCode, cvvHash, owner);
            }

            return bankAccount;
        }

        private static Card CreateCard(ulong cardNumber, ulong bankAccountNumber, uint pvvCode, string cvvHash, string owner)
        {
            var card = new Card()
            {
                CardNumber = cardNumber,
                BankAccountNumber = bankAccountNumber,
                CvvHash = cvvHash,
                Owner = owner
            };

            var cardJson = JsonConvert.SerializeObject(card, Formatting.Indented);
            var cardFilePath = "cards\\" + cardNumber + ".crd";
            var cardInfoFilePath = "cards\\" + cardNumber + "-" + pvvCode + ".txt";

            File.WriteAllText(cardFilePath, cardJson);
            File.WriteAllText(cardInfoFilePath, string.Empty);

            return card;
        }
    }
}
