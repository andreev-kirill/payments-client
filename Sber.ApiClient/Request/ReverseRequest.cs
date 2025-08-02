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
    //отмена ссылки
    public class ReverseRequestYk
    {
        public string PaymentId { get; set; }
        public long Amount { get; set; }
    }
}
