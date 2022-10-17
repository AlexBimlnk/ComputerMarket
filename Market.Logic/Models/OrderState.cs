namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Состояние заказа.
/// </summary>
public enum OrderState
{
    /// <summary xml:lang = "ru">
    /// Создан.
    /// </summary>
    Initialize,

    /// <summary xml:lang = "ru">
    /// Отменён.
    /// </summary>
    Cancel,

    /// <summary xml:lang = "ru">
    /// Ожидает оплаты.
    /// </summary>
    PaymentWait,

    /// <summary xml:lang = "ru">
    /// Ожидает ответа от поставщиков.
    /// </summary>
    ProviderAnswerWait,

    /// <summary xml:lang = "ru">
    /// Ожидает доставки всех товаров.
    /// </summary>
    ProductDeliveryWait,

    /// <summary xml:lang = "ru">
    /// Готов.
    /// </summary>
    Ready
}
