using System;
using System.Collections.Generic;

namespace SimpleEventStoreDemo
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CurrentBalance { get; set; }
        public List<Transcation> Transcations = new List<Transcation>();

        public BankAccount() { }

        public void Apply(AccountCreatedEvent @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            CurrentBalance = 0;
        }

        public void Apply(FundsDespoitedEvent @event)
        {
            var newTranscation = new Transcation { Id = @event.Id, Amount = @event.Amount };
            Transcations.Add(newTranscation);
            CurrentBalance = CurrentBalance + @event.Amount;
        }

        public void Apply(FundsWithdrawedEvent @event)
        {
            var newTranscation = new Transcation { Id = @event.Id, Amount = @event.Amount };
            Transcations.Add(newTranscation);
            CurrentBalance = CurrentBalance - @event.Amount;
        }

    }



    public class Transcation
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }

    }
}
