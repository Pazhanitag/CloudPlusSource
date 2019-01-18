using System;

namespace CloudPlus.Exceptions.Catalog
{
    public class NonExistingCompanyCatalogException : Exception
    {
        public NonExistingCompanyCatalogException()
        {
        }

        public NonExistingCompanyCatalogException(string message)
            : base(message)
        {
        }

        public NonExistingCompanyCatalogException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}