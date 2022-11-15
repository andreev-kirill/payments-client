using System;
using System.Collections.Generic;
using System.Text;

namespace Sber.ApiClient
{
    public class ReverseRequest
    {
        [Value("orderId")]
        public string OrderId { get; set; }
        [Value("amount")]
        public long Amount { get; set; }
    }
}
