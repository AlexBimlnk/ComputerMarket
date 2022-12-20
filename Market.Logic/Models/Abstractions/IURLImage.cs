namespace Market.Logic.Models.Abstractions;

/// <summary>
/// Интерфейс реализующий возможность иметь ссылку на изображение.
/// </summary>
public interface IURLImage
{
    /// <summary>
    /// ССылка на изображение.
    /// </summary>
    public string? URL { get; }
}
