using Import.Logic.Commands;

namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Абстрактная команда создания связей.
/// </summary>
public abstract class Command : ICommand
{
    protected Command(CommandID id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public CommandID Id { get; }

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