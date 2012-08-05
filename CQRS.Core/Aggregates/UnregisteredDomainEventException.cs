using System;

namespace CQRS.Core.Aggregates
{
    public class UnregisteredDomainEventException : Exception
    {
        public UnregisteredDomainEventException(string message) : base(message)
        {
            
        }
    }
}