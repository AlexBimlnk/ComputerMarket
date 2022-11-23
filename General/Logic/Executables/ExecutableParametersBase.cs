namespace General.Logic.Executables;

/// <summary xml:lang = "ru">
/// Базовый тип параметров исполняемой сущности.
/// </summary>
public abstract class ExecutableParametersBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="ExecutableParametersBase"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор сущности.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда <paramref name="id"/> оказался <see langword="null"/>.
    /// </exception>
    protected ExecutableParametersBase(ExecutableID id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор исполняемой сущности.
    /// </summary>
    public ExecutableID Id { get; }
}
