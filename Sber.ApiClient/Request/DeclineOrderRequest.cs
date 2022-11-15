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
}
