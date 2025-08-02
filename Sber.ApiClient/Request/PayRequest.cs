using System;
using System.Collections.Generic;
using System.Text;

namespace Sber.ApiClient
{
    public class PayRequest
    {
        [Value("orderNumber")]
        public string OrderNumber { get; set; }
        [Value("amount")]
        public long Amount { get; set; }
        [Value("returnUrl")]
        public string ReturnUrl { get; set; }
        [Value("failUrl")]
        public string FailUrl { get; set; }
        [Value("description")]
        public string Description { get; set; }
        [Value("sessionTimeoutSecs")]
        public int SessionTimeoutSecs { get; set; }
        [Value("jsonParams")]
        public string JsonParams { get; set; }
        [Value("payType")]
        public string PayType { get; set; }
        [Value("currency")]
        public string Currency { get; set; } = "RUB";
    }
}
