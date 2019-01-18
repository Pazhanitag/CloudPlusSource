using System;

namespace CloudPlus.Exceptions.Office365
{
    public class Office365VerifyDomainException : Exception
    {
        public Office365VerifyDomainException()
        {
        }

        public Office365VerifyDomainException(string message)
            : base(message)
        {
        }

        public Office365VerifyDomainException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}