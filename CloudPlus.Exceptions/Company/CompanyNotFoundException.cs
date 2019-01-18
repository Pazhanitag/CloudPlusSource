using System;

namespace CloudPlus.Exceptions.Company
{
    public class CompanyNotFoundException : Exception
    {
        public CompanyNotFoundException()
        {
        }

        public CompanyNotFoundException(string message)
            : base(message)
        {
        }

        public CompanyNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}