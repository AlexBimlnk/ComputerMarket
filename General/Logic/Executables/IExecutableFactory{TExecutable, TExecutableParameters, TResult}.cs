using General.Logic.Commands;

namespace General.Logic.Executables;

/// <summary xml:lang = "ru">
/// Описывает фабрику для создания исполняемых сущностей.
/// </summary>
/// <typeparam name="TExecutable" xml:lang = "ru">Тип исполяемой сущность.</typeparam>
/// <typeparam name="TExecutableParameters" xml:lang = "ru">Тип параметров сущности <typeparamref name="TExecutable"/>.</typeparam>
/// <typeparam name="TResult" xml:lang = "ru">Тип результата вовзращаемого сущностью <typeparamref name="TExecutable"/>.</typeparam>
public interface IExecutableFactory<TExecutable, TExecutableParameters, TResult>
    where TExecutable : IExecutable<TResult>
    where TResult : IExecutableResult
    where TExecutableParameters : ExecutableParametersBase
{
    /// <summary xml:lang = "ru">
    /// Создаёт на основе параметров команду типа <typeparamref name="TExecutable"/>. 
    /// </summary>
    /// <param name="parameters" xml:lang = "ru">
    /// Параметры команды.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Команду типа <typeparamref name="TExecutable"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="parameters"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если параметры команды имеют неизвестный тип.
    /// </exception>
    public TExecutable Create(TExecutableParameters parameters);
}
