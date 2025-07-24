using Sber.ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sber.ApiClient.Interfaces
{
    public interface IPayClient
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
}
