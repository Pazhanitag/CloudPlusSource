using System;

namespace CloudPlus.Exceptions.Catalog
{
    public class NoProductInProductCatalogException : Exception
    {
        public NoProductInProductCatalogException()
        {
        }

        public NoProductInProductCatalogException(string message)
            : base(message)
        {
        }

        public NoProductInProductCatalogException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}