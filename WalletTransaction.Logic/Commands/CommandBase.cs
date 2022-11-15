using General.Logic.Commands;

namespace WalletTransaction.Logic.Commands;

/// <summary xml:lang = "ru">
/// Абстрактная команда.
/// </summary>
public abstract class CommandBase : ICommand
{
    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public abstract CommandID Id { get; }

    protected abstract Task ExecuteCoreAsync();

    /// <inheritdoc/>
    public async Task<ICommandResult> ExecuteAsync()
    {
        try
        {
            await ExecuteCoreAsync()
                .ConfigureAwait(false);

            return CommandResult.Success(Id);
        }
        catch (InvalidOperationException ex)
        {
            return CommandResult.Fail(Id, ex.Message);
        }
    }
}