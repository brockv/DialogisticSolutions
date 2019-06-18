namespace Dialogistic.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }   
}
