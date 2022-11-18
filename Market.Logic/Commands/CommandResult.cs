using General.Logic.Commands;

namespace Market.Logic.Commands;

/// <summary xml:lang = "ru">
/// Результат выполнения команды.
/// </summary>
public sealed class CommandResult : ICommandResult
{
    public CommandResult(CommandID id, string? errorMessge)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));

        ErrorMessage = errorMessge;
    }

    /// <inheritdoc/>
    public CommandID Id { get; }

    /// <inheritdoc/>
    public bool IsSuccess => ErrorMessage is null;

    /// <inheritdoc/>
    public string? ErrorMessage { get; }
}