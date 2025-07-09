using Sber.ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sber.ApiClient
{
    public class YookassaClient : ISberApiClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        public YookassaClient(IHttpClientFactory httpClientFactory) {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public Task<ResponseCode> Decline(DeclineOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<OrderStatus> GetStatus(OrderStatusRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsOrderPaid(string orderNumber)
        {
            using var client = httpClientFactory.CreateClient("httpclient");
            var responce = await client.GetFromJsonAsync<PaymentObjectList>("payments?status=success");

        }

        public Task<ResponseCode> Refund(RefundRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseCode> Refund(string orderNumber, long amount)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> RegisterPay(PayRequest request)
        {
            using var client = httpClientFactory.CreateClient("httpclient");
            var message = new HttpRequestMessage(HttpMethod.Post, "payments");
            message.Headers.Add("Content-Type", "application/json");
            message.Headers.Add("Idempotence-Key", request.OrderNumber);
            message.Content = JsonContent.Create(new {
            amount = new { 
            value = (decimal)request.Amount / (decimal)100,
                currency = request.Currency,
                payment_method_data = new { type = request.PayType },
                confirmation = new { type = "redirect", return_url = request.ReturnUrl },
                description = request.Description,
                capture = true
            }
            });
            var responce = await client.SendAsync(message);
            if (!responce.IsSuccessStatusCode)
            {
                return new Order() { 
                    ErrorMessage = await responce.Content.ReadAsStringAsync(), 
                    ErrorCode = "-1" 
                };
            }

            var responceData = await responce.Content.ReadFromJsonAsync<PayObject>();
            return new Order() { OrderId = responceData.id, FormUrl = responceData.confirmation.confirmation_url };
        }

        public Task<ResponseCode> Reverse(ReverseRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseCode> Reverse(string orderNumber, long amount)
        {
            throw new NotImplementedException();
        }
    }
}
