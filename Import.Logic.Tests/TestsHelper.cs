using Import.Logic.Models;

namespace Import.Logic.Tests;

/// <summary xml:lang = "ru">
/// Вспомогательный класс для построения сложных тестов.
/// </summary>
public static class TestsHelper
{
    public static Product MappedTo(this Product product, InternalID internalID)
    {
        product.MapTo(internalID);
        return product;
    }
}
