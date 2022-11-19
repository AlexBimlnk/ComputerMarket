using General.Logic.Commands;

namespace Import.Logic.Abstractions;

/// <summary>
/// Описывает результат выполнения комманды с возвращаемым значением.
/// </summary>
/// <typeparam name="TEntity">
/// Тип данных которые будет содержать результат комманды.
/// </typeparam>
public interface ICommandCallbackResult<TEntity> : ICommandResult where TEntity : class
{
    /// <summary xml:lang="ru">
    /// Результат вовзращаемый коммандой.
    /// </summary>
    public TEntity? Result { get; }
}
