using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sber.ApiClient.Models
{
    public class Order : ResponseCode
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        [JsonProperty("formUrl")]
        public string FormUrl { get; set;}        
    }
}
