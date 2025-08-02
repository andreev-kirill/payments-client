using Sber.ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sber.ApiClient.Interfaces
{
    public interface IPayClientYk
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
        Task<OrderStatus> GetStatus(OrderStatusRequestYk request);
        /// <summary>
        /// Проверка оплачен ли заказ
        /// </summary>
        /// <param name="paymentId">id платёжки</param>
        /// <returns></returns>
        Task<bool> IsOrderPaid(string paymentId);
        /// <summary>
        /// Запрос возврата на полную сумму в деньгах
        /// </summary>
        /// <param name="request">id заказа в системе экваэринга, сумма возврата</param>
        /// <returns></returns>
        Task<ResponseCode> Refund(RefundRequestYk request);
        /// <summary>
        /// Запрос отмены неоплаченного заказа
        /// </summary>
        /// <param name="request">номер заказа в системе магазина</param>
        /// <returns></returns>
        Task<ResponseCode> Decline(DeclineOrderRequestYk request);
    }
}
