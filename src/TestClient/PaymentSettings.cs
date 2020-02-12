// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using Settings.Net.Core;

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
