using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sber.ApiClient.Models
{
    public class ResponseCode
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
