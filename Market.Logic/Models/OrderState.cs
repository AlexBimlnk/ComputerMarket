namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Состояние заказа.
/// </summary>
public enum OrderState : int
{
    /// <summary xml:lang = "ru">
    /// Отменён.
    /// </summary>
    Cancel = 1,

    /// <summary xml:lang = "ru">
    /// Ожидает оплаты.
    /// </summary>
    PaymentWait = 2,

    /// <summary xml:lang = "ru">
    /// Ожидает ответа от поставщиков.
    /// </summary>
    ProviderAnswerWait = 3,

    /// <summary xml:lang = "ru">
    /// Ожидает доставки всех товаров.
    /// </summary>
    ProductDeliveryWait = 4,

    /// <summary xml:lang = "ru">
    /// Готов.
    /// </summary>
    Ready = 5
}
