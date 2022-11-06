using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sber.ApiClient.Models
{
    public class OrderStatus
    {
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }
        [JsonProperty("orderStatus")]
        public int OrderPayStatus { get; set; }
        [JsonProperty("attributes")]
        public OrderAttribute[] Attributes { get; set; }
        [JsonProperty("actionCodeDescription")]
        public string ActionCodeDescription { get; set; }
        [JsonProperty("amount")] 
        public int? Amount { get; set; }
        [JsonProperty("depositedDate")]
        public int? DepositedDate { get; set; }
    }
    public class OrderAttribute
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
