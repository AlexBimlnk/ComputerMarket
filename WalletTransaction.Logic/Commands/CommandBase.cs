using General.Logic.Commands;
using General.Logic.Executables;

namespace WalletTransaction.Logic.Commands;

/// <summary xml:lang = "ru">
/// Абстрактная команда.
/// </summary>
public abstract class CommandBase : ICommand
{
    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public abstract ExecutableID Id { get; }

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