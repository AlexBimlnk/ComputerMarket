using General.Logic.Commands;

namespace General.Logic.Executables;

public interface IExecutableFactory<TExecutable, TExecutableParameters, TResult>
    where TExecutable : IExecutable<TResult> 
    where TResult : IExecutableResult
    where TExecutableParameters : ExecutableParametersBase
{
    /// <summary xml:lang = "ru">
    /// Создаёт на основе параметров команду типа <see cref="ICommand"/>. 
    /// </summary>
    /// <param name="parameters" xml:lang = "ru">
    /// Параметры команды.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Команду типа <see cref="ICommand"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="parameters"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если параметры команды имеют неизвестный тип.
    /// </exception>
    public TExecutable Create(TExecutableParameters parameters);
}
