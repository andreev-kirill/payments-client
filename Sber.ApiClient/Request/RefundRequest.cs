using System;
using System.Collections.Generic;
using System.Text;

namespace Sber.ApiClient
{
    public class RefundRequest
    {
        [Value("orderId")]
        public string OrderId { get; set; }
        [Value("amount")] 
        public long Amount { get; set; }
    }
    //возврат
    public class RefundRequestYk
    {
        public string PaymentId { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; } = "RUB";
    }
}
