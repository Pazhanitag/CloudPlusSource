using System;

namespace CloudPlus.Api.Office365.Attributes
{
    public class Office365Name : Attribute
    {
        public string O365PropertyName { get; set; }
    }
}
