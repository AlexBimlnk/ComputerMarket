using General.Logic.Commands;

using Import.Logic.Commands;

namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Абстрактная команда возвращающая результат.
/// </summary>
public abstract class CallbackCommandBase<TEntity> : CommandBase where TEntity : class
{
    protected TEntity? Result { get; set; }

    /// <inheritdoc/>
    public new async Task<ICommandResult> ExecuteAsync()
    {
        try
        {
            await ExecuteCoreAsync()
                .ConfigureAwait(false);

            return CommandCallbackResult<TEntity>.Success(Id, Result ?? throw new InvalidOperationException("Result of command execution is null"));
        }
        catch (InvalidOperationException ex)
        {
            return CommandCallbackResult<TEntity>.Fail(Id, ex.Message);
        }
    }
}
