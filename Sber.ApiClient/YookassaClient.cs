using Sber.ApiClient.Interfaces;
using Sber.ApiClient.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Sber.ApiClient
{
    public class YookassaClient : IPayClientYk
    {
        private readonly IHttpClientFactory httpClientFactory;
        public YookassaClient(IHttpClientFactory httpClientFactory) {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<ResponseCode> Decline(DeclineOrderRequestYk request)
        {
            //Отменяет платеж, находящийся в статусе waiting_for_capture.
            using var client = httpClientFactory.CreateClient("httpclient");
            var responce = await client.PostAsJsonAsync($"payments/{request.PaymentId}/cancel", new { });
            var pay = await responce.Content.ReadFromJsonAsync<PayObject>();
            return new ResponseCode()
            {
                ErrorCode = pay.status != "succeeded" ? pay.status : null,
                ErrorMessage = pay.status != "succeeded" ? pay.status : null
            };
        }

        public async Task<OrderStatus> GetStatus(OrderStatusRequestYk request)
        {
            using var client = httpClientFactory.CreateClient("httpclient");
            var responce = await client.GetFromJsonAsync<PayObject>($"payments/{request.PaymentId}");
            return new OrderStatus
            {
                //pending, waiting_for_capture, succeeded
                //0 - заказ зарегистрирован, но не оплачен;
                //1 - предавторизованная сумма удержана(для двухстадийных платежей);
                //2 - проведена полная авторизация суммы заказа;
                //3 - авторизация отменена;
                //4 - по транзакции была проведена операция возврата;
                //5 - инициирована авторизация через сервер контроля доступа банка-эмитента;
                //6 - авторизация отклонена.
                OrderPayStatus = responce.status == "pending" ? 0 
                : responce.status == "waiting_for_capture" ? 1 
                : responce.status == "succeeded" ? 2 
                : throw new Exception(responce.status)
            };
        }

        public async Task<bool> IsOrderPaid(string paymentId)
        {
            using var client = httpClientFactory.CreateClient("httpclient");
            var responce = await client.GetFromJsonAsync<PayObject>($"payments/{paymentId}");
            return responce.paid && !responce.refundable;
        }

        public async Task<ResponseCode> Refund(RefundRequestYk request)
        {
            using var client = httpClientFactory.CreateClient("httpclient");
            var responce = await client.PostAsJsonAsync($"refunds/{request.PaymentId}",
                new
                {
                    amount = new
                    {
                        value = (decimal)request.Amount / (decimal)100,
                        currency = request.Currency
                    },
                    payment_id = request.PaymentId
                });
            var pay = await responce.Content.ReadFromJsonAsync<PayObject>();
            return new ResponseCode()
            {
                ErrorCode = pay.status != "succeeded" ? pay.status : null,
                ErrorMessage = pay.status != "succeeded" ? pay.status : null
            };
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
    }
}
