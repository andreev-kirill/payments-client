using Sber.ApiClient.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Generic;

namespace Sber.ApiClient
{
    public interface ISberApiClient
    {
        /// <summary>
        /// Оплата заказа
        /// </summary>
        /// <param name="request">Данные заказа</param>
        /// <returns></returns>
        Task<Order> RegisterPay(PayRequest request);
        /// <summary>
        /// Статус заказа
        /// </summary>
        /// <param name="request">номер заказа в системе магазина</param>
        /// <returns></returns>
        Task<OrderStatus> GetStatus(OrderStatusRequest request);
        /// <summary>
        /// Проверка оплачен ли заказ
        /// </summary>
        /// <param name="orderNumber">номер заказа в системе магазина</param>
        /// <returns></returns>
        Task<bool> IsOrderPaid(string orderNumber);
        /// <summary>
        /// Запрос возврата на полную сумму в деньгах
        /// </summary>
        /// <param name="request">id заказа в системе экваэринга, сумма возврата</param>
        /// <returns></returns>
        Task<ResponseCode> Refund(RefundRequest request);
        /// <summary>
        /// Запрос возврата на полную сумму в деньгах
        /// </summary>
        /// <param name="orderNumber">номер заказа в системе магазина</param>
        /// <param name="amount">сумма возврата</param>
        /// <returns></returns>
        Task<ResponseCode> Refund(string orderNumber, long amount);
        /// <summary>
        /// Запрос отмены оплаты заказа
        /// </summary>
        /// <param name="request">id заказа в системе экваэринга, сумма возврата</param>
        /// <returns></returns>
        Task<ResponseCode> Reverse(ReverseRequest request);
        /// <summary>
        /// Запрос отмены оплаты заказа
        /// </summary>
        /// <param name="orderNumber">номер заказа в системе магазина</param>
        /// <param name="amount">сумма возврата</param>
        /// <returns></returns>
        Task<ResponseCode> Reverse(string orderNumber, long amount);
        /// <summary>
        /// Запрос отмены неоплаченного заказа
        /// </summary>
        /// <param name="request">номер заказа в системе магазина</param>
        /// <returns></returns>
        Task<ResponseCode> Decline(DeclineOrderRequest request);
    }
    public class SberApiClient : ISberApiClient
    {
        private readonly string login;
        private readonly string pass;
        private readonly HttpClient httpClient;
        public SberApiClient(string baseUrl, string login, string pass)
        {
            this.login = login;
            this.pass = pass;
            //var httpClientHandler = new HttpClientHandler();
            //httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            //{
            //    //тут нужно проверить сертификат..... взять его из стора нашего?)
            //    return true;
            //};
            httpClient = new HttpClient() { BaseAddress = new Uri(baseUrl), Timeout = TimeSpan.FromSeconds(10) };
        }
        public async Task<Order> RegisterPay(PayRequest request)
        {
            var parameters = request.ToKeyValuePair(
                new[]{new KeyValuePair<string, string>("userName", login),
                    new KeyValuePair<string, string>("password", pass)});
            var responce = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, @$"/payment/rest/register.do")
            {
                Content = new FormUrlEncodedContent(parameters)
            });
            responce.EnsureSuccessStatusCode();
            var stringRequest = await responce.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Order>(stringRequest);
            return result;
        }
        public async Task<OrderStatus> GetStatus(OrderStatusRequest request)
        {
            var parameters = request.ToKeyValuePair(
                new[]{new KeyValuePair<string, string>("userName", login),
                    new KeyValuePair<string, string>("password", pass)});
            var responce = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, @$"/payment/rest/getOrderStatusExtended.do")
            {
                Content = new FormUrlEncodedContent(parameters)
            });
            responce.EnsureSuccessStatusCode();
            var stringRequest = await responce.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OrderStatus>(stringRequest);
            return result;
        }
        public async Task<ResponseCode> Refund(RefundRequest request)
        {
            var parameters = request.ToKeyValuePair(
                new KeyValuePair<string, string>("userName", login),
                    new KeyValuePair<string, string>("password", pass));
            var responce = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, @$"/payment/rest/refund.do")
            {
                Content = new FormUrlEncodedContent(parameters)
            });
            responce.EnsureSuccessStatusCode();
            var stringRequest = await responce.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseCode>(stringRequest);
            return result;
        }
        public async Task<ResponseCode> Refund(string orderNumber, long amount)
        {
            var status = await GetStatus(new OrderStatusRequest() { OrderNumber = orderNumber });
            return await Refund(new RefundRequest() { OrderId = status.GetOrderId(), Amount = amount });
        }

        public async Task<ResponseCode> Reverse(string orderNumber, long amount)
        {
            var status = await GetStatus(new OrderStatusRequest() { OrderNumber = orderNumber });
            return await Reverse(new ReverseRequest() { OrderId = status.GetOrderId(), Amount=amount });
        }

        public async Task<ResponseCode> Reverse(ReverseRequest request)
        {
            var parameters = request.ToKeyValuePair(
                new KeyValuePair<string, string>("userName", login),
                    new KeyValuePair<string, string>("password", pass));
            var responce = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, @$"/payment/rest/reverse.do")
            {
                Content = new FormUrlEncodedContent(parameters)
            });
            responce.EnsureSuccessStatusCode();
            var stringRequest = await responce.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseCode>(stringRequest);
            return result;
        }

        public async Task<ResponseCode> Decline(DeclineOrderRequest request)
        {
            var parameters = request.ToKeyValuePair(
                new[]{new KeyValuePair<string, string>("userName", login),
                    new KeyValuePair<string, string>("password", pass)});
            var responce = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, @$"/payment/rest/decline.do")
            {
                Content = new FormUrlEncodedContent(parameters)
            });
            responce.EnsureSuccessStatusCode();
            var stringRequest = await responce.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseCode>(stringRequest);
            return result;
        }

        public async Task<bool> IsOrderPaid(string orderNumber)
        {
            var status = await GetStatus(new OrderStatusRequest() { OrderNumber = orderNumber });
            return status.IsOrderPaid();
        }
    }
}