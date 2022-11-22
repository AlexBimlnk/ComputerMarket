using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Logic.Executables;
public interface IExecutableResult
{
    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public ExecutableID Id { get; }

    /// <summary xml:lang = "ru">
    /// Сообщение содержащие описание ошибки.
    /// </summary>
    public string? ErrorMessage { get; }
}
