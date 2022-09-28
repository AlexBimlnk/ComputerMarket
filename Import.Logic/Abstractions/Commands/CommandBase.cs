using Import.Logic.Commands;

namespace Import.Logic.Abstractions.Commands;

/// <summary xml:lang = "ru">
/// Абстрактная команда создания связей.
/// </summary>
public abstract class CommandBase : ICommand
{

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public abstract CommandID Id { get; }

    protected abstract Task ExecuteCoreAsync();

    /// <inheritdoc/>
    public async Task<CommandResult> ExecuteAsync()
    {
        try
        {
            await ExecuteCoreAsync();
            return CommandResult.Success(Id);
        }
        catch (InvalidOperationException ex)
        {
            return CommandResult.Fail(Id, ex.Message);
        }
    }
}