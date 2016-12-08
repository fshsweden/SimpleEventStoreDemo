using System;

namespace SimpleEventStoreDemo
{

    public interface IEvent
    {
        Guid Id { get; }
    }

    public class AccountCreatedEvent : IEvent
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public AccountCreatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class FundsDespoitedEvent : IEvent
    {

        public Guid Id { get; private set; }
        public Decimal Amount { get; private set; }

        public FundsDespoitedEvent(Guid id, decimal amount)
        {
            Id = id;
            Amount = amount;
        }

    }

    public class FundsWithdrawedEvent : IEvent
    {

        public Guid Id { get; private set; }
        public Decimal Amount { get; private set; }

        public FundsWithdrawedEvent(Guid id, decimal amount)
        {
            Id = id;
            Amount = amount;
        }


    }
}
