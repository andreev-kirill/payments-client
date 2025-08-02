using System;
using System.Collections.Generic;
using System.Text;

namespace Sber.ApiClient
{
    public class OrderStatusRequest
    {
        [Value("orderNumber")]
        public string OrderNumber { get; set; }
    }
    public class OrderStatusRequestYk
    {
        public string PaymentId { get; set; }
    }
}
