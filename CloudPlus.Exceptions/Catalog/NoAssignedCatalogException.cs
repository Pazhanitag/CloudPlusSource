using System;

namespace CloudPlus.Exceptions.Catalog
{
    public class NoAssignedCatalogException : Exception
    {
        public NoAssignedCatalogException()
        {
        }

        public NoAssignedCatalogException(string message)
            : base(message)
        {
        }

        public NoAssignedCatalogException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}