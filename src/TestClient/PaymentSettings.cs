using System;
using System.Collections.Generic;
using System.Text;
using ApplicationSettings.Core;

namespace TestClient
{

    public class Currency
    {
        public string Name { get; set; }

        public string Code { get; set; }
    }

    public class PaymentSettings
    {
        public Currency BaseCurrency { get; set; }

        public int MaxPaymentRetries { get; set; }


    }
}
