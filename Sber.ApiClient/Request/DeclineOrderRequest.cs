using System;
using System.Collections.Generic;
using System.Text;

namespace Sber.ApiClient
{
    public class DeclineOrderRequest
    {
        [Value("orderNumber")]
        public string OrderNumber { get; set; }
    }
    public class DeclineOrderRequestYk
    {
        public string PaymentId { get; set; }
    }
}
